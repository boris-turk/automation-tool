namespace AutomationEngine
{
    partial class AddFileGroupForm
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
            this._id = new System.Windows.Forms.TextBox();
            this._directory = new System.Windows.Forms.TextBox();
            this._menuFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _id
            // 
            this._id.Location = new System.Drawing.Point(97, 15);
            this._id.Name = "_id";
            this._id.Size = new System.Drawing.Size(183, 20);
            this._id.TabIndex = 0;
            // 
            // _directory
            // 
            this._directory.Location = new System.Drawing.Point(97, 62);
            this._directory.Name = "_directory";
            this._directory.Size = new System.Drawing.Size(183, 20);
            this._directory.TabIndex = 2;
            // 
            // _menuFileName
            // 
            this._menuFileName.Location = new System.Drawing.Point(97, 39);
            this._menuFileName.Name = "_menuFileName";
            this._menuFileName.Size = new System.Drawing.Size(183, 20);
            this._menuFileName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Directory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Id";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Menu file name";
            // 
            // AddFileGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 100);
            this.Controls.Add(this._id);
            this.Controls.Add(this._directory);
            this.Controls.Add(this._menuFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "AddFileGroupForm";
            this.Text = "AddFileGroupForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _id;
        private System.Windows.Forms.TextBox _directory;
        private System.Windows.Forms.TextBox _menuFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}