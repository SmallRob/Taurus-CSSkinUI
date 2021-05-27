
namespace Com_CSSkin.SkinControl
{
    partial class ChatListBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.scorllTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // scorllTimer
            // 
            this.scorllTimer.Enabled = true;
            this.scorllTimer.Interval = 15;
            this.scorllTimer.Tick += new System.EventHandler(this.scorllTimer_Tick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer scorllTimer;
    }
}
