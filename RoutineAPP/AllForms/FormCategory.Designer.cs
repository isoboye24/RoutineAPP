﻿namespace RoutineAPP.AllForms
{
    partial class FormCategory
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.iconMinimize = new FontAwesome.Sharp.IconPictureBox();
            this.iconClose = new FontAwesome.Sharp.IconPictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.iconBtnSave = new FontAwesome.Sharp.IconButton();
            this.iconBtnClose = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Controls.Add(this.iconMinimize);
            this.panel1.Controls.Add(this.iconClose);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(502, 51);
            this.panel1.TabIndex = 46;
            // 
            // iconMinimize
            // 
            this.iconMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconMinimize.BackColor = System.Drawing.Color.Transparent;
            this.iconMinimize.ForeColor = System.Drawing.Color.Gainsboro;
            this.iconMinimize.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            this.iconMinimize.IconColor = System.Drawing.Color.Gainsboro;
            this.iconMinimize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconMinimize.Location = new System.Drawing.Point(416, 11);
            this.iconMinimize.Name = "iconMinimize";
            this.iconMinimize.Size = new System.Drawing.Size(32, 32);
            this.iconMinimize.TabIndex = 16;
            this.iconMinimize.TabStop = false;
            // 
            // iconClose
            // 
            this.iconClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconClose.BackColor = System.Drawing.Color.Transparent;
            this.iconClose.ForeColor = System.Drawing.Color.Gainsboro;
            this.iconClose.IconChar = FontAwesome.Sharp.IconChar.Xmark;
            this.iconClose.IconColor = System.Drawing.Color.Gainsboro;
            this.iconClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconClose.Location = new System.Drawing.Point(454, 11);
            this.iconClose.Name = "iconClose";
            this.iconClose.Size = new System.Drawing.Size(32, 32);
            this.iconClose.TabIndex = 18;
            this.iconClose.TabStop = false;
            this.iconClose.Click += new System.EventHandler(this.iconClose_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.labelTitle.Location = new System.Drawing.Point(8, 11);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(155, 25);
            this.labelTitle.TabIndex = 5;
            this.labelTitle.Text = "ADD CATEGORY";
            // 
            // txtCategory
            // 
            this.txtCategory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCategory.Location = new System.Drawing.Point(121, 111);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(296, 29);
            this.txtCategory.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 44;
            this.label1.Text = "Category";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Maroon;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(507, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 258);
            this.panel3.TabIndex = 49;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Maroon;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 258);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(507, 5);
            this.panel4.TabIndex = 48;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Maroon;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 263);
            this.panel2.TabIndex = 47;
            // 
            // iconBtnSave
            // 
            this.iconBtnSave.FlatAppearance.BorderSize = 0;
            this.iconBtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconBtnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconBtnSave.IconChar = FontAwesome.Sharp.IconChar.Check;
            this.iconBtnSave.IconColor = System.Drawing.Color.DarkGreen;
            this.iconBtnSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBtnSave.IconSize = 32;
            this.iconBtnSave.Location = new System.Drawing.Point(262, 198);
            this.iconBtnSave.Name = "iconBtnSave";
            this.iconBtnSave.Size = new System.Drawing.Size(100, 40);
            this.iconBtnSave.TabIndex = 50;
            this.iconBtnSave.UseVisualStyleBackColor = true;
            this.iconBtnSave.Click += new System.EventHandler(this.iconBtnSave_Click);
            // 
            // iconBtnClose
            // 
            this.iconBtnClose.BackColor = System.Drawing.Color.Transparent;
            this.iconBtnClose.FlatAppearance.BorderSize = 0;
            this.iconBtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconBtnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconBtnClose.IconChar = FontAwesome.Sharp.IconChar.Xmark;
            this.iconBtnClose.IconColor = System.Drawing.Color.Maroon;
            this.iconBtnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBtnClose.IconSize = 32;
            this.iconBtnClose.Location = new System.Drawing.Point(156, 198);
            this.iconBtnClose.Name = "iconBtnClose";
            this.iconBtnClose.Size = new System.Drawing.Size(100, 40);
            this.iconBtnClose.TabIndex = 50;
            this.iconBtnClose.UseVisualStyleBackColor = false;
            this.iconBtnClose.Click += new System.EventHandler(this.iconBtnClose_Click);
            // 
            // FormCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 263);
            this.Controls.Add(this.iconBtnClose);
            this.Controls.Add(this.iconBtnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCategory";
            this.Load += new System.EventHandler(this.FormCategory_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconPictureBox iconMinimize;
        private FontAwesome.Sharp.IconPictureBox iconClose;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private FontAwesome.Sharp.IconButton iconBtnSave;
        private FontAwesome.Sharp.IconButton iconBtnClose;
    }
}