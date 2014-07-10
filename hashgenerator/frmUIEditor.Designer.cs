namespace hashgenerator
{
    partial class frmUIEditor
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
            this.pnControls = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvText = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtBoundsHeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBoundsWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveVert = new System.Windows.Forms.Button();
            this.btnVertDown = new System.Windows.Forms.Button();
            this.btnVertUp = new System.Windows.Forms.Button();
            this.btnNewVert = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvVertex = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnColorSave = new System.Windows.Forms.Button();
            this.btnPreviewBackgroundColor = new System.Windows.Forms.Button();
            this.pbColorPreview = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtColorA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtColorB = new System.Windows.Forms.TextBox();
            this.txtColorG = new System.Windows.Forms.TextBox();
            this.txtColorR = new System.Windows.Forms.TextBox();
            this.cmdColorAdd = new System.Windows.Forms.Button();
            this.btnColorRemove = new System.Windows.Forms.Button();
            this.lbColors = new System.Windows.Forms.ListBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdOpenFile = new System.Windows.Forms.Button();
            this.cdPicker1 = new System.Windows.Forms.ColorDialog();
            this.pnRenderer = new System.Windows.Forms.Panel();
            this.lblCheat = new System.Windows.Forms.Label();
            this.btnTxtDelete = new System.Windows.Forms.Button();
            this.btnTxtDown = new System.Windows.Forms.Button();
            this.btnTxtUp = new System.Windows.Forms.Button();
            this.btnNewTxt = new System.Windows.Forms.Button();
            this.pnControls.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvText)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVertex)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColorPreview)).BeginInit();
            this.pnRenderer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnControls
            // 
            this.pnControls.AutoScroll = true;
            this.pnControls.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnControls.Controls.Add(this.groupBox4);
            this.pnControls.Controls.Add(this.groupBox3);
            this.pnControls.Controls.Add(this.groupBox2);
            this.pnControls.Controls.Add(this.groupBox1);
            this.pnControls.Controls.Add(this.cmdSave);
            this.pnControls.Controls.Add(this.cmdOpenFile);
            this.pnControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnControls.Location = new System.Drawing.Point(0, 468);
            this.pnControls.Name = "pnControls";
            this.pnControls.Size = new System.Drawing.Size(1192, 150);
            this.pnControls.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnTxtDelete);
            this.groupBox4.Controls.Add(this.btnTxtDown);
            this.groupBox4.Controls.Add(this.btnTxtUp);
            this.groupBox4.Controls.Add(this.btnNewTxt);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.dgvText);
            this.groupBox4.Location = new System.Drawing.Point(541, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(607, 137);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Text Elements";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(291, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Index      X           Y          Color      Font      Tag              Text";
            // 
            // dgvText
            // 
            this.dgvText.AllowUserToAddRows = false;
            this.dgvText.AllowUserToDeleteRows = false;
            this.dgvText.AllowUserToResizeColumns = false;
            this.dgvText.AllowUserToResizeRows = false;
            this.dgvText.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvText.ColumnHeadersVisible = false;
            this.dgvText.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.Column6,
            this.Column7,
            this.dataGridViewTextBoxColumn4,
            this.Column5});
            this.dgvText.Location = new System.Drawing.Point(6, 33);
            this.dgvText.MultiSelect = false;
            this.dgvText.Name = "dgvText";
            this.dgvText.RowHeadersVisible = false;
            this.dgvText.Size = new System.Drawing.Size(561, 91);
            this.dgvText.TabIndex = 1;
            this.dgvText.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvText_CellValueChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "X";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 40;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Y";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 40;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Color Index";
            this.Column6.Name = "Column6";
            this.Column6.Width = 40;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Font Code";
            this.Column7.Name = "Column7";
            this.Column7.Width = 40;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Tag";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Text";
            this.Column5.Name = "Column5";
            this.Column5.Width = 1000;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtBoundsHeight);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtBoundsWidth);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(3, 79);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(113, 60);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bounds";
            // 
            // txtBoundsHeight
            // 
            this.txtBoundsHeight.Location = new System.Drawing.Point(51, 35);
            this.txtBoundsHeight.Name = "txtBoundsHeight";
            this.txtBoundsHeight.Size = new System.Drawing.Size(56, 20);
            this.txtBoundsHeight.TabIndex = 3;
            this.txtBoundsHeight.TextChanged += new System.EventHandler(this.txtBoundsHeight_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Height:";
            // 
            // txtBoundsWidth
            // 
            this.txtBoundsWidth.Location = new System.Drawing.Point(51, 13);
            this.txtBoundsWidth.Name = "txtBoundsWidth";
            this.txtBoundsWidth.Size = new System.Drawing.Size(56, 20);
            this.txtBoundsWidth.TabIndex = 1;
            this.txtBoundsWidth.TextChanged += new System.EventHandler(this.txtBoundsWidth_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Width:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemoveVert);
            this.groupBox2.Controls.Add(this.btnVertDown);
            this.groupBox2.Controls.Add(this.btnVertUp);
            this.groupBox2.Controls.Add(this.btnNewVert);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dgvVertex);
            this.groupBox2.Location = new System.Drawing.Point(310, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 136);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vertex Control";
            // 
            // btnRemoveVert
            // 
            this.btnRemoveVert.Location = new System.Drawing.Point(192, 101);
            this.btnRemoveVert.Name = "btnRemoveVert";
            this.btnRemoveVert.Size = new System.Drawing.Size(26, 22);
            this.btnRemoveVert.TabIndex = 5;
            this.btnRemoveVert.Text = "-";
            this.btnRemoveVert.UseVisualStyleBackColor = true;
            this.btnRemoveVert.Click += new System.EventHandler(this.btnRemoveVert_Click);
            // 
            // btnVertDown
            // 
            this.btnVertDown.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVertDown.Location = new System.Drawing.Point(192, 78);
            this.btnVertDown.Name = "btnVertDown";
            this.btnVertDown.Size = new System.Drawing.Size(26, 22);
            this.btnVertDown.TabIndex = 4;
            this.btnVertDown.Text = "↓";
            this.btnVertDown.UseVisualStyleBackColor = true;
            this.btnVertDown.Click += new System.EventHandler(this.btnVertDown_Click);
            // 
            // btnVertUp
            // 
            this.btnVertUp.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVertUp.Location = new System.Drawing.Point(192, 55);
            this.btnVertUp.Name = "btnVertUp";
            this.btnVertUp.Size = new System.Drawing.Size(26, 22);
            this.btnVertUp.TabIndex = 3;
            this.btnVertUp.Text = "↑";
            this.btnVertUp.UseVisualStyleBackColor = true;
            this.btnVertUp.Click += new System.EventHandler(this.btnVertUp_Click);
            // 
            // btnNewVert
            // 
            this.btnNewVert.Location = new System.Drawing.Point(192, 32);
            this.btnNewVert.Name = "btnNewVert";
            this.btnNewVert.Size = new System.Drawing.Size(26, 22);
            this.btnNewVert.TabIndex = 2;
            this.btnNewVert.Text = "+";
            this.btnNewVert.UseVisualStyleBackColor = true;
            this.btnNewVert.Click += new System.EventHandler(this.btnNewVert_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Index      X           Y           Color";
            // 
            // dgvVertex
            // 
            this.dgvVertex.AllowUserToAddRows = false;
            this.dgvVertex.AllowUserToDeleteRows = false;
            this.dgvVertex.AllowUserToResizeColumns = false;
            this.dgvVertex.AllowUserToResizeRows = false;
            this.dgvVertex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVertex.ColumnHeadersVisible = false;
            this.dgvVertex.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvVertex.Location = new System.Drawing.Point(6, 32);
            this.dgvVertex.MultiSelect = false;
            this.dgvVertex.Name = "dgvVertex";
            this.dgvVertex.RowHeadersVisible = false;
            this.dgvVertex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvVertex.Size = new System.Drawing.Size(180, 91);
            this.dgvVertex.TabIndex = 0;
            this.dgvVertex.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVertex_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Index";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "X";
            this.Column2.Name = "Column2";
            this.Column2.Width = 40;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Y";
            this.Column3.Name = "Column3";
            this.Column3.Width = 40;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Color";
            this.Column4.Name = "Column4";
            this.Column4.Width = 40;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnColorSave);
            this.groupBox1.Controls.Add(this.btnPreviewBackgroundColor);
            this.groupBox1.Controls.Add(this.pbColorPreview);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtColorA);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtColorB);
            this.groupBox1.Controls.Add(this.txtColorG);
            this.groupBox1.Controls.Add(this.txtColorR);
            this.groupBox1.Controls.Add(this.cmdColorAdd);
            this.groupBox1.Controls.Add(this.btnColorRemove);
            this.groupBox1.Controls.Add(this.lbColors);
            this.groupBox1.Location = new System.Drawing.Point(122, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 136);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color Palette";
            // 
            // btnColorSave
            // 
            this.btnColorSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColorSave.Location = new System.Drawing.Point(56, 84);
            this.btnColorSave.Name = "btnColorSave";
            this.btnColorSave.Size = new System.Drawing.Size(42, 20);
            this.btnColorSave.TabIndex = 13;
            this.btnColorSave.Text = "Save";
            this.btnColorSave.UseVisualStyleBackColor = true;
            this.btnColorSave.Click += new System.EventHandler(this.btnColorSave_Click);
            // 
            // btnPreviewBackgroundColor
            // 
            this.btnPreviewBackgroundColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreviewBackgroundColor.Location = new System.Drawing.Point(56, 106);
            this.btnPreviewBackgroundColor.Name = "btnPreviewBackgroundColor";
            this.btnPreviewBackgroundColor.Size = new System.Drawing.Size(42, 20);
            this.btnPreviewBackgroundColor.TabIndex = 12;
            this.btnPreviewBackgroundColor.Text = "Bkgrnd";
            this.btnPreviewBackgroundColor.UseVisualStyleBackColor = true;
            this.btnPreviewBackgroundColor.Click += new System.EventHandler(this.btnPreviewBackgroundColor_Click);
            // 
            // pbColorPreview
            // 
            this.pbColorPreview.BackColor = System.Drawing.Color.Black;
            this.pbColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbColorPreview.Location = new System.Drawing.Point(102, 55);
            this.pbColorPreview.Name = "pbColorPreview";
            this.pbColorPreview.Size = new System.Drawing.Size(72, 72);
            this.pbColorPreview.TabIndex = 11;
            this.pbColorPreview.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "A";
            // 
            // txtColorA
            // 
            this.txtColorA.Location = new System.Drawing.Point(68, 55);
            this.txtColorA.Name = "txtColorA";
            this.txtColorA.Size = new System.Drawing.Size(28, 20);
            this.txtColorA.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "B";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "G";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "R";
            // 
            // txtColorB
            // 
            this.txtColorB.Location = new System.Drawing.Point(21, 107);
            this.txtColorB.Name = "txtColorB";
            this.txtColorB.Size = new System.Drawing.Size(28, 20);
            this.txtColorB.TabIndex = 5;
            // 
            // txtColorG
            // 
            this.txtColorG.Location = new System.Drawing.Point(21, 81);
            this.txtColorG.Name = "txtColorG";
            this.txtColorG.Size = new System.Drawing.Size(28, 20);
            this.txtColorG.TabIndex = 4;
            // 
            // txtColorR
            // 
            this.txtColorR.Location = new System.Drawing.Point(21, 55);
            this.txtColorR.Name = "txtColorR";
            this.txtColorR.Size = new System.Drawing.Size(28, 20);
            this.txtColorR.TabIndex = 3;
            // 
            // cmdColorAdd
            // 
            this.cmdColorAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdColorAdd.Location = new System.Drawing.Point(128, 34);
            this.cmdColorAdd.Name = "cmdColorAdd";
            this.cmdColorAdd.Size = new System.Drawing.Size(46, 16);
            this.cmdColorAdd.TabIndex = 2;
            this.cmdColorAdd.Text = "Create";
            this.cmdColorAdd.UseVisualStyleBackColor = true;
            this.cmdColorAdd.Click += new System.EventHandler(this.cmdColorAdd_Click);
            // 
            // btnColorRemove
            // 
            this.btnColorRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColorRemove.Location = new System.Drawing.Point(128, 18);
            this.btnColorRemove.Name = "btnColorRemove";
            this.btnColorRemove.Size = new System.Drawing.Size(46, 16);
            this.btnColorRemove.TabIndex = 1;
            this.btnColorRemove.Text = "Delete";
            this.btnColorRemove.UseVisualStyleBackColor = true;
            this.btnColorRemove.Click += new System.EventHandler(this.btnColorRemove_Click);
            // 
            // lbColors
            // 
            this.lbColors.FormattingEnabled = true;
            this.lbColors.Location = new System.Drawing.Point(6, 19);
            this.lbColors.Name = "lbColors";
            this.lbColors.Size = new System.Drawing.Size(116, 30);
            this.lbColors.TabIndex = 0;
            this.lbColors.SelectedIndexChanged += new System.EventHandler(this.lbColors_SelectedIndexChanged);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(3, 41);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(113, 32);
            this.cmdSave.TabIndex = 1;
            this.cmdSave.Text = "Save As";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdOpenFile
            // 
            this.cmdOpenFile.Location = new System.Drawing.Point(3, 3);
            this.cmdOpenFile.Name = "cmdOpenFile";
            this.cmdOpenFile.Size = new System.Drawing.Size(113, 32);
            this.cmdOpenFile.TabIndex = 0;
            this.cmdOpenFile.Text = "Open File";
            this.cmdOpenFile.UseVisualStyleBackColor = true;
            this.cmdOpenFile.Click += new System.EventHandler(this.cmdOpenFile_Click);
            // 
            // cdPicker1
            // 
            this.cdPicker1.AnyColor = true;
            // 
            // pnRenderer
            // 
            this.pnRenderer.AutoScroll = true;
            this.pnRenderer.BackgroundImage = global::hashgenerator.Properties.Resources.bmpBackground;
            this.pnRenderer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnRenderer.Controls.Add(this.lblCheat);
            this.pnRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnRenderer.Location = new System.Drawing.Point(0, 0);
            this.pnRenderer.Name = "pnRenderer";
            this.pnRenderer.Size = new System.Drawing.Size(1192, 468);
            this.pnRenderer.TabIndex = 1;
            this.pnRenderer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.pnRenderer_Scroll);
            // 
            // lblCheat
            // 
            this.lblCheat.Location = new System.Drawing.Point(30, 26);
            this.lblCheat.Name = "lblCheat";
            this.lblCheat.Size = new System.Drawing.Size(50, 50);
            this.lblCheat.TabIndex = 0;
            this.lblCheat.Text = "lblCheat";
            // 
            // btnTxtDelete
            // 
            this.btnTxtDelete.Location = new System.Drawing.Point(573, 102);
            this.btnTxtDelete.Name = "btnTxtDelete";
            this.btnTxtDelete.Size = new System.Drawing.Size(26, 22);
            this.btnTxtDelete.TabIndex = 9;
            this.btnTxtDelete.Text = "-";
            this.btnTxtDelete.UseVisualStyleBackColor = true;
            this.btnTxtDelete.Click += new System.EventHandler(this.btnTxtDelete_Click);
            // 
            // btnTxtDown
            // 
            this.btnTxtDown.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTxtDown.Location = new System.Drawing.Point(573, 79);
            this.btnTxtDown.Name = "btnTxtDown";
            this.btnTxtDown.Size = new System.Drawing.Size(26, 22);
            this.btnTxtDown.TabIndex = 8;
            this.btnTxtDown.Text = "↓";
            this.btnTxtDown.UseVisualStyleBackColor = true;
            this.btnTxtDown.Click += new System.EventHandler(this.btnTxtDown_Click);
            // 
            // btnTxtUp
            // 
            this.btnTxtUp.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTxtUp.Location = new System.Drawing.Point(573, 56);
            this.btnTxtUp.Name = "btnTxtUp";
            this.btnTxtUp.Size = new System.Drawing.Size(26, 22);
            this.btnTxtUp.TabIndex = 7;
            this.btnTxtUp.Text = "↑";
            this.btnTxtUp.UseVisualStyleBackColor = true;
            this.btnTxtUp.Click += new System.EventHandler(this.btnTxtUp_Click);
            // 
            // btnNewTxt
            // 
            this.btnNewTxt.Location = new System.Drawing.Point(573, 33);
            this.btnNewTxt.Name = "btnNewTxt";
            this.btnNewTxt.Size = new System.Drawing.Size(26, 22);
            this.btnNewTxt.TabIndex = 6;
            this.btnNewTxt.Text = "+";
            this.btnNewTxt.UseVisualStyleBackColor = true;
            this.btnNewTxt.Click += new System.EventHandler(this.btnNewTxt_Click);
            // 
            // frmUIEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 618);
            this.Controls.Add(this.pnRenderer);
            this.Controls.Add(this.pnControls);
            this.Name = "frmUIEditor";
            this.Text = "UI Element Editor";
            this.pnControls.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvText)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVertex)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColorPreview)).EndInit();
            this.pnRenderer.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Panel pnControls;
        private System.Windows.Forms.Panel pnRenderer;
        private System.Windows.Forms.Button cmdOpenFile;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdColorAdd;
        private System.Windows.Forms.Button btnColorRemove;
        private System.Windows.Forms.ListBox lbColors;
        private System.Windows.Forms.ColorDialog cdPicker1;
        private System.Windows.Forms.Button btnPreviewBackgroundColor;
        private System.Windows.Forms.PictureBox pbColorPreview;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtColorA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtColorB;
        private System.Windows.Forms.TextBox txtColorG;
        private System.Windows.Forms.TextBox txtColorR;
        private System.Windows.Forms.Button btnColorSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvVertex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtBoundsHeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBoundsWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCheat;
        private System.Windows.Forms.Button btnRemoveVert;
        private System.Windows.Forms.Button btnVertDown;
        private System.Windows.Forms.Button btnVertUp;
        private System.Windows.Forms.Button btnNewVert;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvText;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button btnTxtDelete;
        private System.Windows.Forms.Button btnTxtDown;
        private System.Windows.Forms.Button btnTxtUp;
        private System.Windows.Forms.Button btnNewTxt;




    }
}