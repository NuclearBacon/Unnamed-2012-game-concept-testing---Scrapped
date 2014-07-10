using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace hashgenerator
{
    public partial class frmUIEditor : Form
    {
        private Rectangle rectBounds;
        
        private int[] iColor_A;
        private int[] iColor_R;
        private int[] iColor_G;
        private int[] iColor_B;

        private Point[] ptVertexCoords;
        private int[] iVertexColorCode;

        private Point[] ptTextCoords;
        private int[] iTextColorCode;
        private int[] iTextFontCode;
        private string[] strTextTag;
        private string[] strTextValue;

        Pen pn = null;
        SolidBrush sb = null;
        Graphics gfx = null;

        bool bVertexListUpdate = false;
        bool bTextListUpdate = false;

        Bitmap bmpDB;
        Graphics gfxDB;
        
        public frmUIEditor()
        {
            InitializeComponent();
            lblCheat.Text = "";
            lblCheat.Width = 0;
            lblCheat.Height = 0;
        }

        private void ResetData()
        {
            iColor_A = null;
            iColor_R = null;
            iColor_G = null;
            iColor_B = null;

            ptVertexCoords = null;
            iVertexColorCode = null;
            
            ptTextCoords = null;
            iTextColorCode = null;
            iTextFontCode = null;
            strTextTag = null;
            strTextValue = null;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.AddExtension = true;
            sfd.DefaultExt = "uie";
            sfd.Filter = "UI Element File (*.uie)|*.uie";
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();
            
            string sOutPath = sfd.FileName;
            sfd.Dispose();

            if (sOutPath != "")
            {
                string sOutData = "";
                char dl = (char)0;

                // append rectangle data
                sOutData += rectBounds.Width.ToString() + dl + rectBounds.Height.ToString() + dl;
                
                // append color information
                int iNumColors = iColor_A.GetUpperBound(0) + 1;
                sOutData += iNumColors.ToString() + dl;
                for (int a = 0; a < iNumColors; a++)
                    sOutData += iColor_A[a].ToString() + dl + iColor_R[a].ToString() + dl + iColor_G[a].ToString() + dl + iColor_B[a].ToString() + dl;
                
                // append vertex information
                int iNumVerts = ptVertexCoords.GetUpperBound(0) + 1;
                sOutData += iNumVerts.ToString() + dl;
                for (int a = 0; a < iNumVerts; a++)
                    sOutData += ptVertexCoords[a].X.ToString() + dl + ptVertexCoords[a].Y.ToString() + dl + iVertexColorCode[a].ToString() + dl;

                // append text information
                int iNumText = ptTextCoords.GetUpperBound(0) + 1;
                sOutData += iNumText.ToString();
                for (int a = 0; a < iNumText; a++)
                    sOutData += dl + ptTextCoords[a].X.ToString() + dl + ptTextCoords[a].Y.ToString() + dl + iTextColorCode[a].ToString() + dl +
                        iTextFontCode[a].ToString() + dl + strTextTag[a] + dl + strTextValue[a];

                try
                {
                    FileStream fs = new FileStream(sOutPath, FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(sOutData);
                    sw.Close();
                    fs.Close();
                    sw.Dispose();
                    fs.Dispose();

                }
                catch { }
            }
        }

        private void cmdOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Multiselect = false;
            ofd.Filter = "UI Element File (*.uie)|*.uie";
            ofd.ShowDialog();

            //string temppath = @"C:\Users\Joe\Documents\Visual Studio 2010\Projects\dasspiel\dasspiel\bin\Debug\ui\options.uie";
            string temppath = ofd.FileName;

            string[] sInData = null;
            try
            {
                FileStream fs = new FileStream(temppath/*ofd.FileName*/, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                sInData = sr.ReadToEnd().Split((char)0);
                sr.Close();
                fs.Close();
                sr.Dispose();
                fs.Dispose();

                ResetData();
            }
            catch{}
            ofd.Dispose();

            // load rectangle data
            rectBounds.Width = Convert.ToInt32(sInData[0]);
            rectBounds.Height = Convert.ToInt32(sInData[1]);
            txtBoundsWidth.Text = sInData[0];
            txtBoundsHeight.Text = sInData[1];

            // load color data
            lbColors.Items.Clear();
            int iNumColors = Convert.ToInt32(sInData[2]);
            iColor_A = new int[iNumColors];
            iColor_R = new int[iNumColors];
            iColor_G = new int[iNumColors];
            iColor_B = new int[iNumColors];
            for (int c = 0; c < iNumColors; c++)
            {
                iColor_A[c] = Convert.ToInt32(sInData[3 + (c * 4)]);
                iColor_R[c] = Convert.ToInt32(sInData[4 + (c * 4)]);
                iColor_G[c] = Convert.ToInt32(sInData[5 + (c * 4)]);
                iColor_B[c] = Convert.ToInt32(sInData[6 + (c * 4)]);

                lbColors.Items.Add(string.Format("Color {0}", c.ToString()));
            }

            // load vertex data
            int iVDatOffset = 3 + (4 * iNumColors);
            int iNumVerts = Convert.ToInt32(sInData[iVDatOffset]);
            if (iNumVerts > 0)
            {
                bVertexListUpdate = true;
                ptVertexCoords = new Point[iNumVerts];
                iVertexColorCode = new int[iNumVerts];
                dgvVertex.Rows.Clear();
                dgvVertex.Rows.Add(iNumVerts);
                for (int v = 0; v < iNumVerts; v++)
                {
                    ptVertexCoords[v].X = Convert.ToInt32(sInData[iVDatOffset + 1 + (v * 3)]);
                    ptVertexCoords[v].Y = Convert.ToInt32(sInData[iVDatOffset + 2 + (v * 3)]);
                    iVertexColorCode[v] = Convert.ToInt32(sInData[iVDatOffset + 3 + (v * 3)]);
                    
                    dgvVertex.Rows[v].Cells[0].Value = v;
                    dgvVertex.Rows[v].Cells[1].Value = ptVertexCoords[v].X;
                    dgvVertex.Rows[v].Cells[2].Value = ptVertexCoords[v].Y;
                    dgvVertex.Rows[v].Cells[3].Value = iVertexColorCode[v];
                }
                bVertexListUpdate = false;
            }

            // load text object data
            int iTxtOffset = iVDatOffset + (3 * iNumVerts) + 1;
            int iNumTxt = Convert.ToInt32(sInData[iTxtOffset]);
            bTextListUpdate = true;
            dgvText.Rows.Clear();
            if (iNumTxt > 0)
            {
                strTextValue = new string[iNumTxt];
                ptTextCoords = new Point[iNumTxt];
                strTextTag = new string[iNumTxt];
                iTextColorCode = new int[iNumTxt];
                iTextFontCode = new int[iNumTxt];
                dgvText.Rows.Add(iNumTxt);

                for (int t = 0; t < iNumTxt; t++)
                {
                    ptTextCoords[t].X = Convert.ToInt32(sInData[iTxtOffset + 1 + (t * 6)]);
                    ptTextCoords[t].Y = Convert.ToInt32(sInData[iTxtOffset + 2 + (t * 6)]);
                    iTextColorCode[t] = Convert.ToInt32(sInData[iTxtOffset + 3 + (t * 6)]);
                    iTextFontCode[t] = Convert.ToInt32(sInData[iTxtOffset + 4 + (t * 6)]);
                    strTextTag[t] = sInData[iTxtOffset + 5 + (t * 6)];
                    strTextValue[t] = sInData[iTxtOffset + 6 + (t * 6)];

                    dgvText.Rows[t].Cells[0].Value = t;
                    dgvText.Rows[t].Cells[1].Value = ptTextCoords[t].X;
                    dgvText.Rows[t].Cells[2].Value = ptTextCoords[t].Y;
                    dgvText.Rows[t].Cells[3].Value = iTextColorCode[t];
                    dgvText.Rows[t].Cells[4].Value = iTextFontCode[t];
                    dgvText.Rows[t].Cells[5].Value = strTextTag[t];
                    dgvText.Rows[t].Cells[6].Value = strTextValue[t];
                }
            }
            bTextListUpdate = false;
            
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //gfx = this.pnRenderer.CreateGraphics();
            //gfx.Clear(Color.Transparent);
            //pnRenderer.Refresh();
            bmpDB = new Bitmap(pnRenderer.Width, pnRenderer.Height);
            gfxDB = Graphics.FromImage(bmpDB);

            // draw the workspace bounds
            sb = new SolidBrush(pbColorPreview.BackColor);
            gfxDB.FillRectangle(sb, rectBounds);

            // draw some mother fuckin' triangles
            // no pixel blending...  triangles are colored based on the color of the first vertex in the trifecta
            // extra points that cannot create a full triangle are ignored
            if (ptVertexCoords != null)
            {
                int peak = ((ptVertexCoords.GetUpperBound(0) + 1) / 3);
                for (int a = 0; a < peak * 3; a += 3)
                {
                    int iColorInd = iVertexColorCode[a];
                    Color c = Color.FromArgb(iColor_A[iColorInd], iColor_R[iColorInd], iColor_G[iColorInd], iColor_B[iColorInd]);

                    sb = new SolidBrush(c);
                    Point[] pts = new Point[3];
                    for (int b = 0; b < 3; b++)
                    {
                        pts[b] = ptVertexCoords[a + b];
                        pts[b].X -= pnRenderer.HorizontalScroll.Value;
                        pts[b].Y -= pnRenderer.VerticalScroll.Value;
                    }
                    gfxDB.FillPolygon(sb, pts);
                }
            }

            // draw text placeholder blocks
            if (ptTextCoords != null)
            {
                for (int a = 0; a <= ptTextCoords.GetUpperBound(0); a++)
                {
                    int iColorInd = iTextColorCode[a];
                    Color c = Color.FromArgb(iColor_A[iColorInd], iColor_R[iColorInd], iColor_G[iColorInd], iColor_B[iColorInd]);

                    pn = new Pen(c);
                    gfxDB.DrawRectangle(pn, ptTextCoords[a].X - pnRenderer.HorizontalScroll.Value, ptTextCoords[a].Y - pnRenderer.VerticalScroll.Value, 100, 20);
                }
            }

            // draw selected color preview block  
            if (lbColors.SelectedIndex != -1)
            {
                sb.Color = Color.FromArgb(iColor_A[lbColors.SelectedIndex], iColor_R[lbColors.SelectedIndex], iColor_G[lbColors.SelectedIndex], iColor_B[lbColors.SelectedIndex]);
                Graphics gfx2 = pbColorPreview.CreateGraphics();
                gfx2.Clear(pbColorPreview.BackColor);
                gfx2.FillRectangle(sb, new Rectangle(5, 5, pbColorPreview.Width - 15, pbColorPreview.Height - 15));
                gfx2.Dispose();
            }

            gfx = pnRenderer.CreateGraphics();
            //pnRenderer.Refresh();
            gfx.DrawImage(bmpDB, 0, 0);

        }

        private void cmdColorAdd_Click(object sender, EventArgs e)
        {
            int iNewCount = lbColors.Items.Count + 1;
            int[] tmp = null;

            lbColors.Items.Add("Color " + lbColors.Items.Count.ToString());

            tmp = iColor_A;
            iColor_A = new int[iNewCount];
            for (int a = 0; a < iNewCount - 1; a++)
                iColor_A[a] = tmp[a];
            iColor_A[iNewCount - 1] = 0;

            tmp = iColor_R;
            iColor_R = new int[iNewCount];
            for (int a = 0; a < iNewCount - 1; a++)
                iColor_R[a] = tmp[a];
            iColor_R[iNewCount - 1] = 0;

            tmp = iColor_G;
            iColor_G = new int[iNewCount];
            for (int a = 0; a < iNewCount - 1; a++)
                iColor_G[a] = tmp[a];
            iColor_G[iNewCount - 1] = 0;

            tmp = iColor_B;
            iColor_B = new int[iNewCount];
            for (int a = 0; a < iNewCount - 1; a++)
                iColor_B[a] = tmp[a];
            iColor_B[iNewCount - 1] = 0;

            lbColors.SelectedIndex = iNewCount - 1;
        }

        private void btnPreviewBackgroundColor_Click(object sender, EventArgs e)
        {
            cdPicker1.Color = Color.Black;

            DialogResult ff = cdPicker1.ShowDialog();
            if (ff.HasFlag(DialogResult.OK))
            {
                pbColorPreview.BackColor = cdPicker1.Color;
            }
            this.Invalidate();
        }

        private void lbColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtColorA.Text = iColor_A[lbColors.SelectedIndex].ToString();
            txtColorR.Text = iColor_R[lbColors.SelectedIndex].ToString();
            txtColorG.Text = iColor_G[lbColors.SelectedIndex].ToString();
            txtColorB.Text = iColor_B[lbColors.SelectedIndex].ToString();
            this.Invalidate();
        }

        private void btnColorSave_Click(object sender, EventArgs e)
        {
            if (lbColors.SelectedIndex != -1)
            {

                try
                {
                    txtColorA.Text = txtColorA.Text.ToString();
                }
                catch
                {
                    txtColorA.Text = "0";
                }

                try
                {
                    txtColorR.Text = txtColorR.Text.ToString();
                }
                catch
                {
                    txtColorR.Text = "0";
                }

                try
                {
                    txtColorG.Text = txtColorG.Text.ToString();
                }
                catch
                {
                    txtColorG.Text = "0";
                }

                try
                {
                    txtColorB.Text = txtColorB.Text.ToString();
                }
                catch
                {
                    txtColorB.Text = "0";
                }


                iColor_A[lbColors.SelectedIndex] = Convert.ToInt32(txtColorA.Text);
                iColor_R[lbColors.SelectedIndex] = Convert.ToInt32(txtColorR.Text);
                iColor_G[lbColors.SelectedIndex] = Convert.ToInt32(txtColorG.Text);
                iColor_B[lbColors.SelectedIndex] = Convert.ToInt32(txtColorB.Text);

                lbColors_SelectedIndexChanged(sender, e);
            }
        }

        private void btnColorRemove_Click(object sender, EventArgs e)
        {
            int iCurIdx = lbColors.SelectedIndex;
            if (iCurIdx > -1)
            {
                lbColors.Items.Clear();
                txtColorA.Text = "";
                txtColorR.Text = "";
                txtColorG.Text = "";
                txtColorB.Text = "";

                int[] iTmpA = iColor_A;
                int[] iTmpR = iColor_R;
                int[] iTmpG = iColor_G;
                int[] iTmpB = iColor_B;

                iColor_A = new int[iColor_A.GetUpperBound(0)];
                iColor_R = new int[iColor_R.GetUpperBound(0)];
                iColor_G = new int[iColor_G.GetUpperBound(0)];
                iColor_B = new int[iColor_B.GetUpperBound(0)];

                for (int a = 0; a <= iColor_A.GetUpperBound(0); a++)
                {
                    lbColors.Items.Add("Color " + a.ToString());
                    if (a >= iCurIdx)
                    {
                        iColor_A[a] = iTmpA[a + 1];
                        iColor_R[a] = iTmpR[a + 1];
                        iColor_G[a] = iTmpG[a + 1];
                        iColor_B[a] = iTmpB[a + 1];
                    }
                    else
                    {
                        iColor_A[a] = iTmpA[a];
                        iColor_R[a] = iTmpR[a];
                        iColor_G[a] = iTmpG[a];
                        iColor_B[a] = iTmpB[a];
                    }
                }

                this.Invalidate();
            }
        }

        void dgvVertex_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (!bVertexListUpdate && (dgvVertex.Rows.Count > 0))
            {
                try
                {
                    int iNewVal = Convert.ToInt32(dgvVertex.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            ptVertexCoords[e.RowIndex].X = iNewVal;
                            break;
                        case 2:
                            ptVertexCoords[e.RowIndex].Y = iNewVal;
                            break;
                        case 3:
                            if (iNewVal > iColor_A.GetUpperBound(0))
                            {
                                iNewVal = 0;
                                dgvVertex.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                            }
                            iVertexColorCode[e.RowIndex] = iNewVal;
                            break;
                        default:
                            throw new Exception("What the fuck did you do?");
                    }
                    this.Invalidate();
                }
                catch
                {
                    dgvVertex.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            ptVertexCoords[e.RowIndex].X = 0;
                            break;
                        case 2:
                            ptVertexCoords[e.RowIndex].Y = 0;
                            break;
                        case 3:
                            iVertexColorCode[e.RowIndex] = 0;
                            break;
                        default:
                            throw new Exception("What the fuck did you do?");
                    }
                }
                
            }
        }

        void dgvText_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (!bTextListUpdate && (dgvText.Rows.Count > 0))
            {
                if (e.ColumnIndex < 5)
                {
                    try
                    {
                        int iNewVal = Convert.ToInt32(dgvText.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                ptTextCoords[e.RowIndex].X = iNewVal;
                                break;
                            case 2:
                                ptTextCoords[e.RowIndex].Y = iNewVal;
                                break;
                            case 3:
                                if (iNewVal > iColor_A.GetUpperBound(0))
                                {
                                    iNewVal = 0;
                                    dgvText.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                                }
                                iTextColorCode[e.RowIndex] = iNewVal;
                                break;
                            case 4:
                                iTextFontCode[e.RowIndex] = iNewVal;
                                break;
                            default:
                                throw new Exception("What the fuck did you do?");
                        }
                        this.Invalidate();
                    }
                    catch
                    {
                        dgvText.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                ptTextCoords[e.RowIndex].X = 0;
                                break;
                            case 2:
                                ptTextCoords[e.RowIndex].Y = 0;
                                break;
                            case 3:
                                iTextColorCode[e.RowIndex] = 0;
                                break;
                            case 4:
                                iTextFontCode[e.RowIndex] = 0;
                                break;
                            default:
                                throw new Exception("What the fuck did you do?");
                        }
                    }
                }
                else if (e.ColumnIndex == 5)
                {
                    strTextTag[e.RowIndex] = dgvText.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 6)
                {
                    strTextValue[e.RowIndex] = dgvText.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
            }           
        }


        private void txtBoundsWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int iNewVal = Convert.ToInt32(txtBoundsWidth.Text);
                if (iNewVal < 1)
                    iNewVal = 1;
                txtBoundsWidth.Text = iNewVal.ToString();
                rectBounds.Width = iNewVal;
                lblCheat.Left = iNewVal;
            }
            catch
            {
                txtBoundsWidth.Text = "1";
                rectBounds.Width = 1;
                
            }
            pnRenderer.Refresh();
            this.Invalidate();
        }

        private void txtBoundsHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int iNewVal = Convert.ToInt32(txtBoundsHeight.Text);
                if (iNewVal < 1)
                    iNewVal = 1;
                txtBoundsHeight.Text = iNewVal.ToString();
                rectBounds.Height = iNewVal;
                lblCheat.Top = iNewVal;
            }
            catch
            {
                txtBoundsHeight.Text = "1";
                rectBounds.Height = 1;
            }
            pnRenderer.Refresh();
            this.Invalidate();
        }

        void pnRenderer_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            this.Invalidate();
        }

        private void btnNewVert_Click(object sender, EventArgs e)
        {
            if (iColor_A == null)
            {
                MessageBox.Show("Need at least 1 color definition before creating a new vertex.");
            }
            else
            {
                int iCurUBound = ptVertexCoords.GetUpperBound(0);
                Point[] ptTmp = ptVertexCoords;
                int[] iTmp = iVertexColorCode;

                ptVertexCoords = new Point[iCurUBound + 2];
                iVertexColorCode = new int[iCurUBound + 2];

                for (int a = 0; a < iCurUBound + 1; a++)
                {
                    ptVertexCoords[a] = ptTmp[a];
                    iVertexColorCode[a] = iTmp[a];
                }
                ptVertexCoords[iCurUBound + 1].X = 0;
                ptVertexCoords[iCurUBound + 1].Y = 0;
                iVertexColorCode[iCurUBound + 1] = 0;

                bVertexListUpdate = true;
                dgvVertex.Rows.Add();
                dgvVertex.Rows[iCurUBound + 1].Cells[0].Value = iCurUBound + 1;
                dgvVertex.Rows[iCurUBound + 1].Cells[1].Value = 0;
                dgvVertex.Rows[iCurUBound + 1].Cells[2].Value = 0;
                dgvVertex.Rows[iCurUBound + 1].Cells[3].Value = 0;
                bVertexListUpdate = false;
                dgvVertex.Rows[iCurUBound + 1].Cells[0].Selected = true;
            }
        }

        private void btnRemoveVert_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection bullshit = dgvVertex.SelectedCells;
            int iRowNum = -1;
            foreach (DataGridViewTextBoxCell c in bullshit)
            {
                iRowNum = c.RowIndex;
            }

            if (iRowNum > -1)
            {
                DialogResult dr = MessageBox.Show("Delete vertex index " + iRowNum + "?", "Dafuq?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    Point[] ptTmp = new Point[ptVertexCoords.GetUpperBound(0)];
                    int[] iTmp = new int[ptVertexCoords.GetUpperBound(0)];

                    for (int a = 0; a <= ptTmp.GetUpperBound(0); a++)
                        if (a >= iRowNum)
                        {
                            iTmp[a] = iVertexColorCode[a + 1];
                            ptTmp[a] = ptVertexCoords[a + 1];
                        }
                        else
                        {
                            iTmp[a] = iVertexColorCode[a];
                            ptTmp[a] = ptVertexCoords[a];
                        }

                    iVertexColorCode = iTmp;
                    ptVertexCoords = ptTmp;

                    bVertexListUpdate = true;
                    dgvVertex.Rows.Clear();
                    if (iVertexColorCode.GetUpperBound(0) > -1)
                    {
                        dgvVertex.Rows.Add(iVertexColorCode.GetUpperBound(0) + 1);
                        for (int v = 0; v <= iVertexColorCode.GetUpperBound(0); v++)
                        {
                            dgvVertex.Rows[v].Cells[0].Value = v;
                            dgvVertex.Rows[v].Cells[1].Value = ptVertexCoords[v].X;
                            dgvVertex.Rows[v].Cells[2].Value = ptVertexCoords[v].Y;
                            dgvVertex.Rows[v].Cells[3].Value = iVertexColorCode[v];
                        }
                    }
                    bVertexListUpdate = false;
                    this.Invalidate();
                }
            }
        }

        private void btnVertUp_Click(object sender, EventArgs e)
        {
            if ((iColor_A != null) && (dgvVertex.SelectedCells[0].RowIndex > 0))  // grid isn't empty and first row isn't selected
            {
                // store selected row's index
                int iSelIndex = dgvVertex.SelectedCells[0].RowIndex;
                
                // update vertex storage info
                Point ptTmp = ptVertexCoords[iSelIndex];
                ptVertexCoords[iSelIndex] = ptVertexCoords[iSelIndex - 1];
                ptVertexCoords[iSelIndex - 1] = ptTmp;

                int iTmp = iVertexColorCode[iSelIndex];
                iVertexColorCode[iSelIndex] = iVertexColorCode[iSelIndex - 1];
                iVertexColorCode[iSelIndex - 1] = iTmp;

                // update datagridview
                bVertexListUpdate = true;
                dgvVertex.Rows.Clear();
                dgvVertex.Rows.Add(iVertexColorCode.GetUpperBound(0) + 1);
                for (int v = 0; v <= iVertexColorCode.GetUpperBound(0); v++)
                {
                    dgvVertex.Rows[v].Cells[0].Value = v;
                    dgvVertex.Rows[v].Cells[1].Value = ptVertexCoords[v].X;
                    dgvVertex.Rows[v].Cells[2].Value = ptVertexCoords[v].Y;
                    dgvVertex.Rows[v].Cells[3].Value = iVertexColorCode[v];
                }
                bVertexListUpdate = false;
                dgvVertex.Rows[iSelIndex - 1].Cells[0].Selected = true;
                this.Invalidate();

            }
        }

        private void btnVertDown_Click(object sender, EventArgs e)
        {
            if ((iColor_A != null) && (dgvVertex.SelectedCells[0].RowIndex < dgvVertex.Rows.Count - 1))  // grid isn't empty and last row isn't selected
            {
                // store selected row's index
                int iSelIndex = dgvVertex.SelectedCells[0].RowIndex;

                // update vertex storage info
                Point ptTmp = ptVertexCoords[iSelIndex];
                ptVertexCoords[iSelIndex] = ptVertexCoords[iSelIndex + 1];
                ptVertexCoords[iSelIndex + 1] = ptTmp;

                int iTmp = iVertexColorCode[iSelIndex];
                iVertexColorCode[iSelIndex] = iVertexColorCode[iSelIndex + 1];
                iVertexColorCode[iSelIndex + 1] = iTmp;

                // update datagridview
                bVertexListUpdate = true;
                dgvVertex.Rows.Clear();
                dgvVertex.Rows.Add(iVertexColorCode.GetUpperBound(0) + 1);
                for (int v = 0; v <= iVertexColorCode.GetUpperBound(0); v++)
                {
                    dgvVertex.Rows[v].Cells[0].Value = v;
                    dgvVertex.Rows[v].Cells[1].Value = ptVertexCoords[v].X;
                    dgvVertex.Rows[v].Cells[2].Value = ptVertexCoords[v].Y;
                    dgvVertex.Rows[v].Cells[3].Value = iVertexColorCode[v];
                }
                bVertexListUpdate = false;
                dgvVertex.Rows[iSelIndex + 1].Cells[0].Selected = true;
                this.Invalidate();

            }
        }
        
        
        private void btnNewTxt_Click(object sender, EventArgs e)
        {
            if (iColor_A == null)
            {
                MessageBox.Show("Need at least 1 color definition before creating a new text object.");
            }
            else
            {
                int iCurUBound = strTextValue.GetUpperBound(0);
                Point[] ptTmp = ptTextCoords;
                int[] iTmp1 = iTextColorCode;
                int[] iTmp2 = iTextFontCode;
                string[] sTmp1 = strTextTag;
                string[] sTmp2 = strTextValue;

                ptTextCoords = new Point[iCurUBound + 2];
                iTextColorCode = new int[iCurUBound + 2];
                iTextFontCode = new int[iCurUBound + 2];
                strTextTag = new string[iCurUBound + 2];
                strTextValue = new string[iCurUBound + 2];

                for (int a = 0; a < iCurUBound + 1; a++)
                {
                    ptTextCoords[a] = ptTmp[a];
                    iTextColorCode[a] = iTmp1[a];
                    iTextFontCode[a] = iTmp2[a];
                    strTextTag[a] = sTmp1[a];
                    strTextValue[a] = sTmp2[a];
                }

                ptTextCoords[iCurUBound + 1].X = 0;
                ptTextCoords[iCurUBound + 1].Y = 0;
                iTextColorCode[iCurUBound + 1] = 0;
                iTextFontCode[iCurUBound + 1] = 0;
                strTextTag[iCurUBound + 1] = "NewTag";
                strTextValue[iCurUBound + 1] = "NewStringValue";

                bTextListUpdate = true;
                dgvText.Rows.Add();
                dgvText.Rows[iCurUBound + 1].Cells[0].Value = iCurUBound + 1;
                dgvText.Rows[iCurUBound + 1].Cells[1].Value = 0;
                dgvText.Rows[iCurUBound + 1].Cells[2].Value = 0;
                dgvText.Rows[iCurUBound + 1].Cells[3].Value = 0;
                dgvText.Rows[iCurUBound + 1].Cells[4].Value = 0;
                dgvText.Rows[iCurUBound + 1].Cells[5].Value = "NewTag";
                dgvText.Rows[iCurUBound + 1].Cells[6].Value = "NewStringValue";
                bTextListUpdate = false;
                dgvText.Rows[iCurUBound + 1].Cells[0].Selected = true;
            }
        }

        private void btnTxtDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection bullshit = dgvText.SelectedCells;
            int iRowNum = -1;
            foreach (DataGridViewTextBoxCell c in bullshit)
            {
                iRowNum = c.RowIndex;
            }

            if (iRowNum > -1)
            {
                DialogResult dr = MessageBox.Show("Delete text index " + iRowNum + "?", "Dafuq?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    Point[] ptTmp = new Point[ptTextCoords.GetUpperBound(0)];
                    int[] iTmp1 = new int[ptTextCoords.GetUpperBound(0)];
                    int[] iTmp2 = new int[ptTextCoords.GetUpperBound(0)];
                    string[] sTmp1 = new string[ptTextCoords.GetUpperBound(0)];
                    string[] sTmp2 = new string[ptTextCoords.GetUpperBound(0)];

                    for (int a = 0; a <= ptTmp.GetUpperBound(0); a++)
                        if (a >= iRowNum)
                        {
                            iTmp1[a] = iTextColorCode[a + 1];
                            iTmp2[a] = iTextFontCode[a + 1];
                            sTmp1[a] = strTextTag[a + 1];
                            sTmp2[a] = strTextValue[a + 1];
                            ptTmp[a] = ptTextCoords[a + 1];
                        }
                        else
                        {
                            iTmp1[a] = iTextColorCode[a];
                            iTmp2[a] = iTextFontCode[a];
                            sTmp1[a] = strTextTag[a];
                            sTmp2[a] = strTextValue[a];
                            ptTmp[a] = ptTextCoords[a];
                        }

                    iTextColorCode = iTmp1;
                    iTextFontCode = iTmp2;
                    ptTextCoords = ptTmp;
                    strTextTag = sTmp1;
                    strTextValue = sTmp2;

                    bTextListUpdate = true;
                    dgvText.Rows.Clear();
                    if (ptTextCoords.GetUpperBound(0) > -1)
                    {
                        dgvText.Rows.Add(ptTextCoords.GetUpperBound(0) + 1);
                        for (int v = 0; v <= ptTextCoords.GetUpperBound(0); v++)
                        {
                            dgvText.Rows[v].Cells[0].Value = v;
                            dgvText.Rows[v].Cells[1].Value = ptTextCoords[v].X;
                            dgvText.Rows[v].Cells[2].Value = ptTextCoords[v].Y;
                            dgvText.Rows[v].Cells[3].Value = iTextColorCode[v];
                            dgvText.Rows[v].Cells[4].Value = iTextFontCode[v];
                            dgvText.Rows[v].Cells[5].Value = strTextTag[v];
                            dgvText.Rows[v].Cells[6].Value = strTextValue[v];
                        }
                    }
                    bTextListUpdate = false;
                    this.Invalidate();
                }
            }
        }

        private void btnTxtUp_Click(object sender, EventArgs e)
        {

        }

        private void btnTxtDown_Click(object sender, EventArgs e)
        {

        }
    }
}
