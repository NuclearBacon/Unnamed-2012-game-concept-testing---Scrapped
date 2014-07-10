namespace hashgenerator
{
    partial class frmMain
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
            this.btnHasher = new System.Windows.Forms.Button();
            this.btnEncryptor = new System.Windows.Forms.Button();
            this.btnConfigEnc = new System.Windows.Forms.Button();
            this.btnUIE = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHasher
            // 
            this.btnHasher.Location = new System.Drawing.Point(12, 12);
            this.btnHasher.Name = "btnHasher";
            this.btnHasher.Size = new System.Drawing.Size(191, 41);
            this.btnHasher.TabIndex = 0;
            this.btnHasher.Text = "Hash Generator";
            this.btnHasher.UseVisualStyleBackColor = true;
            this.btnHasher.Click += new System.EventHandler(this.btnHasher_Click);
            // 
            // btnEncryptor
            // 
            this.btnEncryptor.Location = new System.Drawing.Point(12, 59);
            this.btnEncryptor.Name = "btnEncryptor";
            this.btnEncryptor.Size = new System.Drawing.Size(191, 41);
            this.btnEncryptor.TabIndex = 1;
            this.btnEncryptor.Text = "strCore Generator";
            this.btnEncryptor.UseVisualStyleBackColor = true;
            this.btnEncryptor.Click += new System.EventHandler(this.btnEncryptor_Click);
            // 
            // btnConfigEnc
            // 
            this.btnConfigEnc.Location = new System.Drawing.Point(12, 242);
            this.btnConfigEnc.Name = "btnConfigEnc";
            this.btnConfigEnc.Size = new System.Drawing.Size(191, 41);
            this.btnConfigEnc.TabIndex = 2;
            this.btnConfigEnc.Text = "Configure Encryption Strings";
            this.btnConfigEnc.UseVisualStyleBackColor = true;
            this.btnConfigEnc.Click += new System.EventHandler(this.btnConfigEnc_Click);
            // 
            // btnUIE
            // 
            this.btnUIE.Location = new System.Drawing.Point(12, 106);
            this.btnUIE.Name = "btnUIE";
            this.btnUIE.Size = new System.Drawing.Size(191, 41);
            this.btnUIE.TabIndex = 3;
            this.btnUIE.Text = "UI Element Editor";
            this.btnUIE.UseVisualStyleBackColor = true;
            this.btnUIE.Click += new System.EventHandler(this.btnUIE_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "Data File Builder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 295);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnUIE);
            this.Controls.Add(this.btnConfigEnc);
            this.Controls.Add(this.btnEncryptor);
            this.Controls.Add(this.btnHasher);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "DevTool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHasher;
        private System.Windows.Forms.Button btnEncryptor;
        private System.Windows.Forms.Button btnConfigEnc;
        private System.Windows.Forms.Button btnUIE;
        private System.Windows.Forms.Button button1;
    }
}