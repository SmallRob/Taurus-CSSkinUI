
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using System.Collections;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// SkinMonthCalendar控件
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.MonthCalendar))]
    public partial class SkinMonthCalendar : UserControl
    {
        #region 声明
        private Bitmap _BackImg = Properties.Resources.AlMonthCalendarBg;
        string[] Weekday = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        DateTime[] Cutday = new DateTime[42];
        public SkinDateTimePicker SkinTimerPicker;
        private DateTime Cut_Date;
        #endregion

        private DateTime MinDate {
            get;
            set;
        }
        private DateTime MaxDate {
            get;
            set;
        }

        public SkinMonthCalendar() {
            InitializeComponent();
            #region 去闪烁
            //设置自定义控件Style
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            #endregion
            this.BackColor = Color.Transparent;
            CloseBtn.BackgroundImage = Properties.Resources.AlMonthCalendarBtn;
            this.BackgroundImage = _BackImg;
            //this.Today = DateTime.Today;
            //this.Today_Year = DateTime.Today.Year;
            //this.Today_Month = DateTime.Today.Month;
            //this.Today_Day = DateTime.Today.Day;
            //自定义显示格式
            // lb_date.Text = Today_Year + "年" + Today_Month + "月";
            DateSet();//初始设置
            ShowDate(2010, 2);
        }
        public SkinMonthCalendar(SkinDateTimePicker AlPicker)
            : base() {
            InitializeComponent();
            this.BackColor = Color.Transparent;
            SkinTimerPicker = AlPicker;
            BtnYeayLeft.Image = Properties.Resources.AlMonthCalendar01;
            BtnMonthLeft.Image = Properties.Resources.AlMonthCalendar02;
            BtnMonthRight.Image = Properties.Resources.AlMonthCalendar03;
            BtnYeayRight.Image = Properties.Resources.AlMonthCalendar04;
            CloseBtn.BackgroundImage = Properties.Resources.AlMonthCalendarBtn;
            this.BackgroundImage = _BackImg;
            this.Cut_Date = DateTime.Today;
            DateSet();//初始设置
            ShowDate(Cut_Date.Year, Cut_Date.Month);
            lb_date.Text = Weekday[(int)DateTime.Today.DayOfWeek] + " , " + DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;

        }
        private void DateSet() {
            #region 星期
            //周标题数组
            SkinLabel[] WeekScheme = new SkinLabel[7];
            string[] week = new string[] { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i <= 6; i++) {
                WeekScheme[i] = new SkinLabel();
                WeekScheme[i].ArtTextStyle = ArtTextStyle.None;
                WeekScheme[i].Text = week[i];
                WeekScheme[i].Width = 20;
                WeekScheme[i].Height = 16;
                WeekScheme[i].Font = new Font("微软雅黑", 9.5F);
                WeekScheme[i].Location = new Point(3 + i * 25, 25);
                WeekScheme[i].ForeColor = Color.FromArgb(44, 74, 137);
            }
            WeekScheme[0].ForeColor = WeekScheme[6].ForeColor = Color.FromArgb(255, 128, 128);
            #endregion
            this.Controls.AddRange(WeekScheme);
            int DateNum = 0;
            SkinLabel[] DateDay = new SkinLabel[42];
            for (int i = 0; i < 6; i++) {
                int x = 1;
                int y = 47 + i * 18;
                for (int j = 0; j < 7; j++) {
                    x = 1 + j * 25;
                    DateDay[DateNum] = new SkinLabel();
                    DateDay[DateNum].ArtTextStyle = ArtTextStyle.None;
                    DateDay[DateNum].Name = "CutDataDay" + DateNum.ToString(); ;
                    DateDay[DateNum].Width = 23;
                    DateDay[DateNum].Height = 14;
                    DateDay[DateNum].Font = new Font("微软雅黑", 8F);
                    DateDay[DateNum].Location = new Point(x, y);
                    DateDay[DateNum].TextAlign = ContentAlignment.MiddleRight;
                    DateNum++;
                }
            }
            this.Controls.AddRange(DateDay);
        }

        private void ShowDate(int year, int month) {
            CutDataTime.Text = year + "年" + "," + month + "月";
            int[] DataDayLen = new int[] { 31, 30, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DataDayLen[1] = ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) ? 29 : 28;
            DateTime MyDate = DateTime.Parse(year + "-" + month + "-" + 1);
            DateTime Today = DateTime.Today;
            int Day = (int)MyDate.DayOfWeek;

            for (int L = 0; L < 42; L++) {
                SkinLabel CutDataDay = (SkinLabel)FindControl("CutDataDay" + L);
                CutDataDay.Text = "";
                CutDataDay.BackColor = Color.Transparent;
                //前一个月
                if (Day != 0 && L < Day) {
                    CutDataDay.Text = (DataDayLen[MyDate.AddMonths(-1).Month - 1] - Day + L + 1).ToString();
                    CutDataDay.ForeColor = Color.DarkGray;
                    Cutday[L] = DateTime.Parse(MyDate.AddMonths(-1).Year + "-" + MyDate.AddMonths(-1).Month + "-" + (DataDayLen[MyDate.AddMonths(-1).Month - 1] - Day + L + 1).ToString());

                    CutDataDay.Tag = Cutday[L];
                }
                //当前月
                if (L >= Day && L < DataDayLen[MyDate.Month - 1] + Day) {
                    CutDataDay.Text = (L - Day + 1).ToString();

                    int day = Convert.ToInt32(CutDataDay.Text);
                    CutDataDay.Tag = DateTime.Parse(MyDate.Year + "-" + MyDate.Month + "-" + CutDataDay.Text);
                    //Cutday[L] = DateTime.Parse(MyDate.Year + "-" + MyDate.Month + "-" + CutDataDay.Text);
                }
                //下一个月
                if (L >= DataDayLen[MyDate.Month - 1] + Day) {
                    CutDataDay.Text = (L - DataDayLen[MyDate.Month - 1] - Day + 1).ToString();
                    Cutday[L] = DateTime.Parse(MyDate.AddMonths(+1).Year + "-" + MyDate.AddMonths(+1).Month + "-" + (L - DataDayLen[MyDate.Month - 1] - Day + 1).ToString());
                    CutDataDay.ForeColor = Color.DarkGray;
                    CutDataDay.Tag = Cutday[L];
                }
                CutDataDay.MouseLeave += new EventHandler(DateDay_MouseLeave);
                CutDataDay.MouseClick += new MouseEventHandler(DateDay_MouseClick);
                CutDataDay.MouseEnter += new EventHandler(DateDay_MouseEnter);
            }


            //for (int i = 0; i < DataDayLen[month - 1]; i++)
            //{
            //    object CutDataDay = FindControl("CutDataDay" + (i +Day).ToString());
            //    if (CutDataDay != null)
            //    {
            //        ((SkinLabel)CutDataDay).Text = (i + 1).ToString();
            //        ((SkinLabel)CutDataDay).MouseLeave += new EventHandler(DateDay_MouseLeave);
            //        ((SkinLabel)CutDataDay).MouseClick += new MouseEventHandler(DateDay_MouseClick);
            //        ((SkinLabel)CutDataDay).MouseEnter += new EventHandler(DateDay_MouseEnter);
            //        if (DateTime.Today.Day == (i + 1) && DateTime.Today.Year == year && DateTime.Today.Month==month)
            //        {
            //            //((SkinLabel)CutDataDay).ForeColor = Color.Red;
            //            //((SkinLabel)CutDataDay).Font = new Font("微软雅黑", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            //            ((SkinLabel)CutDataDay).BackColor = Color.FromArgb(254, 141, 141);
            //        }
            //    }              
            //    MyDate = DateTime.Parse(year + "-" + month + "-" + (i+1).ToString());
            //    if ((int)MyDate.DayOfWeek == 6 || (int)MyDate.DayOfWeek == 0)
            //    {
            //        CutDataDay = FindControl(this, "CutDataDay" + (i + (int)Day).ToString());
            //        if (CutDataDay != null)
            //        {
            //            ((SkinLabel)CutDataDay).ForeColor = Color.FromArgb(254, 141, 141);
            //        }
            //    }

            //}

        }
        private Control FindControl(string ControlName) {
            return this.Controls[ControlName];
        }
        public static Control FindControl(Control p_Control, string p_ControlName) {
            if (p_Control.Name == p_ControlName) return p_Control;
            for (int i = 0; i != p_Control.Controls.Count; i++) {
                Control _SubControl = FindControl(p_Control.Controls[i], p_ControlName);
                if (_SubControl != null) return _SubControl;
            }
            return null;
        }


        private void DateDay_MouseLeave(object sender, EventArgs e) {
            SkinLabel CuSkinLabel = (SkinLabel)sender;
            CuSkinLabel.BackColor = Color.Transparent;
        }

        private void DateDay_MouseEnter(object sender, EventArgs e) {
            SkinLabel CuSkinLabel = (SkinLabel)sender;
            CuSkinLabel.BackColor = Color.FromArgb(168, 219, 255);
            //int L = Convert.ToInt32(CuSkinLabel.Tag);
            //lb_date.Text = Weekday[(int)(Cutday[L].DayOfWeek)] + " , " + Cutday[L].ToString("yyyy-MM-dd");
            if (CuSkinLabel.Tag != null) {
                DateTime time = Convert.ToDateTime(CuSkinLabel.Tag);
                lb_date.Text = Weekday[(int)time.DayOfWeek] + " , " + time.Year + "/" + time.Month + "/" + time.Day;
            } else {
                lb_date.Text = Weekday[(int)DateTime.Today.DayOfWeek] + " , " + DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
            }
        }

        private void DateDay_MouseClick(object sender, MouseEventArgs e) {
            SkinLabel CuSkinLabel = (SkinLabel)sender;
            if (SkinTimerPicker != null) {
                SkinTimerPicker.text = Cut_Date.Year + "-" + Cut_Date.Month + "-" + CuSkinLabel.Text;
                SkinTimerPicker.SkinDropDownClose();
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e) {
            if (SkinTimerPicker != null) {
                SkinTimerPicker.SkinDropDownClose();
            }
        }

        private void CloseBtn_MouseEnter(object sender, EventArgs e) {
            lb_date.Text = "关闭";
        }

        private void CloseBtn_MouseLeave(object sender, EventArgs e) {
            lb_date.Text = "";
        }

        private void BtnYeayLeft_Click(object sender, EventArgs e) {
            Cut_Date = Cut_Date.AddYears(-1);
            ShowDate(Cut_Date.Year, Cut_Date.Month);
        }

        private void BtnMonthLeft_Click(object sender, EventArgs e) {
            Cut_Date = Cut_Date.AddMonths(-1);
            ShowDate(Cut_Date.Year, Cut_Date.Month);
        }

        private void BtnMonthRight_Click(object sender, EventArgs e) {
            Cut_Date = Cut_Date.AddMonths(+1);
            ShowDate(Cut_Date.Year, Cut_Date.Month);
        }

        private void BtnYeayRight_Click(object sender, EventArgs e) {
            Cut_Date = Cut_Date.AddYears(+1);
            ShowDate(Cut_Date.Year, Cut_Date.Month);
        }

        private void BtnYeayLeft_MouseEnter(object sender, EventArgs e) {
            BtnYeayLeft.BackColor = Color.FromArgb(168, 219, 255);

        }

        private void BtnYeayLeft_MouseLeave(object sender, EventArgs e) {
            BtnYeayLeft.BackColor = Color.Transparent;
        }

        private void BtnMonthLeft_MouseEnter(object sender, EventArgs e) {
            BtnMonthLeft.BackColor = Color.FromArgb(168, 219, 255);
        }

        private void BtnMonthLeft_MouseLeave(object sender, EventArgs e) {
            BtnMonthLeft.BackColor = Color.Transparent;
        }

        private void BtnMonthRight_MouseEnter(object sender, EventArgs e) {
            BtnMonthRight.BackColor = Color.FromArgb(168, 219, 255);
        }

        private void BtnMonthRight_MouseLeave(object sender, EventArgs e) {
            BtnMonthRight.BackColor = Color.Transparent;
        }

        private void BtnYeayRight_MouseEnter(object sender, EventArgs e) {
            BtnYeayRight.BackColor = Color.FromArgb(168, 219, 255);
        }

        private void BtnYeayRight_MouseLeave(object sender, EventArgs e) {
            BtnYeayRight.BackColor = Color.Transparent;
        }
    }
}
