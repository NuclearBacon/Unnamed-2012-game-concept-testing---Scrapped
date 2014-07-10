namespace hashgenerator
{
    partial class frmHashcodeGen
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
            this.txtMD5 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUncoded = new System.Windows.Forms.TextBox();
            this.txtCodedPartial = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCodedFull = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.cmbMD5 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtMD5
            // 
            this.txtMD5.Location = new System.Drawing.Point(268, 40);
            this.txtMD5.Name = "txtMD5";
            this.txtMD5.ReadOnly = true;
            this.txtMD5.Size = new System.Drawing.Size(508, 20);
            this.txtMD5.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "MD5 Key:";
            // 
            // txtUncoded
            // 
            this.txtUncoded.Location = new System.Drawing.Point(114, 66);
            this.txtUncoded.Name = "txtUncoded";
            this.txtUncoded.ReadOnly = true;
            this.txtUncoded.Size = new System.Drawing.Size(662, 20);
            this.txtUncoded.TabIndex = 2;
            // 
            // txtCodedPartial
            // 
            this.txtCodedPartial.Location = new System.Drawing.Point(114, 92);
            this.txtCodedPartial.Name = "txtCodedPartial";
            this.txtCodedPartial.ReadOnly = true;
            this.txtCodedPartial.Size = new System.Drawing.Size(662, 20);
            this.txtCodedPartial.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Uncoded Hash:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Short Coded Hash:";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(11, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "Encode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(114, 14);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(662, 20);
            this.txtPath.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Target EXE:";
            // 
            // txtCodedFull
            // 
            this.txtCodedFull.Location = new System.Drawing.Point(114, 118);
            this.txtCodedFull.Name = "txtCodedFull";
            this.txtCodedFull.ReadOnly = true;
            this.txtCodedFull.Size = new System.Drawing.Size(662, 20);
            this.txtCodedFull.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Full Coded Hash:";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(126, 159);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(58, 13);
            this.lblResults.TabIndex = 11;
            this.lblResults.Text = "[lblResults]";
            // 
            // cmbMD5
            // 
            this.cmbMD5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMD5.FormattingEnabled = true;
            this.cmbMD5.Location = new System.Drawing.Point(114, 39);
            this.cmbMD5.Name = "cmbMD5";
            this.cmbMD5.Size = new System.Drawing.Size(148, 21);
            this.cmbMD5.TabIndex = 12;
            this.cmbMD5.SelectedIndexChanged += new System.EventHandler(this.cmbMD5_SelectedIndexChanged);
            // 
            // frmHashcodeGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 196);
            this.Controls.Add(this.cmbMD5);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCodedFull);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCodedPartial);
            this.Controls.Add(this.txtUncoded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMD5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmHashcodeGen";
            this.Text = "Hashcode Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMD5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUncoded;
        private System.Windows.Forms.TextBox txtCodedPartial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCodedFull;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.ComboBox cmbMD5;
    }
}

