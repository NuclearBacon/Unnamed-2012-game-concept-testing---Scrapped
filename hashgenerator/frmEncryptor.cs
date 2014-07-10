using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace hashgenerator
{
    public partial class frmEncryptor : Form
    {
        public frmEncryptor()
        {
            InitializeComponent();
            lbResults.DoubleClick += new EventHandler(lbResults_DoubleClick);
            OpenOldFile();

            for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                cmbEnc.Items.Add("Combo " + (a + 1).ToString() + ": " + Program.sCombo[a]);
        }

        private void OpenOldFile()
        {
            try
            {
                string sSP = Application.StartupPath + "\\strCore.txt";
                FileStream fs = new FileStream(sSP, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txtInput.Text = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                fs.Close();
                fs.Dispose();
                lbResults.Items.Add("Loaded old file " + Application.StartupPath + "\\strCore.txt");
            }
            catch
            {
                lbResults.Items.Add("Unable to open file " + Application.StartupPath + "\\strCore.txt");
            }
               
        }

        void lbResults_DoubleClick(object sender, EventArgs e)
        {
            if (lbResults.SelectedItem != null)
                Clipboard.SetDataObject(lbResults.SelectedItem.ToString());
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbResults.Items.Clear();
            
            // parse out comment lines and blank lines
            string[] sParse = txtInput.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string sInput = "";
            for (int a = 0; a <= sParse.GetUpperBound(0); a++)
                if (sParse[a].Substring(0, 2) != "//")
                    sInput += sParse[a] + "\r\n";

            string sOut;
            try
            {
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(txtMD5.Text));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateEncryptor();
                byte[] input = Encoding.Unicode.GetBytes(sInput);

                sOut = Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
            }
            catch
            {
                sOut = null;
            }
            lbResults.Items.Add(sOut);
            


            if (checkBox1.Checked)
            {
                string sCB = "";
                for (int a = 0; a < lbResults.Items.Count; a++)
                {
                    sCB += lbResults.Items[a].ToString() + "\r\n";
                }
                Clipboard.SetDataObject(sCB);
            }

            // cheats and presses the save button
            button3_Click(sender, e);
        }

        private void lbResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmEncryptor_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string sSP = Application.StartupPath + "\\strCore.txt";
                FileStream fs = new FileStream(sSP, FileMode.Create, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(txtInput.Text);
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
                lbResults.Items.Add("Saved file " + Application.StartupPath + "\\strCore.txt");
            }
            catch
            {
                lbResults.Items.Add("Unable to save file " + Application.StartupPath + "\\strCore.txt");
            }
        }

        private void cmbEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            txtMD5.Text = "";
            for (int a = 0; a < Program.sCombo[cmbEnc.SelectedIndex].Length; a++)
                txtMD5.Text += Program.sEnc[Convert.ToInt32(Program.sCombo[cmbEnc.SelectedIndex].Substring(a, 1)) - 1];
        }

        

    }
}
