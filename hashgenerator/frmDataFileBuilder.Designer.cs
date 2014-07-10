namespace hashgenerator
{
    partial class frmDataFileBuilder
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("NewDataFile.jda");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataFileBuilder));
            this.cmsTVRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsRoot_NewSubNode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRoot_DeleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRoot_NewElement = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRoot_RemoveElement = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRoot_FileProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRoot_RenameItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnConfigPath = new System.Windows.Forms.Button();
            this.btnCompilePath = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.txtEncKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbEnc = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCompilePath = new System.Windows.Forms.TextBox();
            this.txtConfigPath = new System.Windows.Forms.TextBox();
            this.txtArchiveName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.nudMajor = new System.Windows.Forms.NumericUpDown();
            this.nudMinor = new System.Windows.Forms.NumericUpDown();
            this.nudBuild = new System.Windows.Forms.NumericUpDown();
            this.nudRevision = new System.Windows.Forms.NumericUpDown();
            this.cmsTVRoot.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRevision)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsTVRoot
            // 
            this.cmsTVRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRoot_NewSubNode,
            this.tsRoot_DeleteNode,
            this.tsRoot_NewElement,
            this.tsRoot_RemoveElement,
            this.tsRoot_FileProperties,
            this.tsRoot_RenameItem});
            this.cmsTVRoot.Name = "cmsTV";
            this.cmsTVRoot.Size = new System.Drawing.Size(216, 136);
            this.cmsTVRoot.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTVRoot_Opening);
            // 
            // tsRoot_NewSubNode
            // 
            this.tsRoot_NewSubNode.Name = "tsRoot_NewSubNode";
            this.tsRoot_NewSubNode.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_NewSubNode.Text = "New Subnode...";
            this.tsRoot_NewSubNode.Click += new System.EventHandler(this.tsRoot_NewSubNode_Click);
            // 
            // tsRoot_DeleteNode
            // 
            this.tsRoot_DeleteNode.Name = "tsRoot_DeleteNode";
            this.tsRoot_DeleteNode.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_DeleteNode.Text = "Delete Node";
            this.tsRoot_DeleteNode.Click += new System.EventHandler(this.tsRoot_DeleteNode_Click);
            // 
            // tsRoot_NewElement
            // 
            this.tsRoot_NewElement.Name = "tsRoot_NewElement";
            this.tsRoot_NewElement.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_NewElement.Text = "Attach Resource Element...";
            this.tsRoot_NewElement.Click += new System.EventHandler(this.tsRoot_NewElement_Click);
            // 
            // tsRoot_RemoveElement
            // 
            this.tsRoot_RemoveElement.Name = "tsRoot_RemoveElement";
            this.tsRoot_RemoveElement.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_RemoveElement.Text = "Remove Element";
            this.tsRoot_RemoveElement.Click += new System.EventHandler(this.tsRoot_RemoveElement_Click);
            // 
            // tsRoot_FileProperties
            // 
            this.tsRoot_FileProperties.Name = "tsRoot_FileProperties";
            this.tsRoot_FileProperties.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_FileProperties.Text = "File Properties...";
            this.tsRoot_FileProperties.Click += new System.EventHandler(this.tsRoof_FileProperties_Click);
            // 
            // tsRoot_RenameItem
            // 
            this.tsRoot_RenameItem.Name = "tsRoot_RenameItem";
            this.tsRoot_RenameItem.Size = new System.Drawing.Size(215, 22);
            this.tsRoot_RenameItem.Text = "Rename Item...";
            this.tsRoot_RenameItem.Click += new System.EventHandler(this.tsRoot_RenameItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(290, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(909, 207);
            this.panel1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudRevision);
            this.groupBox2.Controls.Add(this.nudBuild);
            this.groupBox2.Controls.Add(this.nudMinor);
            this.groupBox2.Controls.Add(this.nudMajor);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnConfigPath);
            this.groupBox2.Controls.Add(this.btnCompilePath);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Controls.Add(this.btnCompile);
            this.groupBox2.Controls.Add(this.btnSaveConfig);
            this.groupBox2.Controls.Add(this.btnLoadConfig);
            this.groupBox2.Controls.Add(this.txtEncKey);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbEnc);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtCompilePath);
            this.groupBox2.Controls.Add(this.txtConfigPath);
            this.groupBox2.Controls.Add(this.txtArchiveName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(5, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(780, 200);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Important Stuff";
            // 
            // btnConfigPath
            // 
            this.btnConfigPath.Location = new System.Drawing.Point(118, 96);
            this.btnConfigPath.Name = "btnConfigPath";
            this.btnConfigPath.Size = new System.Drawing.Size(24, 20);
            this.btnConfigPath.TabIndex = 20;
            this.btnConfigPath.Text = "...";
            this.btnConfigPath.UseVisualStyleBackColor = true;
            this.btnConfigPath.Click += new System.EventHandler(this.btnConfigPath_Click);
            // 
            // btnCompilePath
            // 
            this.btnCompilePath.Location = new System.Drawing.Point(118, 119);
            this.btnCompilePath.Name = "btnCompilePath";
            this.btnCompilePath.Size = new System.Drawing.Size(24, 20);
            this.btnCompilePath.TabIndex = 19;
            this.btnCompilePath.Text = "...";
            this.btnCompilePath.UseVisualStyleBackColor = true;
            this.btnCompilePath.Click += new System.EventHandler(this.btnCompilePath_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(406, 19);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(126, 31);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Import From Data File";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(274, 19);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(126, 31);
            this.btnCompile.TabIndex = 17;
            this.btnCompile.Text = "Compile Data File";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(142, 19);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(126, 31);
            this.btnSaveConfig.TabIndex = 16;
            this.btnSaveConfig.Text = "Save Config File";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(10, 19);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(126, 31);
            this.btnLoadConfig.TabIndex = 15;
            this.btnLoadConfig.Text = "Load Config File";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // txtEncKey
            // 
            this.txtEncKey.Location = new System.Drawing.Point(237, 146);
            this.txtEncKey.Name = "txtEncKey";
            this.txtEncKey.ReadOnly = true;
            this.txtEncKey.Size = new System.Drawing.Size(531, 20);
            this.txtEncKey.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Encryption Combo:";
            // 
            // cmbEnc
            // 
            this.cmbEnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnc.FormattingEnabled = true;
            this.cmbEnc.Location = new System.Drawing.Point(118, 145);
            this.cmbEnc.Name = "cmbEnc";
            this.cmbEnc.Size = new System.Drawing.Size(113, 21);
            this.cmbEnc.TabIndex = 12;
            this.cmbEnc.SelectedIndexChanged += new System.EventHandler(this.cmbEnc_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Compile Path:";
            // 
            // txtCompilePath
            // 
            this.txtCompilePath.Location = new System.Drawing.Point(148, 119);
            this.txtCompilePath.Name = "txtCompilePath";
            this.txtCompilePath.Size = new System.Drawing.Size(620, 20);
            this.txtCompilePath.TabIndex = 10;
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Location = new System.Drawing.Point(148, 93);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.Size = new System.Drawing.Size(620, 20);
            this.txtConfigPath.TabIndex = 9;
            // 
            // txtArchiveName
            // 
            this.txtArchiveName.Location = new System.Drawing.Point(118, 67);
            this.txtArchiveName.Name = "txtArchiveName";
            this.txtArchiveName.Size = new System.Drawing.Size(650, 20);
            this.txtArchiveName.TabIndex = 8;
            this.txtArchiveName.Text = "NewDataFile";
            this.txtArchiveName.TextChanged += new System.EventHandler(this.txtArchiveName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Config Save Path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Archive Name:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(290, 207);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(909, 417);
            this.panel2.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ContextMenuStrip = this.cmsTVRoot;
            treeNode1.Name = "Node0";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.Tag = "0";
            treeNode1.Text = "NewDataFile.jda";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(290, 624);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "archive");
            this.imageList1.Images.SetKeyName(1, "folder");
            this.imageList1.Images.SetKeyName(2, "binaryfile");
            this.imageList1.Images.SetKeyName(3, "imagefile");
            this.imageList1.Images.SetKeyName(4, "textfile");
            this.imageList1.Images.SetKeyName(5, "unknown");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Version Info:";
            // 
            // nudMajor
            // 
            this.nudMajor.Location = new System.Drawing.Point(118, 173);
            this.nudMajor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMajor.Name = "nudMajor";
            this.nudMajor.Size = new System.Drawing.Size(52, 20);
            this.nudMajor.TabIndex = 23;
            this.nudMajor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudMinor
            // 
            this.nudMinor.Location = new System.Drawing.Point(176, 173);
            this.nudMinor.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMinor.Name = "nudMinor";
            this.nudMinor.Size = new System.Drawing.Size(52, 20);
            this.nudMinor.TabIndex = 24;
            this.nudMinor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudBuild
            // 
            this.nudBuild.Location = new System.Drawing.Point(234, 173);
            this.nudBuild.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBuild.Name = "nudBuild";
            this.nudBuild.Size = new System.Drawing.Size(52, 20);
            this.nudBuild.TabIndex = 25;
            this.nudBuild.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudRevision
            // 
            this.nudRevision.Location = new System.Drawing.Point(292, 173);
            this.nudRevision.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudRevision.Name = "nudRevision";
            this.nudRevision.Size = new System.Drawing.Size(52, 20);
            this.nudRevision.TabIndex = 26;
            this.nudRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmDataFileBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 624);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.Name = "frmDataFileBuilder";
            this.Text = "Data File Builder";
            this.cmsTVRoot.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMajor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBuild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRevision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCompilePath;
        private System.Windows.Forms.TextBox txtConfigPath;
        private System.Windows.Forms.TextBox txtArchiveName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.TextBox txtEncKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbEnc;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnConfigPath;
        private System.Windows.Forms.Button btnCompilePath;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip cmsTVRoot;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_NewSubNode;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_DeleteNode;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_NewElement;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_RemoveElement;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_FileProperties;
        private System.Windows.Forms.ToolStripMenuItem tsRoot_RenameItem;
        private System.Windows.Forms.NumericUpDown nudRevision;
        private System.Windows.Forms.NumericUpDown nudBuild;
        private System.Windows.Forms.NumericUpDown nudMinor;
        private System.Windows.Forms.NumericUpDown nudMajor;
        private System.Windows.Forms.Label label5;
    }
}