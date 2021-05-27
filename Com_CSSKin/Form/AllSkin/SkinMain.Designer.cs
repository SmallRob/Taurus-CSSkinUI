
namespace Com_CSSkin
{
    partial class SkinMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timShow = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timShow
            // 
            this.timShow.Enabled = true;
            this.timShow.Interval = 10;
            this.timShow.Tick += new System.EventHandler(this.timShow_Tick);
            // 
            // SkinMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SkinMain";
            this.ShowInTaskbar = false;
            this.Text = "SkinMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timShow;


    }
}