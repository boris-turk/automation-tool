namespace BTurk.Automation.WinForms.Controls
{
    partial class MainForm
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
            this._stackLabel = new System.Windows.Forms.Label();
            this._stateLabel = new System.Windows.Forms.Label();
            this._listBox = new System.Windows.Forms.ListBox();
            this._textBox = new System.Windows.Forms.TextBox();
            this._workInProgressPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._workInProgressPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _stackLabel
            // 
            this._stackLabel.AutoSize = true;
            this._stackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._stackLabel.Location = new System.Drawing.Point(12, 9);
            this._stackLabel.Name = "_stackLabel";
            this._stackLabel.Size = new System.Drawing.Size(18, 20);
            this._stackLabel.TabIndex = 2;
            this._stackLabel.Text = ">";
            // 
            // _stateLabel
            // 
            this._stateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._stateLabel.Location = new System.Drawing.Point(387, 14);
            this._stateLabel.Name = "_stateLabel";
            this._stateLabel.Size = new System.Drawing.Size(222, 13);
            this._stateLabel.TabIndex = 4;
            this._stateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._stateLabel.Visible = false;
            // 
            // _listBox
            // 
            this._listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._listBox.FormattingEnabled = true;
            this._listBox.ItemHeight = 20;
            this._listBox.Location = new System.Drawing.Point(12, 77);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(599, 364);
            this._listBox.TabIndex = 1;
            this._listBox.TabStop = false;
            // 
            // _textBox
            // 
            this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._textBox.Location = new System.Drawing.Point(12, 39);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(599, 26);
            this._textBox.TabIndex = 0;
            // 
            // _workInProgressPictureBox
            // 
            this._workInProgressPictureBox.BackColor = System.Drawing.Color.White;
            this._workInProgressPictureBox.Image = global::BTurk.Automation.WinForms.Properties.Resources.loader;
            this._workInProgressPictureBox.Location = new System.Drawing.Point(12, 77);
            this._workInProgressPictureBox.Name = "_workInProgressPictureBox";
            this._workInProgressPictureBox.Size = new System.Drawing.Size(599, 364);
            this._workInProgressPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._workInProgressPictureBox.TabIndex = 3;
            this._workInProgressPictureBox.TabStop = false;
            this._workInProgressPictureBox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 460);
            this.Controls.Add(this._stackLabel);
            this.Controls.Add(this._stateLabel);
            this.Controls.Add(this._listBox);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._workInProgressPictureBox);
            this.Name = "MainForm";
            this.Text = "Automation engine";
            ((System.ComponentModel.ISupportInitialize)(this._workInProgressPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBox;
        private System.Windows.Forms.ListBox _listBox;
        private System.Windows.Forms.Label _stackLabel;
        private System.Windows.Forms.PictureBox _workInProgressPictureBox;
        private System.Windows.Forms.Label _stateLabel;
    }
}

