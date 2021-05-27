
namespace Com_CSSkin.SkinControl
{
    partial class SkinMonthCalendar
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkinMonthCalendar));
            this.lb_date = new System.Windows.Forms.Label();
            this.CutDataTime = new System.Windows.Forms.Label();
            this.BtnYeayLeft = new System.Windows.Forms.Label();
            this.BtnMonthLeft = new System.Windows.Forms.Label();
            this.BtnMonthRight = new System.Windows.Forms.Label();
            this.BtnYeayRight = new System.Windows.Forms.Label();
            this.CloseBtn = new Com_CSSkin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // lb_date
            // 
            this.lb_date.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_date.Location = new System.Drawing.Point(0, 157);
            this.lb_date.Name = "lb_date";
            this.lb_date.Size = new System.Drawing.Size(180, 18);
            this.lb_date.TabIndex = 0;
            this.lb_date.Text = "label1";
            this.lb_date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CutDataTime
            // 
            this.CutDataTime.AutoSize = true;
            this.CutDataTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutDataTime.Location = new System.Drawing.Point(44, 4);
            this.CutDataTime.Name = "CutDataTime";
            this.CutDataTime.Size = new System.Drawing.Size(67, 17);
            this.CutDataTime.TabIndex = 2;
            this.CutDataTime.Text = "2011年2月";
            // 
            // BtnYeayLeft
            // 
            this.BtnYeayLeft.BackColor = System.Drawing.Color.Transparent;
            this.BtnYeayLeft.Location = new System.Drawing.Point(5, 5);
            this.BtnYeayLeft.Name = "BtnYeayLeft";
            this.BtnYeayLeft.Size = new System.Drawing.Size(13, 13);
            this.BtnYeayLeft.TabIndex = 3;
            this.BtnYeayLeft.Click += new System.EventHandler(this.BtnYeayLeft_Click);
            this.BtnYeayLeft.MouseEnter += new System.EventHandler(this.BtnYeayLeft_MouseEnter);
            this.BtnYeayLeft.MouseLeave += new System.EventHandler(this.BtnYeayLeft_MouseLeave);
            // 
            // BtnMonthLeft
            // 
            this.BtnMonthLeft.BackColor = System.Drawing.Color.Transparent;
            this.BtnMonthLeft.Location = new System.Drawing.Point(23, 5);
            this.BtnMonthLeft.Name = "BtnMonthLeft";
            this.BtnMonthLeft.Size = new System.Drawing.Size(13, 13);
            this.BtnMonthLeft.TabIndex = 4;
            this.BtnMonthLeft.Click += new System.EventHandler(this.BtnMonthLeft_Click);
            this.BtnMonthLeft.MouseEnter += new System.EventHandler(this.BtnMonthLeft_MouseEnter);
            this.BtnMonthLeft.MouseLeave += new System.EventHandler(this.BtnMonthLeft_MouseLeave);
            // 
            // BtnMonthRight
            // 
            this.BtnMonthRight.BackColor = System.Drawing.Color.Transparent;
            this.BtnMonthRight.Location = new System.Drawing.Point(124, 5);
            this.BtnMonthRight.Name = "BtnMonthRight";
            this.BtnMonthRight.Size = new System.Drawing.Size(13, 13);
            this.BtnMonthRight.TabIndex = 5;
            this.BtnMonthRight.Click += new System.EventHandler(this.BtnMonthRight_Click);
            this.BtnMonthRight.MouseEnter += new System.EventHandler(this.BtnMonthRight_MouseEnter);
            this.BtnMonthRight.MouseLeave += new System.EventHandler(this.BtnMonthRight_MouseLeave);
            // 
            // BtnYeayRight
            // 
            this.BtnYeayRight.BackColor = System.Drawing.Color.Transparent;
            this.BtnYeayRight.Location = new System.Drawing.Point(141, 5);
            this.BtnYeayRight.Name = "BtnYeayRight";
            this.BtnYeayRight.Size = new System.Drawing.Size(13, 13);
            this.BtnYeayRight.TabIndex = 6;
            this.BtnYeayRight.Click += new System.EventHandler(this.BtnYeayRight_Click);
            this.BtnYeayRight.MouseEnter += new System.EventHandler(this.BtnYeayRight_MouseEnter);
            this.BtnYeayRight.MouseLeave += new System.EventHandler(this.BtnYeayRight_MouseLeave);
            // 
            // CloseBtn
            // 
            this.CloseBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseBtn.ControlState = Com_CSSkin.SkinClass.ControlState.Normal;
            this.CloseBtn.DownBack = ((System.Drawing.Image)(resources.GetObject("CloseBtn.DownBack")));
            this.CloseBtn.DrawType = Com_CSSkin.SkinControl.DrawStyle.Img;
            this.CloseBtn.Location = new System.Drawing.Point(159, 3);
            this.CloseBtn.MouseBack = ((System.Drawing.Image)(resources.GetObject("CloseBtn.MouseBack")));
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.NormlBack = ((System.Drawing.Image)(resources.GetObject("CloseBtn.NormlBack")));
            this.CloseBtn.Size = new System.Drawing.Size(16, 16);
            this.CloseBtn.TabIndex = 1;
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            this.CloseBtn.MouseEnter += new System.EventHandler(this.CloseBtn_MouseEnter);
            this.CloseBtn.MouseLeave += new System.EventHandler(this.CloseBtn_MouseLeave);
            // 
            // SkinMonthCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnYeayRight);
            this.Controls.Add(this.BtnMonthRight);
            this.Controls.Add(this.BtnMonthLeft);
            this.Controls.Add(this.BtnYeayLeft);
            this.Controls.Add(this.CutDataTime);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.lb_date);
            this.Name = "SkinMonthCalendar";
            this.Size = new System.Drawing.Size(180, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_date;
        private Com_CSSkin.SkinControl.SkinButton  CloseBtn;
        private System.Windows.Forms.Label CutDataTime;
        private System.Windows.Forms.Label BtnYeayLeft;
        private System.Windows.Forms.Label BtnMonthLeft;
        private System.Windows.Forms.Label BtnMonthRight;
        private System.Windows.Forms.Label BtnYeayRight;

    }
}
