using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hashgenerator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnHasher_Click(object sender, EventArgs e)
        {
            bool bAlreadyOpen = false;
            bool bConfigOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmHashcodeGen")
                    bAlreadyOpen = true;
                if (f.Name == "frmEncSettings")
                    bConfigOpen = true;
            }

            if (bConfigOpen)
                MessageBox.Show("Unable to open module while configuration dialog is open.");
            else if (!bAlreadyOpen)
            {
                frmHashcodeGen form1 = new frmHashcodeGen();
                form1.Show();
            }
            else
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmHashcodeGen")
                        f.Focus();
                }
        }

        private void btnEncryptor_Click(object sender, EventArgs e)
        {
            bool bAlreadyOpen = false;
            bool bConfigOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmEncryptor")
                    bAlreadyOpen = true;
                if (f.Name == "frmEncSettings")
                    bConfigOpen = true;
            }

            if (bConfigOpen)
                MessageBox.Show("Unable to open module while configuration dialog is open.");
            else if (!bAlreadyOpen)
            {
                frmEncryptor frmecryptor = new frmEncryptor();
                frmecryptor.Show();
            }
            else
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmEncryptor")
                        f.Focus();
                }
        }

        private void btnConfigEnc_Click(object sender, EventArgs e)
        {
            bool bAlreadyOpen = false;
            bool bOthersOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmEncSettings")
                    bAlreadyOpen = true;
                if (f.Name != "frmMain")
                    bOthersOpen = true;
            }

            if (bOthersOpen && !bAlreadyOpen)
            {
                MessageBox.Show("Cannot configure encryption options while other modules are open.");
            }
            else if (!bAlreadyOpen)
            {
                frmEncSettings frmEncSettings = new frmEncSettings();
                frmEncSettings.Show();
            }
            else
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmEncSettings")
                        f.Focus();
                }
        }

        private void btnUIE_Click(object sender, EventArgs e)
        {
            bool bAlreadyOpen = false;
            bool bConfigOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmUIEditor")
                    bAlreadyOpen = true;
                if (f.Name == "frmEncSettings")
                    bConfigOpen = true;
            }

            if (bConfigOpen)
                MessageBox.Show("Unable to open module while configuration dialog is open.");
            else if (!bAlreadyOpen)
            {
                frmUIEditor frmUIEditor = new frmUIEditor();
                frmUIEditor.Show();
            }
            else
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmUIEditor")
                        f.Focus();
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool bAlreadyOpen = false;
            bool bConfigOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmDataFileBuilder")
                    bAlreadyOpen = true;
                if (f.Name == "frmEncSettings")
                    bConfigOpen = true;
            }

            if (bConfigOpen)
                MessageBox.Show("Unable to open module while configuration dialog is open.");
            else if (!bAlreadyOpen)
            {
                frmDataFileBuilder frmDataFileBuilder = new frmDataFileBuilder();
                frmDataFileBuilder.Show();
            }
            else
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmDataFileBuilder")
                        f.Focus();
                }
        }
    }
}
