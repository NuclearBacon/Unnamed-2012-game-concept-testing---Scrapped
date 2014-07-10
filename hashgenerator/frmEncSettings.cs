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
    public partial class frmEncSettings : Form
    {
        public frmEncSettings()
        {
            InitializeComponent();

            txtEnc1.Text = Program.sEnc[0];
            txtEnc2.Text = Program.sEnc[1];
            txtEnc3.Text = Program.sEnc[2];
            txtEnc4.Text = Program.sEnc[3];
            txtEnc5.Text = Program.sEnc[4];
            txtEnc6.Text = Program.sEnc[5];

            for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                cmbCombos.Items.Add("Combo " + (a + 1).ToString());
        }

        private void cmbCombos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSequence.Text = Program.sCombo[cmbCombos.SelectedIndex];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string[] sTmp = Program.sCombo;
            Program.sCombo = new string[sTmp.GetUpperBound(0) + 2];
            for (int a = 0; a < Program.sCombo.GetUpperBound(0); a++)
                Program.sCombo[a] = sTmp[a];
            cmbCombos.Items.Add("Combo " + (sTmp.GetUpperBound(0) + 2).ToString());
            cmbCombos.SelectedIndex = cmbCombos.Items.Count - 1;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            txtSequence.Text = "";
            int iCurIdx = cmbCombos.SelectedIndex;
            if (iCurIdx > -1)
            {
                string[] sTmp = Program.sCombo;
                Program.sCombo = new string[sTmp.GetUpperBound(0)];
                cmbCombos.Items.Clear();

                for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                {
                    if (a >= iCurIdx)
                        Program.sCombo[a] = sTmp[a + 1];
                    else
                        Program.sCombo[a] = sTmp[a];
                    cmbCombos.Items.Add("Combo " + (a + 1).ToString());
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCombos.SelectedIndex > -1)
            {
                Program.sCombo[cmbCombos.SelectedIndex] = txtSequence.Text;

                string sOut = "";
                for (int a = 0; a <= Program.sEnc.GetUpperBound(0); a++)
                    sOut += Program.sEnc[a] + "\r\n";
                for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                    sOut += Program.sCombo[a] + "\r\n";

                try
                {
                    FileStream fs = new FileStream(Application.StartupPath + "\\devtoolsettings.txt", FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.Write(sOut);
                    sw.Close();
                    fs.Close();
                    sw.Dispose();
                    fs.Dispose();
                }
                catch
                { 
                    
                }
                
            }
        }

        private void btnGenCode_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";

            txtCode.Text += "private string EnCombo(int iCombo)\r\n{\r\n    switch (iCombo)\r\n    {\r\n";

            for (int a = 1; a <= Program.sCombo.GetUpperBound(0); a++)
            {
                txtCode.Text += "        case " + a.ToString() + ":\r\n            return ";
                for (int b = 0; b < Program.sCombo[a].Length; b++)
                {
                    txtCode.Text += "rRes.sEnc" + Program.sCombo[a].Substring(b, 1) + " + ";
                }
                txtCode.Text = txtCode.Text.Substring(0, txtCode.Text.Length - 3) + ";\r\n";
            }

            txtCode.Text += "        default:\r\n            return ";
            for (int b = 0; b < Program.sCombo[0].Length; b++)
            {
                txtCode.Text += "rRes.sEnc" + Program.sCombo[0].Substring(b, 1) + " + ";
            }
            txtCode.Text = txtCode.Text.Substring(0, txtCode.Text.Length - 3) + ";\r\n    }\r\n}";

            Clipboard.SetDataObject(txtCode.Text);
        }


    }
}
