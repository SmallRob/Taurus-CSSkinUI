
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using System.Drawing.Imaging;
using System.Threading;

namespace Com_CSSkin
{
    public class SkinFormProfessionalRenderer : SkinFormRenderer
    {
        private SkinFormColorTable _colorTable;

        public SkinFormProfessionalRenderer()
            : base() {
        }

        public SkinFormProfessionalRenderer(SkinFormColorTable colortable)
            : base() {
            _colorTable = colortable;
        }

        public SkinFormColorTable ColorTable {
            get {
                if (_colorTable == null) {
                    _colorTable = new SkinFormColorTable();
                }
                return _colorTable;
            }
        }

        //画圆角
        public override Region CreateRegion(CSSkinMain form) {
            Rectangle rect = new Rectangle(Point.Empty, form.Size);

            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect,
                form.Radius,
                form.RoundStyle,
                false)) {
                return new Region(path);
            }
        }

        public override void InitSkinForm(CSSkinMain form) {
            form.BackColor = ColorTable.Back;
        }

        //画标题和ICO
        protected override void OnRenderSkinFormCaption(
            SkinFormCaptionRenderEventArgs e) {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            CSSkinMain form = e.SkinForm;
            Rectangle iconRect = form.IconRect;
            Rectangle textRect = Rectangle.Empty;

            bool closeBox = form.ControlBox;
            bool minimizeBox = form.ControlBox && form.MinimizeBox;
            bool maximizeBox = form.ControlBox && form.MaximizeBox;

            int textWidthDec = 0;
            if (closeBox) {
                textWidthDec += form.CloseBoxSize.Width + form.ControlBoxOffset.X;
            }

            if (maximizeBox) {
                textWidthDec += form.MaxSize.Width + form.ControlBoxSpace;
            }

            if (minimizeBox) {
                textWidthDec += form.MiniSize.Width + form.ControlBoxSpace;
            }

            foreach (CmSysButton item in form.ControlBoxManager.SysButtonItems) {
                if (form.ControlBox && item.Visibale) {
                    textWidthDec += item.Size.Width + form.ControlBoxSpace;
                }
            }

            textRect = new Rectangle(
                iconRect.Right + 3,
                form.BorderPadding.Top,
                form.Width - (iconRect.Right + 3) - form.AllButtonWidth(true) - form.ControlBoxOffset.X,
                rect.Height - form.BorderPadding.Left);

            using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                DrawCaptionBackground(
                    g,
                    rect,
                    e.Active,
                    form);

                if (form.ShowDrawIcon && form.Icon != null) {
                    DrawIcon(g, iconRect, form.Icon);
                }

                if (!string.IsNullOrEmpty(form.Text)) {
                    Color EfColor = form.EffectBack;
                    Color TitleColor = form.TitleColor;
                    //判断是否根据背景色适应颜色
                    if (form.TitleSuitColor) {
                        //如果背景色为暗色
                        if (SkinTools.ColorSlantsDarkOrBright(form.BackColor)) {
                            form.ForeColor = TitleColor = Color.White;
                            EfColor = Color.Black;
                        } else//如果背景色为亮色
                        {
                            form.ForeColor = TitleColor = Color.Black;
                            EfColor = Color.White;
                        }
                    }
                    DrawCaptionText(
                        g,
                        textRect,
                        form.Text,
                        form.CaptionFont,
                        form.EffectCaption,
                        EfColor,
                        form.EffectWidth,
                        TitleColor,
                        form.TitleOffset,
                        form.TitleCenter,
                        form.Size);
                }
            }
        }

        //画边框
        protected override void OnRenderSkinFormBorder(
            SkinFormBorderRenderEventArgs e) {
            Graphics g = e.Graphics;
            using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                DrawBorder(
                    g,
                    e.ClipRectangle,
                    e.SkinForm.RoundStyle,
                    e.SkinForm.Radius,
                    e.SkinForm);
            }
        }

        //画窗体按钮
        protected override void OnRenderSkinFormControlBox(
            SkinFormControlBoxRenderEventArgs e) {
            CSSkinMain form = e.Form;
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            ControlBoxState state = e.ControlBoxtate;
            CmSysButton cmSysButton = e.CmSysButton;
            bool active = e.Active;

            bool minimizeBox = form.ControlBox && form.MinimizeBox;
            bool maximizeBox = form.ControlBox && form.MaximizeBox;

            switch (e.ControlBoxStyle) {
                case ControlBoxStyle.Close:
                    RenderSkinFormCloseBoxInternal(
                        g,
                        rect,
                        state,
                        active,
                        minimizeBox,
                        maximizeBox,
                        form);
                    break;
                case ControlBoxStyle.Maximize:
                    RenderSkinFormMaximizeBoxInternal(
                        g,
                        rect,
                        state,
                        active,
                        minimizeBox,
                        form.WindowState == FormWindowState.Maximized,
                        form);
                    break;
                case ControlBoxStyle.Minimize:
                    RenderSkinFormMinimizeBoxInternal(
                       g,
                       rect,
                       state,
                       active,
                       form);
                    break;
                case ControlBoxStyle.CmSysBottom:
                    RenderSkinFormCmSysBottomInternal(
                       g,
                       rect,
                       state,
                       active,
                       form,
                       cmSysButton);
                    break;
            }
        }

        #region Draw Methods

        private void DrawCaptionBackground(
            Graphics g, Rectangle captionRect, bool active, CSSkinMain main) {
            Color baseColor = active ?
                ColorTable.CaptionActive : ColorTable.CaptionDeactive;

            RenderHelper.RenderBackgroundInternal(
                g,
                captionRect,
                baseColor,
                main.BorderColor,
                main.InnerBorderColor,
                RoundStyle.None,
                0,
                .25f,
                false,
                false,
                LinearGradientMode.Vertical);
        }

        //画Icon
        private void DrawIcon(
            Graphics g, Rectangle iconRect, Icon icon) {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawIcon(
                icon,
                iconRect);
        }

        //画标题
        private void DrawCaptionText(
            Graphics g, Rectangle textRect, string text, Font font, TitleType Effect, Color EffetBack, int EffectWidth, Color FrmColor, Point TitleOffset, bool TitleCenter, Size fSize) {
            if (Effect == TitleType.EffectTitle) {
                Size txtsize = TextRenderer.MeasureText(text, font);
                Image imgtext = SkinTools.ImageLightEffect(text, font, FrmColor, EffetBack, EffectWidth, new Rectangle(0, 0, textRect.Width, txtsize.Height), true);
                int center = TitleCenter ? (textRect.Width - imgtext.Width) / 2 : 0;
                center = center < 0 ? 0 : center;
                g.DrawImage(imgtext, textRect.X - EffectWidth / 2 + TitleOffset.X + center, textRect.Y - EffectWidth / 2 + TitleOffset.Y);
            } else if (Effect == TitleType.Title) {
                textRect.X += TitleOffset.X;
                textRect.Y += TitleOffset.Y;
                TextRenderer.DrawText(
                    g,
                    text,
                    font,
                    textRect,
                    FrmColor,
                    TextFormatFlags.VerticalCenter |
                    (TitleCenter ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Left) |
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.WordEllipsis);
            }
        }

        //画边框
        private void DrawBorder(
            Graphics g, Rectangle rect, RoundStyle roundStyle, int radius, CSSkinMain frm) {
            rect.Width--;
            rect.Height--;
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, radius, roundStyle, false)) {
                using (Pen pen = new Pen(frm.BorderColor)) {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, radius, roundStyle, false)) {
                using (Pen pen = new Pen(frm.InnerBorderColor)) {
                    g.DrawPath(pen, path);
                }
            }
        }

        //颜色色调调整设置
        public ImageAttributes Trank(Bitmap btm, int Alphas) {
            Bitmap box = (Bitmap)btm.Clone();
            Graphics g = Graphics.FromImage(box);
            //初始化颜色矩阵
            float Alpha = Alphas / 100f;
            float[][] matrixltems ={   new float[]{1,0,0,0,0},
									  new float[]{0,1,0,0,0},
									  new float[]{0,0,1,0,0},
									  new float[]{0,0,0,Alpha,0},
									  new float[]{0,0,0,0,1}};//在5×5矩阵中，0行0列为红色、1行1列为绿色、2行2列为蓝色、3行3列为alpha颜色的透明度、4行4列为线性变换和平移
            ColorMatrix colorMatrix = new ColorMatrix(matrixltems);
            ImageAttributes ImageAtt = new ImageAttributes();
            ImageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return ImageAtt;
        }

        //画最小化按钮
        private void RenderSkinFormMinimizeBoxInternal(
           Graphics g,
           Rectangle rect,
           ControlBoxState state,
           bool active,
           CSSkinMain form) {
            Bitmap btm = null;
            Color baseColor = ColorTable.ControlBoxActive;

            if (state == ControlBoxState.Pressed) {
                btm = (Bitmap)form.MiniDownBack;
                baseColor = ColorTable.ControlBoxPressed;
            } else if (state == ControlBoxState.Hover) {
                btm = (Bitmap)form.MiniMouseBack;
                baseColor = ColorTable.ControlBoxHover;
            } else {
                btm = (Bitmap)form.MiniNormlBack;
                baseColor = active ?
                    ColorTable.ControlBoxActive :
                    ColorTable.ControlBoxDeactive;
            }
            //绘制图片样式
            if (btm != null) {
                g.DrawImage(btm, rect);
            } else //绘制默认样式
            {
                RoundStyle roundStyle = RoundStyle.BottomLeft;
                //g.DrawImage(btm,rect);
                using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        baseColor,
                        baseColor,
                        ColorTable.ControlBoxInnerBorder,
                        roundStyle,
                        6,
                        .38F,
                        true,
                        false,
                        LinearGradientMode.Vertical);

                    //using (Pen pen = new Pen(form.BorderColor)) {
                    //    g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    //}

                    using (GraphicsPath path = CreateMinimizeFlagPath(rect)) {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen = new Pen(baseColor)) {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        //画自定义系统按钮集合
        private void RenderSkinFormCmSysBottomInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active, CSSkinMain form, CmSysButton cmSysButton) {
            Bitmap btm = null;
            Color baseColor = ColorTable.ControlBoxActive;

            if (cmSysButton.BoxState == ControlBoxState.Pressed) {
                btm = (Bitmap)cmSysButton.SysButtonDown;
                baseColor = ColorTable.ControlBoxPressed;
            } else if (state == ControlBoxState.Hover) {
                btm = (Bitmap)cmSysButton.SysButtonMouse;
                baseColor = ColorTable.ControlBoxHover;
            } else {
                btm = (Bitmap)cmSysButton.SysButtonNorml;
                baseColor = active ?
                    ColorTable.ControlBoxActive :
                    ColorTable.ControlBoxDeactive;
            }

            //绘制图片样式
            if (btm != null) {
                g.DrawImage(btm, rect);
            } else //绘制默认样式
            {
                RoundStyle roundStyle = RoundStyle.BottomLeft;
                using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        baseColor,
                        baseColor,
                        ColorTable.ControlBoxInnerBorder,
                        roundStyle,
                        6,
                        .38F,
                        true,
                        false,
                        LinearGradientMode.Vertical);

                    using (Pen pen = new Pen(form.BorderColor)) {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                }
            }
        }

        //画最大化按钮
        private void RenderSkinFormMaximizeBoxInternal(
            Graphics g,
            Rectangle rect,
            ControlBoxState state,
            bool active,
            bool minimizeBox,
            bool maximize,
            CSSkinMain form) {
            Bitmap btm = null;
            Color baseColor = ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed) {
                btm = maximize ? (Bitmap)form.RestoreDownBack : (Bitmap)form.MaxDownBack;
                baseColor = ColorTable.ControlBoxPressed;
            } else if (state == ControlBoxState.Hover) {
                btm = maximize ? (Bitmap)form.RestoreMouseBack : (Bitmap)form.MaxMouseBack;
                baseColor = ColorTable.ControlBoxHover;
            } else {
                btm = maximize ? (Bitmap)form.RestoreNormlBack : (Bitmap)form.MaxNormlBack;
                baseColor = active ?
                    ColorTable.ControlBoxActive :
                    ColorTable.ControlBoxDeactive;
            }

            //绘制图片样式
            if (btm != null) {
                g.DrawImage(btm, rect);
            } else //绘制默认样式
            {
                RoundStyle roundStyle = minimizeBox ?
                    RoundStyle.None : RoundStyle.BottomLeft;

                using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        baseColor,
                        baseColor,
                        ColorTable.ControlBoxInnerBorder,
                        roundStyle,
                        6,
                        .38F,
                        true,
                        false,
                        LinearGradientMode.Vertical);

                    //using (Pen pen = new Pen(form.BorderColor)) {
                    //    g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    //}

                    using (GraphicsPath path = CreateMaximizeFlafPath(rect, maximize)) {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen = new Pen(baseColor)) {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        //画关闭按钮
        private void RenderSkinFormCloseBoxInternal(
            Graphics g,
            Rectangle rect,
            ControlBoxState state,
            bool active,
            bool minimizeBox,
            bool maximizeBox,
            CSSkinMain form) {
            Bitmap btm = null;
            Color baseColor = ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed) {
                btm = (Bitmap)form.CloseDownBack;
                baseColor = ColorTable.ControlCloseBoxPressed;
            } else if (state == ControlBoxState.Hover) {
                btm = (Bitmap)form.CloseMouseBack;
                baseColor = ColorTable.ControlCloseBoxHover;
            } else {
                btm = (Bitmap)form.CloseNormlBack;
                baseColor = active ?
                    ColorTable.ControlBoxActive :
                    ColorTable.ControlBoxDeactive;
            }
            //绘制图片样式
            if (btm != null) {
                g.DrawImage(btm, rect);
            } else //绘制默认样式
            {
                RoundStyle roundStyle = minimizeBox || maximizeBox ?
                    RoundStyle.BottomRight : RoundStyle.Bottom;

                using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        baseColor,
                        baseColor,
                        ColorTable.ControlBoxInnerBorder,
                        roundStyle,
                        6,
                        .38F,
                        true,
                        false,
                        LinearGradientMode.Vertical);

                    //using (Pen pen = new Pen(form.BorderColor)) {
                    //    g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    //}

                    using (GraphicsPath path = CreateCloseFlagPath(rect)) {
                        g.FillPath(Brushes.White, path);
                        using (Pen pen = new Pen(baseColor)) {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        #endregion

        private GraphicsPath CreateCloseFlagPath(Rectangle rect) {
            PointF centerPoint = new PointF(
                rect.X + rect.Width / 2.0f,
                rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            path.AddLine(
                centerPoint.X,
                centerPoint.Y - 2,
                centerPoint.X - 2,
                centerPoint.Y - 4);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y - 4,
                centerPoint.X - 6,
                centerPoint.Y - 4);
            path.AddLine(
                centerPoint.X - 6,
                centerPoint.Y - 4,
                centerPoint.X - 2,
                centerPoint.Y);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y,
                centerPoint.X - 6,
                centerPoint.Y + 4);
            path.AddLine(
                centerPoint.X - 6,
                centerPoint.Y + 4,
                centerPoint.X - 2,
                centerPoint.Y + 4);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y + 4,
                centerPoint.X,
                centerPoint.Y + 2);
            path.AddLine(
                centerPoint.X,
                centerPoint.Y + 2,
                centerPoint.X + 2,
                centerPoint.Y + 4);
            path.AddLine(
               centerPoint.X + 2,
               centerPoint.Y + 4,
               centerPoint.X + 6,
               centerPoint.Y + 4);
            path.AddLine(
              centerPoint.X + 6,
              centerPoint.Y + 4,
              centerPoint.X + 2,
              centerPoint.Y);
            path.AddLine(
             centerPoint.X + 2,
             centerPoint.Y,
             centerPoint.X + 6,
             centerPoint.Y - 4);
            path.AddLine(
             centerPoint.X + 6,
             centerPoint.Y - 4,
             centerPoint.X + 2,
             centerPoint.Y - 4);

            path.CloseFigure();
            return path;
        }

        private GraphicsPath CreateMinimizeFlagPath(Rectangle rect) {
            PointF centerPoint = new PointF(
                rect.X + rect.Width / 2.0f,
                rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(new RectangleF(
                centerPoint.X - 6,
                centerPoint.Y + 1,
                12,
                3));
            return path;
        }

        private GraphicsPath CreateMaximizeFlafPath(
            Rectangle rect, bool maximize) {
            PointF centerPoint = new PointF(
               rect.X + rect.Width / 2.0f,
               rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            if (maximize) {
                path.AddLine(
                    centerPoint.X - 3,
                    centerPoint.Y - 3,
                    centerPoint.X - 6,
                    centerPoint.Y - 3);
                path.AddLine(
                    centerPoint.X - 6,
                    centerPoint.Y - 3,
                    centerPoint.X - 6,
                    centerPoint.Y + 5);
                path.AddLine(
                    centerPoint.X - 6,
                    centerPoint.Y + 5,
                    centerPoint.X + 3,
                    centerPoint.Y + 5);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y + 5,
                    centerPoint.X + 3,
                    centerPoint.Y + 1);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y + 1,
                    centerPoint.X + 6,
                    centerPoint.Y + 1);
                path.AddLine(
                    centerPoint.X + 6,
                    centerPoint.Y + 1,
                    centerPoint.X + 6,
                    centerPoint.Y - 6);
                path.AddLine(
                    centerPoint.X + 6,
                    centerPoint.Y - 6,
                    centerPoint.X - 3,
                    centerPoint.Y - 6);
                path.CloseFigure();

                path.AddRectangle(new RectangleF(
                    centerPoint.X - 4,
                    centerPoint.Y,
                    5,
                    3));

                path.AddLine(
                    centerPoint.X - 1,
                    centerPoint.Y - 4,
                    centerPoint.X + 4,
                    centerPoint.Y - 4);
                path.AddLine(
                    centerPoint.X + 4,
                    centerPoint.Y - 4,
                    centerPoint.X + 4,
                    centerPoint.Y - 1);
                path.AddLine(
                    centerPoint.X + 4,
                    centerPoint.Y - 1,
                    centerPoint.X + 3,
                    centerPoint.Y - 1);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y - 1,
                    centerPoint.X + 3,
                    centerPoint.Y - 3);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y - 3,
                    centerPoint.X - 1,
                    centerPoint.Y - 3);
                path.CloseFigure();
            } else {
                path.AddRectangle(new RectangleF(
                    centerPoint.X - 6,
                    centerPoint.Y - 4,
                    12,
                    8));
                path.AddRectangle(new RectangleF(
                    centerPoint.X - 3,
                    centerPoint.Y - 1,
                    6,
                    3));
            }

            return path;
        }
    }
}
