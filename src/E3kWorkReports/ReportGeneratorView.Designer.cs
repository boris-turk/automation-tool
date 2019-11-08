namespace E3kWorkReports
{
    partial class ReportGeneratorView
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
            this.label1 = new System.Windows.Forms.Label();
            this.Directory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BorisFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AndrejFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BorutFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory";
            // 
            // Directory
            // 
            this.Directory.Location = new System.Drawing.Point(120, 34);
            this.Directory.Name = "Directory";
            this.Directory.Size = new System.Drawing.Size(270, 20);
            this.Directory.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Boris report file";
            // 
            // BorisFile
            // 
            this.BorisFile.Location = new System.Drawing.Point(120, 59);
            this.BorisFile.Name = "BorisFile";
            this.BorisFile.Size = new System.Drawing.Size(270, 20);
            this.BorisFile.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Andrej report file";
            // 
            // AndrejFile
            // 
            this.AndrejFile.Location = new System.Drawing.Point(120, 84);
            this.AndrejFile.Name = "AndrejFile";
            this.AndrejFile.Size = new System.Drawing.Size(270, 20);
            this.AndrejFile.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Borut report file";
            // 
            // BorutFile
            // 
            this.BorutFile.Location = new System.Drawing.Point(120, 109);
            this.BorutFile.Name = "BorutFile";
            this.BorutFile.Size = new System.Drawing.Size(270, 20);
            this.BorutFile.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(163, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnGenerateReportClick);
            // 
            // ReportGeneratorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 205);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BorutFile);
            this.Controls.Add(this.AndrejFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BorisFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Directory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReportGeneratorView";
            this.Text = "ReportGeneratorView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Directory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BorisFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AndrejFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BorutFile;
        private System.Windows.Forms.Button button1;
    }
}