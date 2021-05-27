
namespace Com_CSSkin.SkinControl
{
    partial class SkinTextBox
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
            this.BaseText = new Com_CSSkin.SkinControl.SkinWaterTextBox();
            this.SuspendLayout();
            // 
            // BaseText
            // 
            this.BaseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseText.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.BaseText.Location = new System.Drawing.Point(5, 5);
            this.BaseText.Name = "BaseText";
            this.BaseText.Size = new System.Drawing.Size(175, 18);
            this.BaseText.TabIndex = 0;
            this.BaseText.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.BaseText.WaterText = "";
            // 
            // SkinTextBox
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.BaseText);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(28, 28);
            this.Name = "SkinTextBox";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(185, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SkinWaterTextBox BaseText;


    }
}
