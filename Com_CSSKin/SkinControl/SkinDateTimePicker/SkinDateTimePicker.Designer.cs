
namespace Com_CSSkin.SkinControl
{
    partial class SkinDateTimePicker
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
                _TextBoxBackImg.Dispose();
                _SkinDropDown.Dispose();
                _SkinMonthCalendar.Dispose();
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
            this.BaseText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BaseText
            // 
            this.BaseText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseText.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BaseText.Location = new System.Drawing.Point(3, 3);
            this.BaseText.Name = "BaseText";
            this.BaseText.Size = new System.Drawing.Size(88, 16);
            this.BaseText.TabIndex = 2;
            this.BaseText.MouseEnter += new System.EventHandler(this.BaseText_MouseEnter);
            this.BaseText.MouseLeave += new System.EventHandler(this.BaseText_MouseLeave);
            // 
            // SkinDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BaseText);
            this.Name = "SkinDateTimePicker";
            this.Size = new System.Drawing.Size(114, 22);
            this.Leave += new System.EventHandler(this.AlDateTimePicker_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AlDateTimePicker_MouseDown);
            this.MouseEnter += new System.EventHandler(this.AlDateTimePicker_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.AlDateTimePicker_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BaseText;


    }
}
