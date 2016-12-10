namespace AutomationEngine
{
    partial class CreateApplicationMenuForm
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
            this._contextRegex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._menuFileName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _contextRegex
            // 
            this._contextRegex.Location = new System.Drawing.Point(99, 15);
            this._contextRegex.Name = "_contextRegex";
            this._contextRegex.Size = new System.Drawing.Size(356, 20);
            this._contextRegex.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Context regex";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Menu file name";
            // 
            // _menuFileName
            // 
            this._menuFileName.Location = new System.Drawing.Point(99, 41);
            this._menuFileName.Name = "_menuFileName";
            this._menuFileName.Size = new System.Drawing.Size(356, 20);
            this._menuFileName.TabIndex = 1;
            // 
            // CreateApplicationMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 80);
            this.Controls.Add(this._menuFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._contextRegex);
            this.Controls.Add(this.label2);
            this.Name = "CreateApplicationMenuForm";
            this.Text = "CreateApplicationMenuForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _contextRegex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _menuFileName;
    }
}