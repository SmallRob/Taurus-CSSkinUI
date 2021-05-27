
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;

namespace Com_CSSkin.SkinControl
{
    [ToolboxItem(false)]
    public partial class SkinDropDown : ToolStripDropDown
    {
        private Control m_popedContainer;
        private ToolStripControlHost m_host;        
        private bool m_fade = true;

        public SkinDropDown(Control popedControl)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.BackColor = Color.Transparent;
            this.Opacity = 0.9;
            if (popedControl == null)
                throw new ArgumentNullException("content");

            this.m_popedContainer = popedControl;

            this.m_fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;

            this.m_host = new ToolStripControlHost(popedControl);
            m_host.AutoSize = false;//make it take the same room as the poped control
       
            Padding = Margin = m_host.Padding = m_host.Margin = Padding.Empty;
            
            popedControl.Location = Point.Empty;
            
            this.Items.Add(m_host);
            
            popedControl.Disposed += delegate(object sender, EventArgs e)
            {
                popedControl = null;
                Dispose(true);// this popup container will be disposed immediately after disposion of the contained control
            };
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            int result = NativeMethods.SetClassLong(this.Handle, GCL.GCL_STYLE, 0);
        }
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == Win32.WM_PAINT)
        //    {
        //       // GetBitmaps();
        //    }
        //    else
        //    {
        //        base.WndProc(ref m);
        //    }
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {//prevent alt from closing it and allow alt+menumonic to work
            if ((keyData & Keys.Alt) == Keys.Alt)
                return false;

            return base.ProcessDialogKey(keyData);
        }


        
        
     
        
        public void Show(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
        
            Show(control, control.ClientRectangle);
        }

        public void Show(Form f, Point p)
        {
            this.Show(f, new Rectangle(p,new Size(0,0)));

        }

        private void Show(Control control, Rectangle area)
        {
            if (control == null)
                throw new ArgumentNullException("control");
           

            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            
            Rectangle screen = Screen.FromControl(control).WorkingArea;
            
            if (location.X + Size.Width > (screen.Left + screen.Width))
                location.X = (screen.Left + screen.Width) - Size.Width;
            
            if (location.Y + Size.Height > (screen.Top + screen.Height))
                location.Y -= Size.Height + area.Height;
                    
            location = control.PointToClient(location);
            
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        
        
        
        private const int frames = 5;
        private const int totalduration = 100;
        private const int frameduration = totalduration / frames;

        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && m_fade) Opacity = 0;
            base.SetVisibleCore(visible);
            if (!visible || !m_fade) return;
            for (int i = 1; i <= frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(frameduration);
                }
                Opacity = opacity * (double)i / (double)frames;
            }
            Opacity = opacity;
        }


               

        protected override void OnOpening(CancelEventArgs e)
        {
            if (m_popedContainer.IsDisposed || m_popedContainer.Disposing)
            {
                e.Cancel = true;
                return;
            }
            base.OnOpening(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            m_popedContainer.Focus();
            
            base.OnOpened(e);
        }
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            base.OnClosed(e);            
        }
    }
}
