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
            this._date = new System.Windows.Forms.DateTimePicker();
            this._duration = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._description = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._project = new System.Windows.Forms.Label();
            this._task = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _date
            // 
            this._date.Location = new System.Drawing.Point(72, 61);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(132, 20);
            this._date.TabIndex = 0;
            // 
            // _duration
            // 
            this._duration.Location = new System.Drawing.Point(72, 87);
            this._duration.Name = "_duration";
            this._duration.Size = new System.Drawing.Size(55, 20);
            this._duration.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Duration:";
            // 
            // _description
            // 
            this._description.Location = new System.Drawing.Point(16, 143);
            this._description.Multiline = true;
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(408, 71);
            this._description.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Description:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Project:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Task:";
            // 
            // _project
            // 
            this._project.AutoSize = true;
            this._project.Location = new System.Drawing.Point(61, 9);
            this._project.Name = "_project";
            this._project.Size = new System.Drawing.Size(39, 13);
            this._project.TabIndex = 5;
            this._project.Text = "project";
            // 
            // _task
            // 
            this._task.AutoSize = true;
            this._task.Location = new System.Drawing.Point(61, 31);
            this._task.Name = "_task";
            this._task.Size = new System.Drawing.Size(27, 13);
            this._task.TabIndex = 5;
            this._task.Text = "task";
            // 
            // WorkingTimeInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 229);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._task);
            this.Controls.Add(this._project);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._description);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._duration);
            this.Controls.Add(this._date);
            this.Name = "WorkingTimeInput";
            this.Text = "WorkingTimeInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _date;
        private System.Windows.Forms.TextBox _duration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _description;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label _project;
        private System.Windows.Forms.Label _task;
    }
}