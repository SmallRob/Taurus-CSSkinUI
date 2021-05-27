
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    public class ToolStripColorTable
    {
        private Color _base = Color.FromArgb(105, 200, 254);
        private Color _itemborder = Color.FromArgb(60, 148, 212);
        private Color _back = Color.White;
        private Color _itemHover = Color.FromArgb(60, 148, 212);
        private Color _itemPressed = Color.FromArgb(60, 148, 212);
        private Color _fore = Color.Black;
        private Color _dropDownImageSeparator = Color.FromArgb(197, 197, 197);
        private RoundStyle _radiusstyle = RoundStyle.All;
        private int _backradius = 4;
        private Color _titleColor = Color.FromArgb(209, 228, 236);
        private bool _titleAnamorphosis = true;
        private int _titleRadius = 4;
        private RoundStyle _titleRadiusStyle = RoundStyle.All;
        private RoundStyle _itemRadiusStyle = RoundStyle.All;
        private int _itemRadius = 4;
        private bool _itemAnamorphosis = true;
        private bool _itemBorderShow = true;
        private Color _hoverFore = Color.White;
        private Color _arrow = Color.Black;
        private Color _baseFore = Color.Black;
        private Color _baseHoverFore = Color.White;
        private RoundStyle _baseItemRadiusStyle = RoundStyle.All;
        private int _baseItemRadius = 4;
        private bool _baseItemBorderShow = true;
        private bool _baseItemAnamorphosis = true;
        private Color _baseItemBorder = Color.FromArgb(60, 148, 212);
        private Color _baseItemHover = Color.FromArgb(60, 148, 212);
        private Color _baseItemPressed = Color.FromArgb(60, 148, 212);
        private Color _baseItemSplitter = Color.FromArgb(60, 148, 212);
        private bool _baseForeAnamorphosis = false;
        private int _baseForeAnamorphosisBorder = 4;
        private Color _baseForeAnamorphosisColor = Color.White;
        private Image _baseItemMouse = Properties.Resources.allbtn_highlight;
        private Image _baseItemDown = Properties.Resources.allbtn_down;
        private Image _baseItemNorml = null;
        private Rectangle _backrectangle = new Rectangle(10, 10, 10, 10);
        private Point _baseForeOffset = new Point(0, 0);
        private bool _skinAllColor = true;

        public ToolStripColorTable() { }

        public bool SkinAllColor {
            get { return _skinAllColor; }
            set { _skinAllColor = value; }
        }

        public Rectangle BackRectangle {
            get { return _backrectangle; }
            set { _backrectangle = value; }
        }

        public Image BaseItemMouse {
            get { return _baseItemMouse; }
            set { _baseItemMouse = value; }
        }

        public Image BaseItemDown {
            get { return _baseItemDown; }
            set { _baseItemDown = value; }
        }
        public Image BaseItemNorml {
            get { return _baseItemNorml; }
            set { _baseItemNorml = value; }
        }

        public Point BaseForeOffset {
            get { return _baseForeOffset; }
            set { _baseForeOffset = value; }
        }

        public bool BaseForeAnamorphosis {
            get { return _baseForeAnamorphosis; }
            set { _baseForeAnamorphosis = value; }
        }

        public int BaseForeAnamorphosisBorder {
            get { return _baseForeAnamorphosisBorder; }
            set { _baseForeAnamorphosisBorder = value; }
        }

        public Color BaseForeAnamorphosisColor {
            get { return _baseForeAnamorphosisColor; }
            set { _baseForeAnamorphosisColor = value; }
        }

        public Color BaseFore {
            get { return _baseFore; }
            set { _baseFore = value; }
        }

        public Color BaseItemPressed {
            get { return _baseItemPressed; }
            set { _baseItemPressed = value; }
        }

        public Color BaseItemSplitter {
            get { return _baseItemSplitter; }
            set { _baseItemSplitter = value; }
        }

        public Color BaseItemBorder {
            get { return _baseItemBorder; }
            set { _baseItemBorder = value; }
        }

        public Color BaseItemHover {
            get { return _baseItemHover; }
            set { _baseItemHover = value; }
        }

        public int BaseItemRadius {
            get { return _baseItemRadius; }
            set { _baseItemRadius = value; }
        }

        public RoundStyle BaseItemRadiusStyle {
            get { return _baseItemRadiusStyle; }
            set { _baseItemRadiusStyle = value; }
        }

        public bool BaseItemBorderShow {
            get { return _baseItemBorderShow; }
            set { _baseItemBorderShow = value; }
        }

        public bool BaseItemAnamorphosis {
            get { return _baseItemAnamorphosis; }
            set { _baseItemAnamorphosis = value; }
        }

        public Color BaseHoverFore {
            get { return _baseHoverFore; }
            set { _baseHoverFore = value; }
        }

        public Color TitleColor {
            get { return _titleColor; }
            set { _titleColor = value; }
        }

        public Color Arrow {
            get { return _arrow; }
            set { _arrow = value; }
        }

        public bool TitleAnamorphosis {
            get { return _titleAnamorphosis; }
            set { _titleAnamorphosis = value; }
        }

        public int TitleRadius {
            get { return _titleRadius; }
            set { _titleRadius = value; }
        }

        public RoundStyle TitleRadiusStyle {
            get { return _titleRadiusStyle; }
            set { _titleRadiusStyle = value; }
        }

        public int BackRadius {
            get { return _backradius; }
            set { _backradius = value; }
        }

        public RoundStyle RadiusStyle {
            get { return _radiusstyle; }
            set { _radiusstyle = value; }
        }

        public Color Base {
            get { return _base; }
            set { _base = value; }
        }

        public Color ItemBorder {
            get { return _itemborder; }
            set { _itemborder = value; }
        }

        public bool ItemBorderShow {
            get { return _itemBorderShow; }
            set { _itemBorderShow = value; }
        }

        public Color Back {
            get { return _back; }
            set { _back = value; }
        }

        public Color ItemHover {
            get { return _itemHover; }
            set { _itemHover = value; }
        }

        public Color ItemPressed {
            get { return _itemPressed; }
            set { _itemPressed = value; }
        }

        public RoundStyle ItemRadiusStyle {
            get { return _itemRadiusStyle; }
            set { _itemRadiusStyle = value; }
        }

        public int ItemRadius {
            get { return _itemRadius; }
            set { _itemRadius = value; }
        }

        public bool ItemAnamorphosis {
            get { return _itemAnamorphosis; }
            set { _itemAnamorphosis = value; }
        }

        public Color Fore {
            get { return _fore; }
            set { _fore = value; }
        }

        public Color HoverFore {
            get { return _hoverFore; }
            set { _hoverFore = value; }
        }

        public Color DropDownImageSeparator {
            get { return _dropDownImageSeparator; }
            set { _dropDownImageSeparator = value; }
        }
    }
}
