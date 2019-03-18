namespace DlHelper
{
    partial class ClsHelper
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
            this.cmID = new System.Windows.Forms.ComboBox();
            this.tbData = new System.Windows.Forms.TextBox();
            this.cmRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmID
            // 
            this.cmID.FormattingEnabled = true;
            this.cmID.Location = new System.Drawing.Point(68, 12);
            this.cmID.Name = "cmID";
            this.cmID.Size = new System.Drawing.Size(121, 20);
            this.cmID.TabIndex = 17;
            // 
            // tbData
            // 
            this.tbData.Location = new System.Drawing.Point(68, 72);
            this.tbData.Name = "tbData";
            this.tbData.Size = new System.Drawing.Size(121, 21);
            this.tbData.TabIndex = 16;
            // 
            // cmRate
            // 
            this.cmRate.Location = new System.Drawing.Point(68, 42);
            this.cmRate.Name = "cmRate";
            this.cmRate.Size = new System.Drawing.Size(121, 21);
            this.cmRate.TabIndex = 15;
            this.cmRate.Text = "9600";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "波特率";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "端口号";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmID);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.cmRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(206, 117);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmID;
        private System.Windows.Forms.TextBox tbData;
        private System.Windows.Forms.TextBox cmRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}

