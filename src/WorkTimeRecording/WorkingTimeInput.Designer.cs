namespace WorkTimeRecording
{
    partial class WorkingTimeInput
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
            this._description = new System.Windows.Forms.TextBox();
            this._date = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // _description
            // 
            this._description.Location = new System.Drawing.Point(16, 38);
            this._description.Multiline = true;
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(408, 71);
            this._description.TabIndex = 3;
            // 
            // _date
            // 
            this._date.Location = new System.Drawing.Point(16, 12);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(132, 20);
            this._date.TabIndex = 0;
            // 
            // WorkingTimeInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 125);
            this.Controls.Add(this._description);
            this.Controls.Add(this._date);
            this.Name = "WorkingTimeInput";
            this.Text = "WorkingTimeInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox _description;
        private System.Windows.Forms.DateTimePicker _date;
    }
}