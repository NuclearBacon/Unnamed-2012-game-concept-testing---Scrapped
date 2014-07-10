using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace hashgenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static string[] sEnc = new string[6];
        public static string[] sCombo;

        [STAThread]
        static void Main()
        {
            LoadSettings();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            //Application.Run(new frmDataFileBuilder());
        }

        private static void LoadSettings()
        {
            try
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\devtoolsettings.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                string[] fileVals = sr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                sr.Close();
                fs.Close();
                sr.Dispose();
                fs.Dispose();

                for (int a = 0; a < 6; a++)
                    sEnc[a] = fileVals[a];

                if (fileVals.GetUpperBound(0) > 5)
                {
                    sCombo = new string[fileVals.GetUpperBound(0) - 5];
                    for (int a = 6; a <= fileVals.GetUpperBound(0); a++)
                    {
                        sCombo[a - 6] = fileVals[a];
                    }
                }

            }
            catch { }
        }
    }
}
