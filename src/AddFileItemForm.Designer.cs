﻿namespace AutomationEngine
{
    partial class AddFileItemForm
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
            this._context = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._value = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Context";
            // 
            // _context
            // 
            this._context.Location = new System.Drawing.Point(65, 10);
            this._context.Name = "_context";
            this._context.Size = new System.Drawing.Size(183, 20);
            this._context.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name";
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(65, 33);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(183, 20);
            this._name.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Value";
            // 
            // _value
            // 
            this._value.Location = new System.Drawing.Point(65, 60);
            this._value.Multiline = true;
            this._value.Name = "_value";
            this._value.Size = new System.Drawing.Size(541, 305);
            this._value.TabIndex = 2;
            // 
            // AddFileItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 384);
            this.Controls.Add(this._value);
            this.Controls.Add(this._name);
            this.Controls.Add(this._context);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddFileItemForm";
            this.Text = "AddFileItemForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _context;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _value;
    }
}