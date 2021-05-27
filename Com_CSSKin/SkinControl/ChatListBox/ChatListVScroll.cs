
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinControl
{
    public class ChatListVScroll
    {   //滚动条自身的区域
        private Rectangle bounds;
        public Rectangle Bounds
        {
            get { return bounds; }
        }
        //上边箭头区域
        private Rectangle upBounds;
        public Rectangle UpBounds
        {
            get { return upBounds; }
        }
        //下边箭头区域
        private Rectangle downBounds;
        public Rectangle DownBounds
        {
            get { return downBounds; }
        }
        //滑块区域
        private Rectangle sliderBounds;
        public Rectangle SliderBounds
        {
            get { return sliderBounds; }
        }

        private Color backColor;
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        private Color sliderDefaultColor;
        public Color SliderDefaultColor
        {
            get { return sliderDefaultColor; }
            set
            {
                if (sliderDefaultColor == value)
                    return;
                sliderDefaultColor = value;
                ctrl.Invalidate(this.sliderBounds);
            }
        }

        private Color sliderDownColor;
        public Color SliderDownColor
        {
            get { return sliderDownColor; }
            set
            {
                if (sliderDownColor == value)
                    return;
                sliderDownColor = value;
                ctrl.Invalidate(this.sliderBounds);
            }
        }

        private Color arrowBackColor;
        public Color ArrowBackColor
        {
            get { return arrowBackColor; }
            set
            {
                if (arrowBackColor == value)
                    return;
                arrowBackColor = value;
                ctrl.Invalidate(this.bounds);
            }
        }

        private Color arrowColor;
        public Color ArrowColor
        {
            get { return arrowColor; }
            set
            {
                if (arrowColor == value)
                    return;
                arrowColor = value;
                ctrl.Invalidate(this.bounds);
            }
        }
        //绑定的控件
        private Control ctrl;
        public Control Ctrl
        {
            get { return ctrl; }
            set { ctrl = value; }
        }
        //虚拟的一个高度(控件中内容的高度)
        private int virtualHeight;
        public int VirtualHeight
        {
            get { return virtualHeight; }
            set
            {
                if (value <= ctrl.Height)
                {
                    if (shouldBeDraw == false)
                        return;
                    shouldBeDraw = false;
                    if (this.value != 0)
                    {
                        this.value = 0;
                        ctrl.Invalidate();
                    }
                }
                else
                {
                    shouldBeDraw = true;
                    if (value - this.value < ctrl.Height)
                    {
                        this.value -= ctrl.Height - value + this.value;
                        ctrl.Invalidate();
                    }
                }
                virtualHeight = value;
            }
        }
        //当前滚动条位置
        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                if (!shouldBeDraw)
                    return;
                if (value < 0)
                {
                    if (this.value == 0)
                        return;
                    this.value = 0;
                    ctrl.Invalidate();
                    return;
                }
                if (value > virtualHeight - ctrl.Height)
                {
                    if (this.value == virtualHeight - ctrl.Height)
                        return;
                    this.value = virtualHeight - ctrl.Height;
                    ctrl.Invalidate();
                    return;
                }
                this.value = value;
                ctrl.Invalidate();
            }
        }
        //是否有必要在控件上绘制滚动条
        private bool shouldBeDraw;
        public bool ShouldBeDraw
        {
            get { return shouldBeDraw; }
        }

        private bool isMouseDown;
        public bool IsMouseDown
        {
            get { return isMouseDown; }
            set
            {
                if (value)
                {
                    m_nLastSliderY = sliderBounds.Y;
                }
                isMouseDown = value;
            }
        }

        private bool isMouseOnSlider;
        public bool IsMouseOnSlider
        {
            get { return isMouseOnSlider; }
            set
            {
                if (isMouseOnSlider == value)
                    return;
                isMouseOnSlider = value;
                ctrl.Invalidate(this.SliderBounds);
            }
        }

        private bool isMouseOnUp;
        public bool IsMouseOnUp
        {
            get { return isMouseOnUp; }
            set
            {
                if (isMouseOnUp == value)
                    return;
                isMouseOnUp = value;
                ctrl.Invalidate(this.UpBounds);
            }
        }

        private bool isMouseOnDown;
        public bool IsMouseOnDown
        {
            get { return isMouseOnDown; }
            set
            {
                if (isMouseOnDown == value)
                    return;
                isMouseOnDown = value;
                ctrl.Invalidate(this.DownBounds);
            }
        }
        //鼠标在滑块点下时候的y坐标
        private int mouseDownY;
        public int MouseDownY
        {
            get { return mouseDownY; }
            set { mouseDownY = value; }
        }
        //滑块移动前的 滑块的y坐标
        private int m_nLastSliderY;

        public ChatListVScroll(Control c)
        {
            this.ctrl = c;
            virtualHeight = 400;
            bounds = new Rectangle(0, 0, 5, 5);
            upBounds = new Rectangle(0, 0, 5, 5);
            downBounds = new Rectangle(0, 0, 5, 5);
            sliderBounds = new Rectangle(0, 0, 5, 5);
            this.backColor = Color.FromArgb(50, 224, 239, 235);
            this.sliderDefaultColor = Color.FromArgb(100, 110, 111, 112);
            this.sliderDownColor = Color.FromArgb(200, 110, 111, 112);
            this.arrowBackColor = Color.Transparent;
            this.arrowColor = Color.FromArgb(200, 148, 150, 151);
        }

        public void ClearAllMouseOn()
        {
            if (!this.isMouseOnDown && !this.isMouseOnSlider && !this.isMouseOnUp)
                return;

            this.isMouseOnSlider =
            this.isMouseOnDown =
            this.isMouseOnUp = false;
            ctrl.Invalidate(this.bounds);
        }
        //将滑块跳动至一个地方
        public void MoveSliderToLocation(int nCurrentMouseY)
        {
            if (nCurrentMouseY - sliderBounds.Height / 2 < 11)
                sliderBounds.Y = 11;
            else if (nCurrentMouseY + sliderBounds.Height / 2 > ctrl.Height - 11)
                sliderBounds.Y = ctrl.Height - sliderBounds.Height - 11;
            else
                sliderBounds.Y = nCurrentMouseY - sliderBounds.Height / 2;
            this.value = (int)((double)(sliderBounds.Y - 11) / (ctrl.Height - 22 - SliderBounds.Height) * (virtualHeight - ctrl.Height));
            ctrl.Invalidate();
        }
        //根据鼠标位置移动滑块
        public void MoveSliderFromLocation(int nCurrentMouseY)
        {
            //if (!this.IsMouseDown) return;
            if (m_nLastSliderY + nCurrentMouseY - mouseDownY < 11)
            {
                if (sliderBounds.Y == 11)
                    return;
                sliderBounds.Y = 11;
            }
            else if (m_nLastSliderY + nCurrentMouseY - mouseDownY > ctrl.Height - 11 - SliderBounds.Height)
            {
                if (sliderBounds.Y == ctrl.Height - 11 - sliderBounds.Height)
                    return;
                sliderBounds.Y = ctrl.Height - 11 - sliderBounds.Height;
            }
            else
            {
                sliderBounds.Y = m_nLastSliderY + nCurrentMouseY - mouseDownY;
            }
            this.value = (int)((double)(sliderBounds.Y - 11) / (ctrl.Height - 22 - SliderBounds.Height) * (virtualHeight - ctrl.Height));
            ctrl.Invalidate();
        }
        //绘制滚动条
        public void ReDrawScroll(Graphics g)
        {
            if (!shouldBeDraw)
                return;
            bounds.X = ctrl.Width - 7;
            bounds.Height = ctrl.Height;
            upBounds.X = downBounds.X = bounds.X;
            downBounds.Y = ctrl.Height - 5;
            //计算滑块位置
            sliderBounds.X = bounds.X;
            sliderBounds.Height = (int)(((double)ctrl.Height / virtualHeight) * (ctrl.Height - 22));
            if (sliderBounds.Height < 3) sliderBounds.Height = 3;
            sliderBounds.Y = 11 + (int)(((double)value / (virtualHeight - ctrl.Height)) * (ctrl.Height - 22 - sliderBounds.Height));
            SolidBrush sb = new SolidBrush(this.backColor);
            try
            {
                g.FillRectangle(sb, bounds);
                sb.Color = this.arrowBackColor;
                g.FillRectangle(sb, upBounds);
                g.FillRectangle(sb, downBounds);
                if (this.isMouseDown || this.isMouseOnSlider)
                    sb.Color = this.sliderDownColor;
                else
                    sb.Color = this.sliderDefaultColor;

                g.FillRectangle(sb, sliderBounds);

                //绘制上下箭头
                sb.Color = this.arrowColor;
                if (this.isMouseOnUp)
                {
                    //g.FillPolygon(sb, new Point[]{
                    //new Point(ctrl.Width - 3,3),
                    //new Point(ctrl.Width - 7,7),
                    //new Point(ctrl.Width - 2,7)};
                }
                if (this.isMouseOnDown)
                {
                    //g.FillPolygon(sb, new Point[]{
                    //new Point(ctrl.Width - 5,ctrl.Height - 4),
                    //new Point(ctrl.Width - 8,ctrl.Height - 7),
                    //new Point(ctrl.Width - 2,ctrl.Height - 7)};
                }
            }
            finally
            {
                sb.Dispose();
            }
        }
    }
}
