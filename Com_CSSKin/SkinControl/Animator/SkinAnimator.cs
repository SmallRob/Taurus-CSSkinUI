
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 动画管理器
    /// </summary>
    [ProvideProperty("Decoration", typeof(Control))]
    public class SkinAnimator : Component, IExtenderProvider
    {
        protected List<QueueItem> queue = new List<QueueItem>();
        private Thread thread;

        /// <summary>
        /// 发生在控制动画完成后
        /// </summary>
        [Description("发生在控制动画完成后")]
        public event EventHandler<AnimationCompletedEventArg> AnimationCompleted;
        /// <summary>
        /// 当所有的动画完成时
        /// </summary>
        [Description("当所有的动画完成时")]
        public event EventHandler AllAnimationsCompleted;
        /// <summary>
        /// 当需要变换矩阵时
        /// </summary>
        [Description("当需要变换矩阵时")]
        public event EventHandler<TransfromNeededEventArg> TransfromNeeded;
        /// <summary>
        /// 当需要非线性变换时
        /// </summary>
        [Description("当需要非线性变换时")]
        public event EventHandler<NonLinearTransfromNeededEventArg> NonLinearTransfromNeeded;
        /// <summary>
        /// 当用户点击动画控制时
        /// </summary>
        [Description("当用户点击动画控制时")]
        public event EventHandler<MouseEventArgs> MouseDown;
        /// <summary>
        /// 发生帧动画绘画时
        /// </summary>
        [Description("发生帧动画绘画时")]
        public event EventHandler<PaintEventArgs> FramePainted;

        /// <summary>
        /// 动画的最大时间（毫秒）
        /// </summary>
        [DefaultValue(1500)]
        [Description("动画的最大时间（毫秒）")]
        public int MaxAnimationTime { get; set; }

        /// <summary>
        /// 默认的动画
        /// </summary>
        [Description("默认的动画")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Animation DefaultAnimation { get; set; }

        /// <summary>
        /// 动画控制光标
        /// </summary>
        [Description("动画控制光标")]
        [DefaultValue(typeof(Cursor), "Default")]
        public Cursor Cursor { get; set; }

        /// <summary>
        /// 是否所有动画完成
        /// </summary>
        [Description("是否所有动画完成")]
        public bool IsCompleted
        {
            get { lock (queue) return queue.Count == 0; }
        }

        /// <summary>
        /// 帧之间时间间隔（MS）
        /// </summary>
        [DefaultValue(10)]
        [Description("帧之间时间间隔（MS）")]
        public int Interval
        {
            get;
            set;
        }

        AnimationType animationType;
        /// <summary>
        /// 内置的动画类型
        /// </summary>
        [Description("内置的动画类型")]
        public AnimationType AnimationType
        {
            get { return animationType; }
            set { animationType = value; InitDefaultAnimation(animationType); }
        }

        public SkinAnimator()
        {
            Init();
        }

        public SkinAnimator(IContainer container)
        {
            container.Add(this);
            Init();
        }

        protected virtual void Init()
        {
            DefaultAnimation = new Animation();
            MaxAnimationTime = 1500;
            TimeStep = 0.02f;
            Interval = 10;

            Disposed += new EventHandler(Animator_Disposed);

            //主要工作线程
            thread = new Thread(Work);
            thread.IsBackground = true;
            thread.Start();
        }

        void Animator_Disposed(object sender, EventArgs e)
        {
            ClearQueue();
            thread.Abort();
        }

        void Work()
        {
            while (true)
            {
                Thread.Sleep(Interval);
                try
                {
                    var count = 0;
                    var completed = new List<QueueItem>();
                    var actived = new List<QueueItem>();

                    //找到完成
                    lock (queue)
                    {
                        count = queue.Count;
                        var wasActive = false;

                        foreach (var item in queue)
                        {
                            if (item.IsActive) wasActive = true;

                            if (item.controller != null && item.controller.IsCompleted)
                                completed.Add(item);
                            else
                            {
                                if (item.IsActive)
                                {
                                    if ((DateTime.Now - item.ActivateTime).TotalMilliseconds > MaxAnimationTime)
                                        completed.Add(item);
                                    else
                                        actived.Add(item);
                                }
                            }
                        }
                        //开始下一个动画
                        if (!wasActive)
                            foreach (var item in queue)
                                if (!item.IsActive)
                                {
                                    actived.Add(item);
                                    item.IsActive = true;
                                    break;
                                }
                    }

                    //完成
                    foreach (var item in completed)
                        OnCompleted(item);

                    //建立双位图的下一帧
                    foreach (var item in actived)
                        try
                        {
                            //建立双位图的下一帧
                            item.control.BeginInvoke(new MethodInvoker(() => DoAnimation(item)));
                        }
                        catch
                        {
                            //我们不能启动动画，从队列中删除
                            OnCompleted(item);
                        }

                    if (count == 0)
                    {
                        if (completed.Count > 0)
                            OnAllAnimationsCompleted();
                        CheckRequests();
                    }
                }
                catch
                {
                    //形式是封闭的
                }
            }
        }

        /// <summary>
        /// 检查控制结果状态
        /// </summary>
        private void CheckRequests()
        {
            var toRemove = new List<QueueItem>();

            lock (requests)
            {
                var dict = new Dictionary<Control, QueueItem>();
                foreach (var item in requests)
                    if (item.control != null)
                    {
                        if (dict.ContainsKey(item.control))
                            toRemove.Add(dict[item.control]);
                        dict[item.control] = item;
                    }
                    else
                        toRemove.Add(item);

                foreach (var item in dict.Values)
                {
                    if (item.control != null && !IsStateOK(item.control, item.mode))
                        RepairState(item.control, item.mode);
                    else
                        toRemove.Add(item);
                }

                foreach (var item in toRemove)
                    requests.Remove(item);
            }
        }

        bool IsStateOK(Control control, AnimateMode mode)
        {
            switch (mode)
            {
                case AnimateMode.Hide: return !control.Visible;
                case AnimateMode.Show: return control.Visible;
            }

            return true;
        }

        void RepairState(Control control, AnimateMode mode)
        {
            control.BeginInvoke(new MethodInvoker(() =>
            {
                switch (mode)
                {
                    case AnimateMode.Hide: control.Visible = false; break;
                    case AnimateMode.Show: control.Visible = true; break;
                }
            }));
        }

        private void DoAnimation(QueueItem item)
        {
            if (Monitor.TryEnter(item))
                try
                {
                    if (item.controller == null)
                    {
                        item.controller = CreateDoubleBitmap(item.control, item.mode, item.animation,
                                                             item.clipRectangle);
                    }
                    if (item.controller.IsCompleted)
                        return;
                    item.controller.BuildNextFrame();
                }
                catch
                {
                    OnCompleted(item);
                }
        }

        private void InitDefaultAnimation(Com_CSSkin.SkinControl.AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Custom: break;
                case AnimationType.Rotate: DefaultAnimation = Animation.Rotate; break;
                case AnimationType.HorizSlide: DefaultAnimation = Animation.HorizSlide; break;
                case AnimationType.VertSlide: DefaultAnimation = Animation.VertSlide; break;
                case AnimationType.Scale: DefaultAnimation = Animation.Scale; break;
                case AnimationType.ScaleAndRotate: DefaultAnimation = Animation.ScaleAndRotate; break;
                case AnimationType.HorizSlideAndRotate: DefaultAnimation = Animation.HorizSlideAndRotate; break;
                case AnimationType.ScaleAndHorizSlide: DefaultAnimation = Animation.ScaleAndHorizSlide; break;
                case AnimationType.Transparent: DefaultAnimation = Animation.Transparent; break;
                case AnimationType.Leaf: DefaultAnimation = Animation.Leaf; break;
                case AnimationType.Mosaic: DefaultAnimation = Animation.Mosaic; break;
                case AnimationType.Particles: DefaultAnimation = Animation.Particles; break;
                case AnimationType.VertBlind: DefaultAnimation = Animation.VertBlind; break;
                case AnimationType.HorizBlind: DefaultAnimation = Animation.HorizBlind; break;
            }
        }

        /// <summary>
        /// 时间步长
        /// </summary>
        [DefaultValue(0.02f)]
        [Description("时间步长")]
        public float TimeStep { get; set; }

        /// <summary>
        /// 显示控制。作为结果的控制将显示动画。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param>
        public void Show(Control control, bool parallel = false, Animation animation = null)
        {
            AddToQueue(control, AnimateMode.Show, parallel, animation);
        }

        /// <summary>
        /// 显示控制和等待在动画将完成。作为结果的控制将显示动画。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param>
        public void ShowSync(Control control, bool parallel = false, Animation animation = null)
        {
            Show(control, parallel, animation);
            WaitAnimation(control);
        }

        /// <summary>
        /// 隐藏的控制。作为结果的控制将被隐藏的动画。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param>
        public void Hide(Control control, bool parallel = false, Animation animation = null)
        {
            AddToQueue(control, AnimateMode.Hide, parallel, animation);
        }

        /// <summary>
        /// 隐藏的控制和等待在动画将完成。作为结果的控制将被隐藏的动画。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param>
        public void HideSync(Control control, bool parallel = false, Animation animation = null)
        {
            Hide(control, parallel, animation);
            WaitAnimation(control);
        }

        /// <summary>
        /// 这使得控制快照更新前。它需要更新调用结束。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param>
        /// <param name="clipRectangle">动画剪辑矩形</param>
        public void BeginUpdateSync(Control control, bool parallel = false, Animation animation = null, Rectangle clipRectangle = default(Rectangle))
        {
            AddToQueue(control, AnimateMode.BeginUpdate, parallel, animation, clipRectangle);

            bool wait = false;
            do
            {
                wait = false;
                lock (queue)
                    foreach (var item in queue)
                        if (item.control == control && item.mode == AnimateMode.BeginUpdate)
                            if (item.controller == null)
                                wait = true;

                if (wait)
                    Application.DoEvents();

            } while (wait);
        }

        /// <summary>
        /// 动画视图更新控制。它需要调用开始更新之前。
        /// </summary>
        /// <param name="control">目标控制</param>
        public void EndUpdate(Control control)
        {
            lock (queue)
            {
                foreach (var item in queue)
                {
                    if (item.control == control && item.mode == AnimateMode.BeginUpdate && item.controller != null)
                    {
                        item.controller.EndUpdate();
                        item.mode = AnimateMode.Update;
                    }
                }
            }
        }

        /// <summary>
        /// 更新控制视图与动画等动画将完成时。它需要调用开始更新之前
        /// </summary>
        /// <param name="control">目标控制</param>
        public void EndUpdateSync(Control control)
        {
            EndUpdate(control);
            WaitAnimation(control);
        }

        /// <summary>
        /// 在等待所有的动画将完成。
        /// </summary>
        public void WaitAllAnimations()
        {
            while (!IsCompleted)
                Application.DoEvents();
        }

        /// <summary>
        /// 在等待的控制动画将完成。
        /// </summary>
        /// <param name="animatedControl"></param>
        public void WaitAnimation(Control animatedControl)
        {
            while (true)
            {
                bool flag = false;
                lock (queue)
                    foreach (var item in queue)
                        if (item.control == animatedControl)
                        {
                            flag = true;
                            break;
                        }

                if (!flag)
                    return;

                Application.DoEvents();
            }
        }

        List<QueueItem> requests = new List<QueueItem>();

        void OnCompleted(QueueItem item)
        {
            if (item.controller != null)
            {
                item.controller.Dispose();
            }
            lock (queue)
                queue.Remove(item);

            OnAnimationCompleted(new AnimationCompletedEventArg { Animation = item.animation, Control = item.control, Mode = item.mode });
        }

        /// <summary>
        /// 添加到队列控制动画。
        /// </summary>
        /// <param name="control">目标控制</param>
        /// <param name="mode">动画模式</param>
        /// <param name="parallel">允许动画等动画同时</param>
        /// <param name="animation">个人动画</param> 
        public void AddToQueue(Control control, AnimateMode mode, bool parallel = true, Animation animation = null, Rectangle clipRectangle = default(Rectangle))
        {
            if (animation == null)
                animation = DefaultAnimation;

            if (control is IFakeControl)
            {
                control.Visible = false;
                return;
            }

            var item = new QueueItem() { animation = animation, control = control, IsActive = parallel, mode = mode, clipRectangle = clipRectangle };

            //检查可见状态
            switch (mode)
            {
                case AnimateMode.Show:
                    if (control.Visible)//已经显示
                    {
                        OnCompleted(new QueueItem { control = control, mode = mode });
                        return;
                    }
                    break;
                case AnimateMode.Hide:
                    if (!control.Visible)//已隐藏
                    {
                        OnCompleted(new QueueItem { control = control, mode = mode });
                        return;
                    }
                    break;
            }
            //添加到队列
            lock (queue)
                queue.Add(item);
            lock (requests)
                requests.Add(item);
        }

        private Controller CreateDoubleBitmap(Control control, AnimateMode mode, Animation animation, Rectangle clipRect)
        {
            var controller = new Controller(control, mode, animation, TimeStep, clipRect);
            controller.TransfromNeeded += OnTransformNeeded;
            controller.NonLinearTransfromNeeded += OnNonLinearTransfromNeeded;
            controller.MouseDown += OnMouseDown;
            controller.DoubleBitmap.Cursor = Cursor;
            controller.FramePainted += OnFramePainted;
            return controller;
        }

        void OnFramePainted(object sender, PaintEventArgs e)
        {
            if (FramePainted != null)
                FramePainted(sender, e);
        }

        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //动画控制的坐标变换的点
                var db = (Controller)sender;
                var l = e.Location;
                l.Offset(db.DoubleBitmap.Left - db.AnimatedControl.Left, db.DoubleBitmap.Top - db.AnimatedControl.Top);
                //
                if (MouseDown != null)
                    MouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, l.X, l.Y, e.Delta));
            }
            catch
            {
            }
        }

        protected virtual void OnNonLinearTransfromNeeded(object sender, NonLinearTransfromNeededEventArg e)
        {
            if (NonLinearTransfromNeeded != null)
                NonLinearTransfromNeeded(this, e);
            else
                e.UseDefaultTransform = true;
        }

        protected virtual void OnTransformNeeded(object sender, TransfromNeededEventArg e)
        {
            if (TransfromNeeded != null)
                TransfromNeeded(this, e);
            else
                e.UseDefaultMatrix = true;
        }

        /// <summary>
        /// 清除队列。
        /// </summary>
        public void ClearQueue()
        {
            List<QueueItem> items = null;
            lock (queue)
            {
                items = new List<QueueItem>(queue);
                queue.Clear();
            }


            foreach (var item in items)
            {
                if (item.control != null)
                    item.control.BeginInvoke(new MethodInvoker(() =>
                    {
                        switch (item.mode)
                        {
                            case AnimateMode.Hide: item.control.Visible = false; break;
                            case AnimateMode.Show: item.control.Visible = true; break;
                        }
                    }));
                OnAnimationCompleted(new AnimationCompletedEventArg { Animation = item.animation, Control = item.control, Mode = item.mode });
            }

            if (items.Count > 0)
                OnAllAnimationsCompleted();
        }

        protected virtual void OnAnimationCompleted(AnimationCompletedEventArg e)
        {
            if (AnimationCompleted != null)
                AnimationCompleted(this, e);
        }

        protected virtual void OnAllAnimationsCompleted()
        {
            if (AllAnimationsCompleted != null)
                AllAnimationsCompleted(this, EventArgs.Empty);
        }

        #region Nested type: QueueItem

        protected class QueueItem
        {
            public Animation animation;
            public Controller controller;
            public Control control;
            public DateTime ActivateTime { get; private set; }
            public AnimateMode mode;
            public Rectangle clipRectangle;

            public bool isActive;
            public bool IsActive
            {
                get { return isActive; }
                set
                {
                    if (isActive == value) return;
                    isActive = value;
                    if (value)
                        ActivateTime = DateTime.Now;
                }
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                if (control != null)
                    sb.Append(control.GetType().Name + " ");
                sb.Append(mode);
                return sb.ToString();
            }
        }

        #endregion

        #region IExtenderProvider

        public DecorationType GetDecoration(Control control)
        {
            if (DecorationByControls.ContainsKey(control))
                return DecorationByControls[control].DecorationType;
            else
                return DecorationType.None;
        }

        public void SetDecoration(Control control, DecorationType decoration)
        {
            var wrapper = DecorationByControls.ContainsKey(control) ? DecorationByControls[control] : null;
            if (decoration == DecorationType.None)
            {
                if (wrapper != null)
                    wrapper.Dispose();
                DecorationByControls.Remove(control);
            }
            else
            {
                if (wrapper == null)
                    wrapper = new DecorationControl(decoration, control);
                wrapper.DecorationType = decoration;
                DecorationByControls[control] = wrapper;
            }
        }

        private readonly Dictionary<Control, DecorationControl> DecorationByControls = new Dictionary<Control, DecorationControl>();

        public bool CanExtend(object extendee)
        {
            return extendee is Control;
        }

        #endregion
    }

    public enum DecorationType
    {
        None,
        BottomMirror,
        Custom
    }


    public class AnimationCompletedEventArg : EventArgs
    {
        public Animation Animation { get; set; }
        public Control Control { get; internal set; }
        public AnimateMode Mode { get; internal set; }
    }

    public class TransfromNeededEventArg : EventArgs
    {
        public TransfromNeededEventArg()
        {
            Matrix = new Matrix(1, 0, 0, 1, 0, 0);
        }

        public Matrix Matrix { get; set; }
        public float CurrentTime { get; internal set; }
        public Rectangle ClientRectangle { get; internal set; }
        public Rectangle ClipRectangle { get; internal set; }
        public Animation Animation { get; set; }
        public Control Control { get; internal set; }
        public AnimateMode Mode { get; internal set; }
        public bool UseDefaultMatrix { get; set; }
    }

    public class NonLinearTransfromNeededEventArg : EventArgs
    {
        public float CurrentTime { get; internal set; }

        public Rectangle ClientRectangle { get; internal set; }
        public byte[] Pixels { get; internal set; }
        public int Stride { get; internal set; }

        public Rectangle SourceClientRectangle { get; internal set; }
        public byte[] SourcePixels { get; internal set; }
        public int SourceStride { get; set; }

        public Animation Animation { get; set; }
        public Control Control { get; internal set; }
        public AnimateMode Mode { get; internal set; }
        public bool UseDefaultTransform { get; set; }
    }


    public enum AnimateMode
    {
        Show,
        Hide,
        Update,
        BeginUpdate
    }
}