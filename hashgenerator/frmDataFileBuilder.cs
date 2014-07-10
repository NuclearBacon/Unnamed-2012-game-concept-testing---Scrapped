using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace hashgenerator
{
    public partial class frmDataFileBuilder : Form
    {
        private StreamWriter sr;
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static void ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }

        public frmDataFileBuilder()
        {
            InitializeComponent();

            cmbEnc.Items.Add("<None>");
            for (int a = 0; a <= Program.sCombo.GetUpperBound(0); a++)
                cmbEnc.Items.Add("Combo " + (a + 1).ToString() + ": " + Program.sCombo[a]);
            cmbEnc.SelectedIndex = 0;
            treeView1.Sort();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowElement(treeView1.SelectedNode.ToolTipText);
        }

        void tsRoot_NewSubNode_Click(object sender, System.EventArgs e)
        {
            InputBox derp = new InputBox();
            string d = derp.Show("Need a name for the new subnode...", "New Subnode");

            if (d != "")
            {
                TreeNode tnTmp = new TreeNode(d);
                tnTmp.ContextMenuStrip = cmsTVRoot;
                tnTmp.Tag = "1";
                tnTmp.ImageIndex = 1;
                tnTmp.SelectedImageIndex = 1;
                
                treeView1.SelectedNode.Nodes.Add(tnTmp);
                treeView1.SelectedNode.Expand();
            }
        }

        void tsRoot_DeleteNode_Click(object sender, System.EventArgs e)
        {
            string sP = "Delete node '" + treeView1.SelectedNode.Text + "' and all subnodes/elements?";
            DialogResult DR = MessageBox.Show(sP, "ZOMG, deletin' stuffs!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (DR == System.Windows.Forms.DialogResult.Yes)
            {
                treeView1.SelectedNode.Remove();
            }
        }

        void tsRoot_RemoveElement_Click(object sender, System.EventArgs e)
        {
            string sP = "Remove element '" + treeView1.SelectedNode.Text + "'?";
            DialogResult DR = MessageBox.Show(sP, "ZOMG, deletin' stuffs!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (DR == System.Windows.Forms.DialogResult.Yes)
            {
                treeView1.SelectedNode.Remove();
            }            
        }

        void tsRoot_NewElement_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Multiselect = false;
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                TreeNode tnTmp = new TreeNode(ofd.SafeFileName);
                tnTmp.ContextMenuStrip = cmsTVRoot;
                tnTmp.Tag = "2";
                tnTmp.ToolTipText = ofd.FileName;

                switch (ofd.SafeFileName.Substring(ofd.SafeFileName.Length - 3))
                {
                    case "bmp":
                    case "jpg":
                    case "ico":
                    case "png":
                        tnTmp.ImageIndex = 3;
                        tnTmp.SelectedImageIndex = 3;
                        break;
                    case "txt":
                    case "ini":
                        tnTmp.ImageIndex = 4;
                        tnTmp.SelectedImageIndex = 4;
                        break;
                    default:
                        tnTmp.ImageIndex = 5;
                        tnTmp.SelectedImageIndex = 5;
                        break;
                }

                treeView1.SelectedNode.Nodes.Add(tnTmp);
                treeView1.SelectedNode.Expand();
            }


            ofd.Dispose();
        }

        void tsRoof_FileProperties_Click(object sender, System.EventArgs e)
        {
            ShowFileProperties(treeView1.SelectedNode.ToolTipText);

        }

        void tsRoot_RenameItem_Click(object sender, System.EventArgs e)
        {
            InputBox ib = new InputBox();
            string sResult = ib.Show("What's the new name, yo?", "Done changed your mind, eh?", treeView1.SelectedNode.Text);
            if (sResult != "")
            {
                treeView1.SelectedNode.Text = sResult;
            }
        }


        void cmsTVRoot_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsRoot_DeleteNode.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) == 1);
            tsRoot_NewSubNode.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) < 2);
            tsRoot_NewElement.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) < 2);
            tsRoot_RemoveElement.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) == 2);
            tsRoot_FileProperties.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) == 2);
            tsRoot_RenameItem.Visible = (int.Parse(treeView1.SelectedNode.Tag.ToString().Substring(0, 1)) > 0);
        }


        public class InputBox
        {
            Form frmIB = new Form();
            Label lblPrompt = new Label();
            TextBox txtEntry = new TextBox();
            Button btnOK = new Button();
            Button btnCancel = new Button();
            bool bClosedWithOK = false;

            public InputBox()
            {
                
                frmIB.Size = new Size(400, 130);
                frmIB.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                frmIB.MaximizeBox = false;
                frmIB.MinimizeBox = false;

                frmIB.Controls.Add(lblPrompt);
                lblPrompt.Left = 10;
                lblPrompt.Top = 10;
                lblPrompt.AutoSize = true;

                frmIB.Controls.Add(txtEntry);
                txtEntry.Left = 10;
                txtEntry.Width = 370;
                txtEntry.Top = 30;
                
                frmIB.Controls.Add(btnOK);
                btnOK.Size = new Size(80, 30);
                btnOK.Left = 210;
                btnOK.Top = 60;
                btnOK.Text = "OK";
                
                frmIB.Controls.Add(btnCancel);
                btnCancel.Size = new Size(80, 30);
                btnCancel.Left = 300;
                btnCancel.Top = 60;
                btnCancel.Text = "Cancel";

                frmIB.AcceptButton = btnOK;
                frmIB.CancelButton = btnCancel;

                btnOK.Click += new EventHandler(btnOK_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                frmIB.FormClosed += new FormClosedEventHandler(frmIB_FormClosed);
            }

            void frmIB_FormClosed(object sender, FormClosedEventArgs e)
            {
                if (!bClosedWithOK)
                    txtEntry.Text = "";
            }

            void btnCancel_Click(object sender, EventArgs e)
            {
                frmIB.Close();
            }

            public string Show(string sPrompt, string sTitle, string sDefault = "")
            {
                frmIB.Text = sTitle;
                lblPrompt.Text = sPrompt;
                txtEntry.Text = sDefault;

                frmIB.ShowDialog();
                return txtEntry.Text;
            }

            void btnOK_Click(object sender, EventArgs e)
            {
                bClosedWithOK = true;
                frmIB.Close();
            }
        }

        private void btnConfigPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.ShowDialog();

            if (fbd.SelectedPath != "")
            {
                txtConfigPath.Text = fbd.SelectedPath;
            }


            fbd.Dispose();
        }

        private void btnCompilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.ShowDialog();

            if (fbd.SelectedPath != "")
            {
                txtCompilePath.Text = fbd.SelectedPath;
            }


            fbd.Dispose();
        }

        private void cmbEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEncKey.Text = "";
            if (cmbEnc.SelectedIndex > 0)
            {
                for (int a = 0; a < Program.sCombo[cmbEnc.SelectedIndex - 1].Length; a++)
                    txtEncKey.Text += Program.sEnc[Convert.ToInt32(Program.sCombo[cmbEnc.SelectedIndex - 1].Substring(a, 1)) - 1];
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (txtArchiveName.Text == "")
                MessageBox.Show("Need an archive name, yo...");
            else if (txtConfigPath.Text == "")
                MessageBox.Show("Need an config file patch, dude...");
            else
            {
                sr = new StreamWriter(txtConfigPath.Text + txtArchiveName.Text + ".xml", false, Encoding.UTF8);
                sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sr.WriteLine("<node text=\"" + treeView1.Nodes[0].Text + "\" compilepath=\"" + txtCompilePath.Text + "\"" + 
                    " encryptioncombo=\"" + cmbEnc.SelectedIndex.ToString() + "\" major=\"" + nudMajor.Value.ToString() +
                    "\" minor=\"" + nudMinor.Value.ToString() + "\" build=\"" + nudBuild.Value.ToString() + "\" revision=\"" + 
                    nudRevision.Value.ToString() + "\">");
                SaveNode(treeView1.Nodes[0].Nodes);
                sr.WriteLine("</node>");
                sr.Close();

                MessageBox.Show("Saved configuration file:\r\n" + txtConfigPath.Text + txtArchiveName.Text + ".xml");
            }
        }

        private void SaveNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Nodes.Count > 0)
                {
                    sr.WriteLine("<node text=\"" + node.Text + "\"" + " ttt=\"" + node.ToolTipText + "\">");
                    SaveNode(node.Nodes);
                    sr.WriteLine("</node>");
                }
                else
                    sr.WriteLine("<node text=\"" + node.Text + "\"" + " ttt=\"" + node.ToolTipText + "\"/>");
            }
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open XML Document";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            dlg.FileName = Application.StartupPath + "\\..\\..\\example.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TreeNode masternode;
                XmlDocument dom = new XmlDocument();
                dom.Load(dlg.FileName);

                treeView1.Nodes.Clear();

                // setup root node
                treeView1.Nodes.Add(new TreeNode(dom.DocumentElement.GetAttribute("text")));
                treeView1.Nodes[0].Tag = "0";
                treeView1.Nodes[0].ContextMenuStrip = cmsTVRoot;

                // populate subnodes
                masternode = treeView1.Nodes[0];
                AddNode(dom.DocumentElement, masternode);
                treeView1.ExpandAll();

                // load general settings
                string sArchName = dom.DocumentElement.GetAttribute("text");
                sArchName = sArchName.Substring(0, sArchName.Length - 4);
                txtArchiveName.Text = sArchName;
                txtCompilePath.Text = dom.DocumentElement.GetAttribute("compilepath");
                cmbEnc.SelectedIndex = int.Parse(dom.DocumentElement.GetAttribute("encryptioncombo"));
                txtConfigPath.Text = dlg.FileName.Substring(0, dlg.FileName.Length - dlg.SafeFileName.Length);
                nudMajor.Value = decimal.Parse(dom.DocumentElement.GetAttribute("major"));
                nudMinor.Value = decimal.Parse(dom.DocumentElement.GetAttribute("minor"));
                nudBuild.Value = decimal.Parse(dom.DocumentElement.GetAttribute("build"));
                nudRevision.Value = decimal.Parse(dom.DocumentElement.GetAttribute("revision"));

            }
        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            int counter = 0;
            TreeNode ChildNode;
            XmlNodeList nodeList;
            XmlAttributeCollection attributes;


            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;

                foreach (XmlNode item in nodeList)
                {
                    if (item.NodeType == XmlNodeType.Element)
                    {
                        attributes = item.Attributes;
                        TreeNode tn = new TreeNode();
                                
                        foreach (XmlAttribute x in attributes)
                        {
                            if (x.Name == "text")
                                tn.Text = x.Value;
                            if (x.Name == "ttt")
                                tn.ToolTipText = x.Value;
                        }
                        tn.ContextMenuStrip = cmsTVRoot;
                        if (tn.ToolTipText == "")
                        {
                            tn.Tag = "1";
                            tn.ImageIndex = 1;
                            tn.SelectedImageIndex = 1;
                        }
                        else
                        {
                            tn.Tag = "2";
                            switch (tn.ToolTipText.Substring(tn.ToolTipText.Length - 3))
                            {
                                case "bmp":
                                case "jpg":
                                case "ico":
                                case "png":
                                    tn.ImageIndex = 3;
                                    tn.SelectedImageIndex = 3;
                                    break;
                                case "txt":
                                case "ini":
                                    tn.ImageIndex = 4;
                                    tn.SelectedImageIndex = 4;
                                    break;
                                default:
                                    tn.ImageIndex = 5;
                                    tn.SelectedImageIndex = 5;
                                    break;
                            }
                        }

                        inTreeNode.Nodes.Add(tn);
                        ChildNode = inTreeNode.Nodes[counter];
                        counter++;
                        AddNode(item, ChildNode);
                    }
                }
            }
        } 

        private void txtArchiveName_TextChanged(object sender, EventArgs e)
        {
            treeView1.Nodes[0].Text = txtArchiveName.Text + ".jda";
        }

        private void ShowElement(string sFilePath)
        {
            panel2.Controls.Clear();

            if (sFilePath != "")
            {
                string[] derp = sFilePath.Split(new string[] { "." }, StringSplitOptions.None);
                switch (derp[1])
                {
                    case "txt":
                    case "ini":
                        TextBox tb1 = new TextBox();
                        tb1.Multiline = true;
                        panel2.Controls.Add(tb1);
                        tb1.Dock = DockStyle.Fill;
                        tb1.ScrollBars = ScrollBars.Both;
                        tb1.ReadOnly = true;

                        StreamReader srpreview1 = new StreamReader(sFilePath);
                        tb1.Text = srpreview1.ReadToEnd();

                        break;
                    case "jpg":
                    case "png":
                    case "bmp":
                    case "ico":
                        PictureBox pb1 = new PictureBox();
                        panel2.Controls.Add(pb1);
                        pb1.Image = Image.FromFile(sFilePath);
                        pb1.Size = pb1.Image.Size;
                        panel2.AutoScroll = true;
                        break;
                    default:
                        FileInfo finfo = new FileInfo(sFilePath);
                        bool bOpenFile = true;
                        if (finfo.Length > 1024 * 1024)
                        {
                            DialogResult dr = MessageBox.Show("Unknown file type detected of size > 1MB.  Load binary?\r\n\r\n" + sFilePath, "Uhm...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == System.Windows.Forms.DialogResult.No)
                                bOpenFile = false;
                        }
                        if (bOpenFile)
                        {
                            TextBox tb2 = new TextBox();
                            tb2.Multiline = true;
                            panel2.Controls.Add(tb2);
                            tb2.Dock = DockStyle.Fill;
                            tb2.ScrollBars = ScrollBars.Both;
                            tb2.ReadOnly = true;
                            tb2.Font = new Font("String Literal", 8);

                            FileStream fs = new FileStream(sFilePath, FileMode.Open);
                            BinaryReader br = new BinaryReader(fs);

                            byte[] b = br.ReadBytes(int.Parse(finfo.Length.ToString()));
                            string sHex = string.Empty;
                            br.Close();
                            fs.Close();
                            br.Dispose();
                            fs.Dispose();

                            int iCycle = 0;
                            foreach (byte bb in b)
                            {
                                sHex += bb.ToString("X2");
                                iCycle++;
                                if (iCycle < 17)
                                    sHex += " ";
                                if (iCycle == 8)
                                    sHex += " ";
                                if (iCycle == 17)
                                {
                                    iCycle = 0;
                                    sHex += "\r\n";
                                }
                            }

                            tb2.Text = sHex;
                        }

                        break;
                }
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if (txtArchiveName.Text == "")
                MessageBox.Show("Need an archive name, dipshit.");
            else if (txtCompilePath.Text == "")
                MessageBox.Show("How about a compile path?");
            else
            {
                DialogResult dr = MessageBox.Show("Heh....   really?", "Dun...  dun...  DUNNNNN!!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // Initial setup //////////////////////////////////////////////////////////////////////////////////////////////
                    // Create compile directory and build file
                    if (!Directory.Exists(txtCompilePath.Text))
                        Directory.CreateDirectory(txtCompilePath.Text);
                    FileStream fs = new FileStream(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda.temp", FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    // Purge old compile files and folders if present
                    if (Directory.Exists(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda"))
                        DeleteDirectory(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda");
                    if (File.Exists(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda"))
                        File.Delete(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda");

                    // Encryption byte
                    bw.Write((byte)(cmbEnc.SelectedIndex - 1));

                    // Archive header //////////////////////////////////////////////////////////////////////////////////////////////
                    // Raw archive header
                    byte[] bArchName = Encoding.ASCII.GetBytes(txtArchiveName.Text);
                    byte[] bArchHeader = new byte[16 + bArchName.Length];  // (4 * 4 build versions) = 16
                    System.Buffer.BlockCopy(BitConverter.GetBytes((int)nudMajor.Value), 0, bArchHeader, 0, 4);
                    System.Buffer.BlockCopy(BitConverter.GetBytes((int)nudMinor.Value), 0, bArchHeader, 4, 4);
                    System.Buffer.BlockCopy(BitConverter.GetBytes((int)nudBuild.Value), 0, bArchHeader, 8, 4);
                    System.Buffer.BlockCopy(BitConverter.GetBytes((int)nudRevision.Value), 0, bArchHeader, 12, 4);
                    System.Buffer.BlockCopy(bArchName, 0, bArchHeader, 16, bArchName.Length);

                    // Hash generation
                    byte[] bAH_SHA = ToSHA256(bArchHeader);
                    byte[] bHeadTmp = new byte[bArchHeader.Length + 8];
                    for (int a = 0; a < bArchHeader.Length; a++)
                        bHeadTmp[a] = bArchHeader[a];
                    bArchHeader = bHeadTmp;
                    System.Buffer.BlockCopy(bAH_SHA, 0, bArchHeader, bArchHeader.Length - 8, 8);

                    // Encrypt archive header if needed
                    if (cmbEnc.SelectedIndex > 0)
                        bArchHeader = EncStr(bArchHeader);

                    // Write header data
                    bw.Write(BitConverter.GetBytes(bArchHeader.Length));
                    bw.Write(bArchHeader);

                    // Record table //////////////////////////////////////////////////////////////////////////////////////////////
                    int iElementTableOffset = bArchHeader.Length + 5;
                    // Collect element records from treeview
                    List<String> lElementNodePath = new List<string>();
                    List<String> lElementFilePath = new List<string>();
                    ProcessNodes(treeView1.Nodes[0], lElementNodePath, lElementFilePath);

                    // Setup compile work folders and duplicate files
                    CreateFolders(txtCompilePath.Text, treeView1.Nodes[0]);
                    for (int a = 0; a < lElementFilePath.Count; a++)
                        File.Copy(lElementFilePath[a], txtCompilePath.Text + "\\" + lElementNodePath[a]);

                    // File hashing
                    List<byte[]> lElementHash = new List<byte[]>();
                    for (int a = 0; a < lElementNodePath.Count; a++)
                        lElementHash.Add(ToSHA256(txtCompilePath.Text + "\\" + lElementNodePath[a]));
                    
                    // Individual element encryption
                    if (cmbEnc.SelectedIndex > 0)
                    {
                        FileStream fsEnc;
                        BinaryReader brEnc;
                        BinaryWriter bwEnc;
                        byte[] byWorking;
                        
                        for (int a = 0; a < lElementNodePath.Count; a++)
                        {
                            // load unencrypted resource
                            fsEnc = new FileStream(txtCompilePath.Text + "\\" + lElementNodePath[a], FileMode.Open);
                            brEnc = new BinaryReader(fsEnc);
                            byWorking = brEnc.ReadBytes((int)fsEnc.Length);
                            brEnc.Close();
                            fsEnc.Close();

                            // encrypt resource
                            byWorking = EncStr(byWorking);
                            
                            // save encrypted resource
                            fsEnc = new FileStream(txtCompilePath.Text + "\\" + lElementNodePath[a] + ".encrypted", FileMode.Create);
                            bwEnc = new BinaryWriter(fsEnc);
                            bwEnc.Write(byWorking);
                            bwEnc.Close();
                            fsEnc.Close();
                            
                            // remove old file and rename new
                            File.Delete(txtCompilePath.Text + "\\" + lElementNodePath[a]);
                            File.Move(txtCompilePath.Text + "\\" + lElementNodePath[a] + ".encrypted", txtCompilePath.Text + "\\" + lElementNodePath[a]);
                            
                            fsEnc.Dispose();
                            brEnc.Dispose();
                            bwEnc.Dispose();
                        }
                    }

                    // Collect record information
                    List<int> lFileSize = new List<int>();
                    for (int a = 0; a < lElementNodePath.Count; a++)
                        lFileSize.Add((int)(new FileInfo(txtCompilePath.Text + "\\" + lElementNodePath[a]).Length));

                    // Create element records table
                    int iTableLen = 4 + (268 * lElementNodePath.Count);
                    byte[] bRecordTable = new byte[iTableLen];

                    // Populate record table
                    System.Buffer.BlockCopy(BitConverter.GetBytes(lElementNodePath.Count), 0, bRecordTable, 0, 4);
                    int iRecordAbsoluteOffset = iElementTableOffset + iTableLen + 12;
                    for (int a = 0; a < lElementNodePath.Count; a++)
                    {
                        // stores the relative offset in the record table where the entries actually start
                        byte[] bRecordOffset = BitConverter.GetBytes(iRecordAbsoluteOffset);
                        System.Buffer.BlockCopy(bRecordOffset, 0, bRecordTable, 4 + (268 * a), 4);
                        iRecordAbsoluteOffset += lFileSize[a];
                        
                        // stores file hash information
                        System.Buffer.BlockCopy(lElementHash[a], 0, bRecordTable, 8 + (268 * a), 8);

                        // stores internal file path information
                        byte[] bRecordPath = Encoding.ASCII.GetBytes(lElementNodePath[a]);
                        System.Buffer.BlockCopy(bRecordPath, 0, bRecordTable, 16 + (268 * a), bRecordPath.Length);
                    }

                    // Hash generation
                    byte[] bRT_SHA = ToSHA256(bRecordTable);
                    byte[] bRTTmp = new byte[bRecordTable.Length + 8];
                    for (int a = 0; a < bRecordTable.Length; a++)
                        bRTTmp[a] = bRecordTable[a];
                    bRecordTable = bRTTmp;
                    System.Buffer.BlockCopy(bRT_SHA, 0, bRecordTable, bRecordTable.Length - 8, 8);

                    // Encrypt record table if needed
                    if (cmbEnc.SelectedIndex > 0)
                        bRecordTable = EncStr(bRecordTable);

                    // Write record table to file
                    bw.Write(BitConverter.GetBytes(bRecordTable.Length));
                    bw.Write(bRecordTable);

                    // Record data //////////////////////////////////////////////////////////////////////////////////////////////
                    FileStream fsData;
                    BinaryReader brData;
                    for (int a = 0; a < lElementFilePath.Count; a++)
                    {
                        fsData = new FileStream(txtCompilePath.Text + "\\" + lElementNodePath[a], FileMode.Open);
                        brData = new BinaryReader(fsData);
                        bw.Write(brData.ReadBytes((int)fsData.Length));
                        brData.Close();
                        fsData.Close();
                        brData.Dispose();
                        fsData.Dispose();
                    }

                    // Cleanup //////////////////////////////////////////////////////////////////////////////////////////////
                    bw.Close();
                    fs.Close();
                    bw.Dispose();
                    fs.Dispose();
                    DeleteDirectory(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda");
                    File.Move(txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda.temp", txtCompilePath.Text + "\\" + txtArchiveName.Text + ".jda");

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        // compiler sub methods
        public byte[] EncStr(byte[] bRaw)
        {
            byte[] bWorking = null;
            string sK = txtEncKey.Text;

            try
            {
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(sK));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateEncryptor();

                bWorking = ct.TransformFinalBlock(bRaw, 0, bRaw.Length);
            }
            catch
            {
                bWorking = null;
            }

            return bWorking;
        }
        public byte[] ToSHA256(byte[] bRaw)
        {
            SHA256Managed sha = new SHA256Managed();
            
            byte[] chks = sha.ComputeHash(bRaw);
            byte[] shortness = new byte[8];
            for (int a = 0; a < 8; a++)
                shortness[a] = chks[a];

            sha.Dispose();

            return shortness;
        }
        public byte[] ToSHA256(string sFilePath)
        {
            SHA256Managed sha = new SHA256Managed();
            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] indat = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            br.Dispose();
            fs.Dispose();

            byte[] chks = sha.ComputeHash(indat);
            byte[] shortness = new byte[8];
            for (int a = 0; a < 8; a++)
                shortness[a] = chks[a];
            
            sha.Dispose();

            return shortness;
        }
        private void ProcessNodes(TreeNode tn, List<String> NodePaths, List<string> NodeFilePaths)
        {
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode t in tn.Nodes)
                    ProcessNodes(t, NodePaths, NodeFilePaths);
            }
            if (tn.ToolTipText != "")
            {
                NodePaths.Add(tn.FullPath);
                NodeFilePaths.Add(tn.ToolTipText);
            }
        }
        private void CreateFolders(string sRootPath, TreeNode tn)
        {
            if (tn.ToolTipText == "")
            {
                if (!Directory.Exists(sRootPath + "\\" + tn.FullPath))
                {
                    Directory.CreateDirectory(sRootPath + "\\" + tn.FullPath);
                }
                if (tn.Nodes.Count > 0)
                    foreach (TreeNode t in tn.Nodes)
                        CreateFolders(sRootPath, t);
            }
        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            System.Threading.Thread.Sleep(5);
            Directory.Delete(target_dir, false);
        }
    }
}
