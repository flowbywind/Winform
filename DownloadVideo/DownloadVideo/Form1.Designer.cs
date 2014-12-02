namespace DownloadVideo
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnDownload = new System.Windows.Forms.Button();
            this.TxtVideoUrl = new System.Windows.Forms.TextBox();
            this.TxtResult = new System.Windows.Forms.RichTextBox();
            this.BtnGetVideoUrl = new System.Windows.Forms.Button();
            this.txtVideoTitle = new System.Windows.Forms.Label();
            this.BtnAuto = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.BtnControl = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnDownload
            // 
            this.BtnDownload.Location = new System.Drawing.Point(191, 219);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(95, 23);
            this.BtnDownload.TabIndex = 0;
            this.BtnDownload.Text = "下载";
            this.BtnDownload.UseVisualStyleBackColor = true;
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // TxtVideoUrl
            // 
            this.TxtVideoUrl.Location = new System.Drawing.Point(28, 12);
            this.TxtVideoUrl.Name = "TxtVideoUrl";
            this.TxtVideoUrl.Size = new System.Drawing.Size(418, 21);
            this.TxtVideoUrl.TabIndex = 1;
            // 
            // TxtResult
            // 
            this.TxtResult.Location = new System.Drawing.Point(28, 66);
            this.TxtResult.Name = "TxtResult";
            this.TxtResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.TxtResult.Size = new System.Drawing.Size(418, 147);
            this.TxtResult.TabIndex = 2;
            this.TxtResult.Text = "";
            // 
            // BtnGetVideoUrl
            // 
            this.BtnGetVideoUrl.Location = new System.Drawing.Point(28, 219);
            this.BtnGetVideoUrl.Name = "BtnGetVideoUrl";
            this.BtnGetVideoUrl.Size = new System.Drawing.Size(143, 23);
            this.BtnGetVideoUrl.TabIndex = 3;
            this.BtnGetVideoUrl.Text = "解析视频地址";
            this.BtnGetVideoUrl.UseVisualStyleBackColor = true;
            this.BtnGetVideoUrl.Click += new System.EventHandler(this.BtnGetVideoUrl_Click);
            // 
            // txtVideoTitle
            // 
            this.txtVideoTitle.AutoSize = true;
            this.txtVideoTitle.Location = new System.Drawing.Point(36, 51);
            this.txtVideoTitle.Name = "txtVideoTitle";
            this.txtVideoTitle.Size = new System.Drawing.Size(53, 12);
            this.txtVideoTitle.TabIndex = 4;
            this.txtVideoTitle.Text = "视频名称";
            // 
            // BtnAuto
            // 
            this.BtnAuto.Location = new System.Drawing.Point(28, 286);
            this.BtnAuto.Name = "BtnAuto";
            this.BtnAuto.Size = new System.Drawing.Size(75, 23);
            this.BtnAuto.TabIndex = 5;
            this.BtnAuto.Text = "自动执行";
            this.BtnAuto.UseVisualStyleBackColor = true;
            this.BtnAuto.Click += new System.EventHandler(this.BtnAuto_Click);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(28, 342);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(394, 23);
            this.progressBarDownload.TabIndex = 6;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.ForeColor = System.Drawing.Color.Chocolate;
            this.lblProgress.Location = new System.Drawing.Point(370, 383);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 12);
            this.lblProgress.TabIndex = 7;
            // 
            // BtnControl
            // 
            this.BtnControl.Location = new System.Drawing.Point(139, 285);
            this.BtnControl.Name = "BtnControl";
            this.BtnControl.Size = new System.Drawing.Size(75, 23);
            this.BtnControl.TabIndex = 8;
            this.BtnControl.Text = "暂停";
            this.BtnControl.UseVisualStyleBackColor = true;
            this.BtnControl.Click += new System.EventHandler(this.BtnControl_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkCyan;
            this.label1.Location = new System.Drawing.Point(28, 324);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "运行信息";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblInfo.Location = new System.Drawing.Point(87, 324);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(65, 12);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "初始化数据";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 425);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnControl);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.BtnAuto);
            this.Controls.Add(this.txtVideoTitle);
            this.Controls.Add(this.BtnGetVideoUrl);
            this.Controls.Add(this.TxtResult);
            this.Controls.Add(this.TxtVideoUrl);
            this.Controls.Add(this.BtnDownload);
            this.Name = "Form1";
            this.Text = "自动下载视频";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnDownload;
        private System.Windows.Forms.TextBox TxtVideoUrl;
        private System.Windows.Forms.RichTextBox TxtResult;
        private System.Windows.Forms.Button BtnGetVideoUrl;
        private System.Windows.Forms.Label txtVideoTitle;
        private System.Windows.Forms.Button BtnAuto;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button BtnControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfo;
    }
}

