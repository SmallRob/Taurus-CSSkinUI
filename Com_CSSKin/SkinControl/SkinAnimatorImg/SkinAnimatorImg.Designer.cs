
namespace Com_CSSkin.SkinControl
{
    partial class SkinAnimatorImg
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
            this.components = new System.ComponentModel.Container();
            this.timShow = new System.Windows.Forms.Timer(this.components);
            this.timStart = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timShow
            // 
            this.timShow.Tick += new System.EventHandler(this.timShow_Tick);
            // 
            // timStart
            // 
            this.timStart.Enabled = true;
            this.timStart.Tick += new System.EventHandler(this.timStart_Tick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timShow;
        private System.Windows.Forms.Timer timStart;
    }
}
