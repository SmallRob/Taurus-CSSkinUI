
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 动画控制器
    /// </summary>
    public class Animation
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Advanced), TypeConverter(typeof (PointFConverter))]
        [Description("滑动系数")]
        public PointF SlideCoeff { get; set; }
        [Description("转动系数")]
        public float RotateCoeff { get; set; }
        [Description("旋转限度")]
        public float RotateLimit { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Advanced), TypeConverter(typeof(PointFConverter))]
        [Description("尺度系数")]
        public PointF ScaleCoeff { get; set; }
        [Description("透明系数")]
        public float TransparencyCoeff { get; set; }
        [Description("叶系数")]
        public float LeafCoeff { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Advanced), TypeConverter(typeof(PointFConverter))]
        [Description("马赛克偏移")]
        public PointF MosaicShift { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Advanced), TypeConverter(typeof(PointFConverter))]
        [Description("马赛克系数")]
        public PointF MosaicCoeff { get; set; }
        [Description("马赛克大小")]
        public int MosaicSize { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Advanced), TypeConverter(typeof(PointFConverter))]
        [Description("百叶窗系数")]
        public PointF BlindCoeff { get; set; }
        [Description("时间系数")]
        public float TimeCoeff { get; set; }
        [Description("最小时间")]
        public float MinTime { get; set; }
        [Description("最大时间")]
        public float MaxTime { get; set; }
        [Description("内边距")]
        public Padding Padding { get; set; }
        [Description("是否存在动画差异")]
        public bool AnimateOnlyDifferences { get;set;}

        public Animation()
        {
            MinTime = 0f;
            MaxTime = 1f;
            AnimateOnlyDifferences = true;
        }


        public Animation Clone()
        {
            return (Animation)MemberwiseClone();
        }


        public static Animation Rotate { get { return new Animation { RotateCoeff = 1f, TransparencyCoeff = 1, Padding = new Padding(50, 50, 50, 50) }; } }
        public static Animation HorizSlide { get { return new Animation { SlideCoeff = new PointF(1, 0) }; } }
        public static Animation VertSlide { get { return new Animation { SlideCoeff = new PointF(0, 1) }; } }
        public static Animation Scale { get { return new Animation { ScaleCoeff = new PointF(1, 1) }; } }
        public static Animation ScaleAndRotate { get { return new Animation { ScaleCoeff = new PointF(1, 1), RotateCoeff = 0.5f, RotateLimit = 0.2f, Padding = new Padding(30, 30, 30, 30) }; } }
        public static Animation HorizSlideAndRotate { get { return new Animation { SlideCoeff = new PointF(1, 0), RotateCoeff = 0.3f, RotateLimit = 0.2f,  Padding = new Padding(50, 50, 50, 50) }; } }
        public static Animation ScaleAndHorizSlide { get { return new Animation { ScaleCoeff = new PointF(1, 1), SlideCoeff = new PointF(1, 0), Padding = new Padding(30, 0, 0, 0) }; } }
        public static Animation Transparent { get { return new Animation { TransparencyCoeff = 1 }; } }
        public static Animation Leaf { get { return new Animation { LeafCoeff = 1 }; } }
        public static Animation Mosaic { get { return new Animation { MosaicCoeff = new PointF(100f, 100f), MosaicSize = 20, Padding = new Padding(30, 30, 30, 30) }; } }
        public static Animation Particles { get { return new Animation { MosaicCoeff = new PointF(200, 200), MosaicSize = 1, MosaicShift = new PointF(0, 0.5f), Padding = new Padding(100, 50, 100, 150), TimeCoeff = 2 }; } }
        public static Animation VertBlind { get { return new Animation { BlindCoeff = new PointF(0f, 1f) }; } }
        public static Animation HorizBlind { get { return new Animation { BlindCoeff = new PointF(1f, 0f) }; } }



        public void Add(Animation a)
        {
            SlideCoeff = new PointF(SlideCoeff.X + a.SlideCoeff.X, SlideCoeff.Y + a.SlideCoeff.Y);
            RotateCoeff += a.RotateCoeff;
            RotateLimit += a.RotateLimit;
            ScaleCoeff = new PointF(ScaleCoeff.X + a.ScaleCoeff.X, ScaleCoeff.Y + a.ScaleCoeff.Y);
            TransparencyCoeff += a.TransparencyCoeff;
            LeafCoeff += a.LeafCoeff;
            MosaicShift = new PointF(MosaicShift.X + a.MosaicShift.X, MosaicShift.Y + a.MosaicShift.Y);
            MosaicCoeff = new PointF(MosaicCoeff.X + a.MosaicCoeff.X, MosaicCoeff.Y + a.MosaicCoeff.Y);
            MosaicSize += a.MosaicSize;
            BlindCoeff = new PointF(BlindCoeff.X + a.BlindCoeff.X, BlindCoeff.Y + a.BlindCoeff.Y);
            TimeCoeff += a.TimeCoeff;
            Padding += a.Padding;
        }
    }

    public enum AnimationType
    {
        Custom = 0,
        Rotate,
        HorizSlide,
        VertSlide,
        Scale,
        ScaleAndRotate,
        HorizSlideAndRotate,
        ScaleAndHorizSlide,
        Transparent,
        Leaf,
        Mosaic,
        Particles,
        VertBlind,
        HorizBlind
    }                      
}