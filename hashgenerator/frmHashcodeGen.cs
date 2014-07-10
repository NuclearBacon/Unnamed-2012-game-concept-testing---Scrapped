using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace hashgenerator
{
    public partial class frmHashcodeGen : Form
    {
        public frmHashcodeGen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtPath.Text = Application.StartupPath + "\\dasspiel.exe";
            lblResults.Text = "";

            for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                cmbMD5.Items.Add("Combo " + (a + 1).ToString() + ": " + Program.sCombo[a]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string derpa;
            string darpa;

            using (FileStream stream = File.OpenRead(txtPath.Text))
            {
                SHA256Managed sha = new SHA256Managed();
                StreamReader sr = new StreamReader(stream);
                string instr = sr.ReadToEnd();
                instr = instr.Substring(0, instr.Length - 64);
                sr.Close();
                sr.Dispose();
                byte[] derp = System.Text.Encoding.ASCII.GetBytes(instr);

                byte[] chks = sha.ComputeHash(derp);
                derpa = BitConverter.ToString(chks).Replace("-", string.Empty);
            }

            txtUncoded.Text = derpa;

            try
            {
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(txtMD5.Text));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateEncryptor();
                byte[] input = Encoding.Unicode.GetBytes(derpa);

                darpa = Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
            }
            catch
            {
                darpa = null;
            }

            int iTruncateLength = 64;
            txtCodedFull.Text = darpa;
            txtCodedPartial.Text = darpa.Substring(0, iTruncateLength);
            lblResults.Text = "Encoded string length: " + darpa.Length + "\r\n" + "Encoded string truncated to length: " + iTruncateLength;
        }

        private void cmbMD5_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            txtMD5.Text = "";
            for (int a = 0; a < Program.sCombo[cmbMD5.SelectedIndex].Length; a++)
                txtMD5.Text += Program.sEnc[Convert.ToInt32(Program.sCombo[cmbMD5.SelectedIndex].Substring(a, 1)) - 1];
        }
    }
}
