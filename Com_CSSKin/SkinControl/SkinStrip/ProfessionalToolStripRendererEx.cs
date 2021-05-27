
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    public class ProfessionalToolStripRendererEx
        : ToolStripRenderer
    {
        private ToolStripColorTable _colorTable;

        public ProfessionalToolStripRendererEx()
            : base() {
            ColorTable = new ToolStripColorTable();
        }

        public ProfessionalToolStripRendererEx(
            ToolStripColorTable colorTable)
            : base() {
            ColorTable = colorTable;
        }

        public ToolStripColorTable ColorTable {
            get {
                return _colorTable;
            }
            set {
                _colorTable = value;
            }
        }

        protected override void OnRenderToolStripBackground(
            ToolStripRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown) {
                RegionHelper.CreateRegion(toolStrip, bounds, ColorTable.BackRadius, ColorTable.RadiusStyle);
                using (SolidBrush brush = new SolidBrush(ColorTable.Back)) {
                    g.FillRectangle(brush, bounds);
                }
            } else {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                RenderHelperStrip.RenderBackgroundInternal(
                    g,
                    bounds,
                    ColorTable.Base,
                    ColorTable.ItemBorder,
                    ColorTable.Back,
                    ColorTable.RadiusStyle,
                    ColorTable.BackRadius,
                    .35f,
                    false,
                    false,
                    mode);
            }
        }

        protected override void OnRenderImageMargin(
            ToolStripRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown) {
                bool bRightToLeft = toolStrip.RightToLeft == RightToLeft.Yes;

                Rectangle imageBackRect = bounds;
                Rectangle logoRect = bounds;
                if (bRightToLeft) {
                    logoRect.X -= 2;
                    imageBackRect.X = logoRect.X;
                } else {
                    logoRect.X += 2;
                    imageBackRect.X = logoRect.Right;
                }
                logoRect.Y += 1;
                logoRect.Height -= 2;

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    logoRect,
                    ColorTable.TitleColor,
                    ColorTable.Back,
                    90f)) {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0f, .2f, 1f };
                    blend.Factors = new float[] { 0f, 0.1f, .9f };
                    brush.Blend = blend;
                    logoRect.Y += 1;
                    logoRect.Height -= 2;
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(logoRect, ColorTable.TitleRadius, ColorTable.TitleRadiusStyle, false)) {
                        using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                            if (ColorTable.TitleAnamorphosis) {
                                g.FillPath(brush, path);
                            } else {
                                SolidBrush br = new SolidBrush(ColorTable.TitleColor);
                                g.FillPath(br, path);
                            }
                        }
                    }
                }
            } else {
                base.OnRenderImageMargin(e);
            }
        }

        protected override void OnRenderToolStripBorder(
            ToolStripRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown) {
                if (ColorTable.RadiusStyle == RoundStyle.None) {
                    bounds.Width--;
                    bounds.Height--;
                }
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(bounds, ColorTable.BackRadius, ColorTable.RadiusStyle, true)) {
                        using (Pen pen = new Pen(ColorTable.DropDownImageSeparator)) {
                            path.Widen(pen);
                            g.DrawPath(pen, path);
                        }
                    }
                }

                if (!(toolStrip is ToolStripOverflow)) {
                    bounds.Inflate(-1, -1);
                    using (GraphicsPath innerPath = GraphicsPathHelper.CreatePath(
                        bounds, ColorTable.BackRadius, ColorTable.RadiusStyle, true)) {
                        using (Pen pen = new Pen(ColorTable.Back)) {
                            g.DrawPath(pen, innerPath);
                        }
                    }
                }
            } else {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected override void OnRenderMenuItemBackground(
           ToolStripItemRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled) {
                return;
            }

            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
            if (toolStrip is MenuStrip) {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                if (item.Selected) {
                    RenderHelperStrip.RenderBackgroundInternal(
                        g,
                        rect,
                        ColorTable.ItemHover,
                        ColorTable.ItemBorder,
                        ColorTable.Back,
                        ColorTable.BaseItemRadiusStyle,
                        ColorTable.BaseItemRadius,
                        true,
                        true,
                        mode);
                } else if (item.Pressed) {
                    RenderHelperStrip.RenderBackgroundInternal(
                       g,
                       rect,
                       ColorTable.ItemPressed,
                       ColorTable.ItemBorder,
                       ColorTable.Back,
                       ColorTable.BaseItemRadiusStyle,
                       ColorTable.BaseItemRadius,
                       true,
                       true,
                       mode);
                } else {
                    base.OnRenderMenuItemBackground(e);
                }
            } else if (toolStrip is ToolStripDropDown) {
                rect = new Rectangle(new Point(-2, 0), new Size(e.Item.Size.Width + 5, e.Item.Size.Height + 1));
                if (item.RightToLeft == RightToLeft.Yes) {
                    rect.X += 4;
                } else {
                    rect.X += 4;
                }
                rect.Width -= 8;
                rect.Height--;

                if (item.Selected) {
                    //Item
                    RenderHelperStrip.RenderBackgroundInternal(
                      g,
                      rect,
                      ColorTable.ItemHover,
                      ColorTable.ItemBorder,
                      ColorTable.Back,
                      ColorTable.ItemRadiusStyle,
                      ColorTable.ItemRadius,
                      ColorTable.ItemBorderShow,
                      ColorTable.ItemAnamorphosis,
                      LinearGradientMode.Vertical);
                } else {
                    base.OnRenderMenuItemBackground(e);
                }
            } else {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemImage(
            ToolStripItemImageRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (toolStrip is ToolStripDropDown &&
               e.Item is ToolStripMenuItem) {
                ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
                if (item.Checked) {
                    return;
                }
                Rectangle rect = e.ImageRectangle;
                if (e.Item.RightToLeft == RightToLeft.Yes) {
                    rect.X -= 2;
                } else {
                    rect.X += 2;
                }

                using (InterpolationModeGraphics ig =
                    new InterpolationModeGraphics(g)) {
                    ToolStripItemImageRenderEventArgs ne =
                        new ToolStripItemImageRenderEventArgs(
                        g, e.Item, e.Image, rect);
                    base.OnRenderItemImage(ne);
                }
            } else {
                base.OnRenderItemImage(e);
            }
        }

        protected override void OnRenderItemText(
            ToolStripItemTextRenderEventArgs e) {
            //
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;
            e.TextRectangle = new Rectangle(new Point(e.TextRectangle.X + ColorTable.BaseForeOffset.X, e.TextRectangle.Y + ColorTable.BaseForeOffset.Y), e.TextRectangle.Size);
            //ͳһɫ任ģʽ
            if (ColorTable.SkinAllColor) {
                if (toolStrip is ToolStripDropDown) {
                    e.TextColor = item.Selected ? ColorTable.HoverFore : ColorTable.Fore;
                } else {
                    e.TextColor = item.Selected ? ColorTable.BaseHoverFore : ColorTable.BaseFore;
                }
            }

            if (toolStrip is ToolStripDropDown &&
                e.Item is ToolStripMenuItem) {
                Rectangle rect = e.TextRectangle;
                e.TextRectangle = rect;
            }
            if (!(toolStrip is ToolStripDropDown) && ColorTable.BaseForeAnamorphosis && !string.IsNullOrEmpty(e.Item.Text)) {
                Graphics g = e.Graphics;
                Image img = SkinTools.ImageLightEffect(e.Item.Text, e.Item.Font, e.TextColor, ColorTable.BaseForeAnamorphosisColor, ColorTable.BaseForeAnamorphosisBorder);
                g.DrawImage(img, e.TextRectangle.Left - ColorTable.BaseForeAnamorphosisBorder / 2, e.TextRectangle.Top - ColorTable.BaseForeAnamorphosisBorder / 2);
                return;
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(
            ToolStripItemImageRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (toolStrip is ToolStripDropDown &&
               e.Item is ToolStripMenuItem) {
                Rectangle rect = e.ImageRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes) {
                    rect.X -= 2;
                } else {
                    rect.X += 2;
                }

                rect.Width = 13;
                rect.Y += 1;
                rect.Height -= 3;

                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                    using (GraphicsPath path = new GraphicsPath()) {
                        path.AddRectangle(rect);
                        using (PathGradientBrush brush = new PathGradientBrush(path)) {
                            brush.CenterColor = Color.White;
                            brush.SurroundColors = new Color[] { ControlPaint.Light(ColorTable.Back) };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.3f, 1f };
                            blend.Factors = new float[] { 0f, 0.5f, 1f };
                            brush.Blend = blend;
                            g.FillRectangle(brush, rect);
                        }
                    }

                    using (Pen pen = new Pen(ControlPaint.Light(ColorTable.Back))) {
                        g.DrawRectangle(pen, rect);
                    }

                    ControlPaintEx.DrawCheckedFlag(g, rect, ColorTable.Fore);
                }
            } else {
                base.OnRenderItemCheck(e);
            }
        }

        protected override void OnRenderArrow(
            ToolStripArrowRenderEventArgs e) {
            if (e.Item.Enabled) {
                e.ArrowColor = ColorTable.Arrow;
            }

            ToolStrip toolStrip = e.Item.Owner;

            if (toolStrip is ToolStripDropDown &&
                e.Item is ToolStripMenuItem) {
                Rectangle rect = e.ArrowRectangle;
                e.ArrowRectangle = rect;
            }

            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(
            ToolStripSeparatorRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            Rectangle rect = e.Item.ContentRectangle;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (toolStrip is ToolStripDropDown) {
                if (e.Item.RightToLeft != RightToLeft.Yes) {
                    rect.X += 26;
                }
                rect.Width -= 28;
            }

            RenderSeparatorLine(
               g,
               rect,
               ColorTable.BaseItemSplitter,
               ColorTable.Back,
               SystemColors.ControlLightLight,
               e.Vertical);
        }

        protected override void OnRenderButtonBackground(
            ToolStripItemRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripButton item = e.Item as ToolStripButton;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (item != null) {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.BackgroundImage != null) {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    ControlPaintEx.DrawBackgroundImage(
                        g,
                        item.BackgroundImage,
                        ColorTable.Back,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.CheckState == CheckState.Unchecked) {
                    if (item.Selected) {
                        Bitmap btm = item.Pressed ? (Bitmap)ColorTable.BaseItemDown : (Bitmap)ColorTable.BaseItemMouse;
                        if (btm != null) {
                            ImageDrawRect.DrawRect(g, btm, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                        } else {
                            Color color = ColorTable.BaseItemHover;
                            if (item.Pressed) {
                                color = ColorTable.BaseItemPressed;
                            }
                            RenderHelperStrip.RenderBackgroundInternal(
                                g,
                                bounds,
                                color,
                                ColorTable.BaseItemBorder,
                                ColorTable.Back,
                                ColorTable.BaseItemRadiusStyle,
                                ColorTable.BaseItemRadius,
                                ColorTable.BaseItemBorderShow,
                                ColorTable.BaseItemAnamorphosis,
                                mode);
                        }
                    } else {
                        //画默认背景
                        if (ColorTable.BaseItemNorml != null) {
                            ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemNorml, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                        }
                        if (toolStrip is ToolStripOverflow) {
                            using (Brush brush = new SolidBrush(ColorTable.ItemHover)) {
                                g.FillRectangle(brush, bounds);
                            }
                        }
                    }
                } else {

                    Bitmap btm = (Bitmap)ColorTable.BaseItemMouse;
                    Color color = ControlPaint.Light(ColorTable.ItemHover);
                    if (item.Selected) {
                        color = ColorTable.ItemHover;
                        btm = (Bitmap)ColorTable.BaseItemMouse;
                    }
                    if (item.Pressed) {
                        color = ColorTable.ItemPressed;
                        btm = (Bitmap)ColorTable.BaseItemDown;
                    }

                    if (btm == null) {
                        RenderHelperStrip.RenderBackgroundInternal(
                            g,
                            bounds,
                            color,
                            ColorTable.BaseItemBorder,
                            ColorTable.Back,
                            ColorTable.BaseItemRadiusStyle,
                            ColorTable.BaseItemRadius,
                            ColorTable.BaseItemBorderShow,
                            ColorTable.BaseItemAnamorphosis,
                            mode);
                    } else {
                        ImageDrawRect.DrawRect(g, btm, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    }
                }
                sg.Dispose();
            }
        }

        protected override void OnRenderDropDownButtonBackground(
            ToolStripItemRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripDropDownItem item = e.Item as ToolStripDropDownItem;

            if (item != null) {
                LinearGradientMode mode =
                   toolStrip.Orientation == Orientation.Horizontal ?
                   LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Graphics g = e.Graphics;
                //
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);
                if (item.Pressed && item.HasDropDownItems) {
                    if (ColorTable.BaseItemDown != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemDown, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    } else {
                        RenderHelperStrip.RenderBackgroundInternal(
                          g,
                          bounds,
                          ColorTable.BaseItemPressed,
                          ColorTable.BaseItemBorder,
                          ColorTable.Back,
                          ColorTable.BaseItemRadiusStyle,
                          ColorTable.BaseItemRadius,
                          ColorTable.BaseItemBorderShow,
                          ColorTable.BaseItemAnamorphosis,
                          mode);
                    }
                } else if (item.Selected) {
                    if (ColorTable.BaseItemDown != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemMouse, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    } else {
                        RenderHelperStrip.RenderBackgroundInternal(
                          g,
                          bounds,
                          ColorTable.BaseItemHover,
                          ColorTable.BaseItemBorder,
                          ColorTable.Back,
                          ColorTable.BaseItemRadiusStyle,
                          ColorTable.BaseItemRadius,
                          ColorTable.BaseItemBorderShow,
                          ColorTable.BaseItemAnamorphosis,
                          mode);
                    }
                } else if (toolStrip is ToolStripOverflow) {
                    using (Brush brush = new SolidBrush(ColorTable.Back)) {
                        g.FillRectangle(brush, bounds);
                    }
                } else {
                    //画默认背景
                    if (ColorTable.BaseItemNorml != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemNorml, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    }
                    base.OnRenderDropDownButtonBackground(e);
                }

                sg.Dispose();
            }
        }

        protected override void OnRenderSplitButtonBackground(
            ToolStripItemRenderEventArgs e) {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;

            if (item != null) {
                Graphics g = e.Graphics;
                //
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

                Color arrowColor = toolStrip.Enabled ?
                    ColorTable.Fore : SystemColors.ControlDark;

                if (item.BackgroundImage != null) {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    ControlPaintEx.DrawBackgroundImage(
                        g,
                        item.BackgroundImage,
                        ColorTable.Back,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.ButtonPressed) {
                    if (ColorTable.BaseItemDown != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemDown, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    } else {
                        Rectangle buttonBounds = item.ButtonBounds;
                        Padding padding = (item.RightToLeft == RightToLeft.Yes) ?
                            new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
                        buttonBounds = LayoutUtils.DeflateRect(buttonBounds, padding);
                        RenderHelperStrip.RenderBackgroundInternal(
                           g,
                           bounds,
                           ColorTable.BaseItemHover,
                           ColorTable.BaseItemBorder,
                           ColorTable.Back,
                           ColorTable.BaseItemRadiusStyle,
                           ColorTable.BaseItemRadius,
                           ColorTable.BaseItemBorderShow,
                           ColorTable.BaseItemAnamorphosis,
                           mode);

                        buttonBounds.Inflate(-1, -1);
                        g.SetClip(buttonBounds);
                        RenderHelperStrip.RenderBackgroundInternal(
                           g,
                           buttonBounds,
                           ColorTable.BaseItemPressed,
                           ColorTable.BaseItemBorder,
                           ColorTable.Back,
                           RoundStyle.Left,
                           false,
                           true,
                           mode);
                        g.ResetClip();

                        using (Pen pen = new Pen(ColorTable.BaseItemSplitter)) {
                            g.DrawLine(
                                pen,
                                item.SplitterBounds.Left,
                                item.SplitterBounds.Top,
                                item.SplitterBounds.Left,
                                item.SplitterBounds.Bottom);
                        }
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                }

                if (item.Pressed || item.DropDownButtonPressed) {
                    if (ColorTable.BaseItemDown != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemDown, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    } else {
                        RenderHelperStrip.RenderBackgroundInternal(
                          g,
                          bounds,
                          ColorTable.BaseItemPressed,
                          ColorTable.BaseItemBorder,
                          ColorTable.Back,
                          ColorTable.BaseItemRadiusStyle,
                          ColorTable.BaseItemRadius,
                          ColorTable.BaseItemBorderShow,
                          ColorTable.BaseItemAnamorphosis,
                          mode);
                    }
                    base.DrawArrow(
                       new ToolStripArrowRenderEventArgs(
                       g,
                       item,
                       item.DropDownButtonBounds,
                       arrowColor,
                       ArrowDirection.Down));
                    return;
                }

                if (item.Selected) {
                    if (ColorTable.BaseItemMouse != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemMouse, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    } else {
                        RenderHelperStrip.RenderBackgroundInternal(
                          g,
                          bounds,
                          ColorTable.BaseItemHover,
                          ColorTable.BaseItemBorder,
                          ColorTable.Back,
                          ColorTable.BaseItemRadiusStyle,
                          ColorTable.BaseItemRadius,
                          ColorTable.BaseItemBorderShow,
                          ColorTable.BaseItemAnamorphosis,
                          mode);
                        using (Pen pen = new Pen(ColorTable.BaseItemSplitter)) {
                            g.DrawLine(
                               pen,
                               item.SplitterBounds.Left,
                               item.SplitterBounds.Top,
                               item.SplitterBounds.Left,
                               item.SplitterBounds.Bottom);
                        }
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                } else {
                    //画默认背景
                    if (ColorTable.BaseItemNorml != null) {
                        ImageDrawRect.DrawRect(g, (Bitmap)ColorTable.BaseItemNorml, bounds, Rectangle.FromLTRB(ColorTable.BackRectangle.X, ColorTable.BackRectangle.Y, ColorTable.BackRectangle.Width, ColorTable.BackRectangle.Height), 1, 1);
                    }
                }

                base.DrawArrow(
                   new ToolStripArrowRenderEventArgs(
                   g,
                   item,
                   item.DropDownButtonBounds,
                   arrowColor,
                   ArrowDirection.Down));
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

        protected override void OnRenderOverflowButtonBackground(
            ToolStripItemRenderEventArgs e) {
            ToolStripItem item = e.Item;
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            bool rightToLeft = item.RightToLeft == RightToLeft.Yes;

            SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

            RenderOverflowBackground(e, rightToLeft);

            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;
            Rectangle empty = Rectangle.Empty;

            if (rightToLeft) {
                empty = new Rectangle(0, item.Height - 8, 10, 5);
            } else {
                empty = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
            }
            ArrowDirection direction = bHorizontal ?
                ArrowDirection.Down : ArrowDirection.Right;
            int x = (rightToLeft && bHorizontal) ? -1 : 1;
            empty.Offset(x, 1);

            Color arrowColor = toolStrip.Enabled ?
                ColorTable.Fore : SystemColors.ControlDark;

            using (Brush brush = new SolidBrush(arrowColor)) {
                RenderHelperStrip.RenderArrowInternal(g, empty, direction, brush);
            }

            if (bHorizontal) {
                using (Pen pen = new Pen(arrowColor)) {
                    g.DrawLine(
                        pen,
                        empty.Right - 8,
                        empty.Y - 2,
                        empty.Right - 2,
                        empty.Y - 2);
                    g.DrawLine(
                        pen,
                        empty.Right - 8,
                        empty.Y - 1,
                        empty.Right - 2,
                        empty.Y - 1);
                }
            } else {
                using (Pen pen = new Pen(arrowColor)) {
                    g.DrawLine(
                        pen,
                        empty.X,
                        empty.Y,
                        empty.X,
                        empty.Bottom - 1);
                    g.DrawLine(
                        pen,
                        empty.X,
                        empty.Y + 1,
                        empty.X,
                        empty.Bottom);
                }
            }
        }

        protected override void OnRenderGrip(
            ToolStripGripRenderEventArgs e) {
            if (e.GripStyle == ToolStripGripStyle.Visible) {
                Rectangle bounds = e.GripBounds;
                bool vert = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
                ToolStrip toolStrip = e.ToolStrip;
                Graphics g = e.Graphics;
                //
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                if (vert) {
                    bounds.X = e.AffectedBounds.X;
                    bounds.Width = e.AffectedBounds.Width;
                    if (toolStrip is MenuStrip) {
                        if (e.AffectedBounds.Height > e.AffectedBounds.Width) {
                            vert = false;
                            bounds.Y = e.AffectedBounds.Y;
                        } else {
                            toolStrip.GripMargin = new Padding(0, 2, 0, 2);
                            bounds.Y = e.AffectedBounds.Y;
                            bounds.Height = e.AffectedBounds.Height;
                        }
                    } else {
                        toolStrip.GripMargin = new Padding(2, 2, 4, 2);
                        bounds.X++;
                        bounds.Width++;
                    }
                } else {
                    bounds.Y = e.AffectedBounds.Y;
                    bounds.Height = e.AffectedBounds.Height;
                }

                DrawDottedGrip(
                    g,
                    bounds,
                    vert,
                    false,
                    ColorTable.Back,
                    ControlPaint.Dark(ColorTable.Base, 0.3F));
            }
        }

        protected override void OnRenderStatusStripSizingGrip(
            ToolStripRenderEventArgs e) {
            //
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            DrawSolidStatusGrip(
                e.Graphics,
                e.AffectedBounds,
                ColorTable.Back,
                ControlPaint.Dark(ColorTable.Base, 0.3f));
        }

        public void RenderSeparatorLine(
            Graphics g,
            Rectangle rect,
            Color baseColor,
            Color backColor,
            Color shadowColor,
            bool vertical) {
            if (vertical) {
                rect.Y += 2;
                rect.Height -= 4;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    LinearGradientMode.Vertical)) {
                    using (Pen pen = new Pen(brush)) {
                        g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                    }
                }
            } else {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    180F)) {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0f, .2f, .5f, .8f, 1f };
                    blend.Factors = new float[] { 1f, .3f, 0f, .3f, 1f };
                    brush.Blend = blend;
                    using (Pen pen = new Pen(brush)) {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);

                        brush.LinearColors = new Color[] {
                        shadowColor, backColor };
                        pen.Brush = brush;
                        g.DrawLine(pen, rect.X, rect.Y + 1, rect.Right, rect.Y + 1);
                    }
                }
            }
        }


        public void RenderOverflowBackground(
            ToolStripItemRenderEventArgs e,
            bool rightToLeft) {
            Color color = Color.Empty;
            Graphics g = e.Graphics;
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripOverflowButton item = e.Item as ToolStripOverflowButton;
            Rectangle bounds = new Rectangle(Point.Empty, item.Size);
            Rectangle withinBounds = bounds;
            bool bParentIsMenuStrip = !(item.GetCurrentParent() is MenuStrip);
            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;

            if (bHorizontal) {
                bounds.X += (bounds.Width - 12) + 1;
                bounds.Width = 12;
                if (rightToLeft) {
                    bounds = LayoutUtils.RTLTranslate(bounds, withinBounds);
                }
            } else {
                bounds.Y = (bounds.Height - 12) + 1;
                bounds.Height = 12;
            }

            if (item.Pressed) {
                color = ColorTable.ItemPressed;
            } else if (item.Selected) {
                color = ColorTable.ItemHover;
            } else {
                color = ColorTable.Base;
            }
            if (bParentIsMenuStrip) {
                using (Pen pen = new Pen(ColorTable.Base)) {
                    Point point = new Point(bounds.Left - 1, bounds.Height - 2);
                    Point point2 = new Point(bounds.Left, bounds.Height - 2);
                    if (rightToLeft) {
                        point.X = bounds.Right + 1;
                        point2.X = bounds.Right;
                    }
                    g.DrawLine(pen, point, point2);
                }
            }

            LinearGradientMode mode = bHorizontal ?
                LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

            RenderHelperStrip.RenderBackgroundInternal(
                g,
                bounds,
                color,
                ColorTable.ItemBorder,
                ColorTable.Back,
                RoundStyle.None,
                0,
                .35f,
                false,
                false,
                mode);

            if (bParentIsMenuStrip) {
                using (Brush brush = new SolidBrush(ColorTable.Base)) {
                    if (bHorizontal) {
                        Point point3 = new Point(bounds.X - 2, 0);
                        Point point4 = new Point(bounds.X - 1, 1);
                        if (rightToLeft) {
                            point3.X = bounds.Right + 1;
                            point4.X = bounds.Right;
                        }
                        g.FillRectangle(brush, point3.X, point3.Y, 1, 1);
                        g.FillRectangle(brush, point4.X, point4.Y, 1, 1);
                    } else {
                        g.FillRectangle(brush, bounds.Width - 3, bounds.Top - 1, 1, 1);
                        g.FillRectangle(brush, bounds.Width - 2, bounds.Top - 2, 1, 1);
                    }
                }
                using (Brush brush = new SolidBrush(ColorTable.Base)) {
                    if (bHorizontal) {
                        Rectangle rect = new Rectangle(bounds.X - 1, 0, 1, 1);
                        if (rightToLeft) {
                            rect.X = bounds.Right;
                        }
                        g.FillRectangle(brush, rect);
                    } else {
                        g.FillRectangle(brush, bounds.X, bounds.Top - 1, 1, 1);
                    }
                }
            }
        }

        private void DrawDottedGrip(
            Graphics g,
            Rectangle bounds,
            bool vertical,
            bool largeDot,
            Color innerColor,
            Color outerColor) {
            bounds.Height -= 3;
            Point position = new Point(bounds.X, bounds.Y);
            int sep;
            Rectangle posRect = new Rectangle(0, 0, 2, 2);

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                IntPtr hdc;

                if (vertical) {
                    sep = bounds.Height;
                    position.Y += 8;
                    for (int i = 0; position.Y > 4; i += 4) {
                        position.Y = sep - (2 + i);
                        if (largeDot) {
                            posRect.Location = position;
                            DrawCircle(
                                g,
                                posRect,
                                outerColor,
                                innerColor);
                        } else {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);

                            hdc = g.GetHdc();

                            SetPixel(
                                hdc,
                                position.X,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 1,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X,
                                position.Y + 1,
                                outerWin32Corlor);

                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 4,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y + 1,
                                outerWin32Corlor);

                            g.ReleaseHdc(hdc);
                        }
                    }
                } else {
                    bounds.Inflate(-2, 0);
                    sep = bounds.Width;
                    position.X += 2;

                    for (int i = 1; position.X > 0; i += 4) {
                        position.X = sep - (2 + i);
                        if (largeDot) {
                            posRect.Location = position;
                            DrawCircle(g, posRect, outerColor, innerColor);
                        } else {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);
                            hdc = g.GetHdc();

                            SetPixel(
                                hdc,
                                position.X,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 1,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X,
                                position.Y + 1,
                                outerWin32Corlor);

                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 4,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y + 1,
                                outerWin32Corlor);

                            g.ReleaseHdc(hdc);
                        }
                    }
                }
            }
        }

        private void DrawCircle(
            Graphics g,
            Rectangle bounds,
            Color borderColor,
            Color fillColor) {
            using (GraphicsPath circlePath = new GraphicsPath()) {
                circlePath.AddEllipse(bounds);
                circlePath.CloseFigure();

                using (Pen borderPen = new Pen(borderColor)) {
                    g.DrawPath(borderPen, circlePath);
                }

                using (Brush backBrush = new SolidBrush(fillColor)) {
                    g.FillPath(backBrush, circlePath);
                }
            }
        }

        private void DrawDottedStatusGrip(
            Graphics g,
            Rectangle bounds,
            Color innerColor,
            Color outerColor) {
            Rectangle shape = new Rectangle(0, 0, 2, 2);
            shape.X = bounds.Width - 17;
            shape.Y = bounds.Height - 8;
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                DrawCircle(g, shape, outerColor, innerColor);

                shape.X = bounds.Width - 12;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.X = bounds.Width - 7;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 18;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                shape.X = bounds.Width - 12;
                DrawCircle(g, shape, outerColor, innerColor);
            }
        }

        private void DrawSolidStatusGrip(
            Graphics g,
            Rectangle bounds,
            Color innerColor,
            Color outerColor
            ) {
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                using (Pen innerPen = new Pen(innerColor),
                    outerPen = new Pen(outerColor)) {
                    //outer line
                    g.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 14, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 16));
                    g.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 13, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 15));
                    // line
                    g.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 12, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 14));
                    g.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 11, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 13));
                    // line
                    g.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 10, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 12));
                    g.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 9, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 11));
                    // line
                    g.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 8, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 10));
                    g.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 7, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 9));
                    // inner line
                    g.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 6, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 8));
                    g.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 5, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 7));
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
    }
}
