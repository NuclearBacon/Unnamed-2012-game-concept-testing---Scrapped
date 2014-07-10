using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using rRes = dasspiel.Properties.Resources;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;


namespace dasspiel
{
    static class core
    {
        // Variable declaration
        #region VARDEC

        // Core crap
        public static string cr = "\r\n";
        public static GameSettingsManager settings;
        public static Util utilities;
        public static GameLogic logic;
        public static FileIOManager fiom;

        // FPS & GSPS calulation
        public static long lCalculatedFPS = 0;
        public static long lCalculatedGSPS = 0;
        public static Stopwatch swGfxWatchdog = new Stopwatch();
        public static Stopwatch swGSPS = new Stopwatch();
        public static int iGSCount = 0;

        // Graphics stuff requiring graphics.cs-external variables
        public static bool bWndSD = false;
        public static bool bGfxFocus = false;
        public static bool bGfxResetRequired = false;

        // Game logic stuff requiring gamelogic.cs-external variables
        public static bool bCPUUnload = true;

        // Development variables - this stuff should NOT make it in to final build
        #if DEBUG
            public static string TEMP1 = "";
        #endif

        #endregion

        // Program entry point
        [STAThread]
        static void Main()
        {
            utilities = new Util();
            if (!utilities.CValidate())
            {
                MessageBox.Show("Fail!");
                Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            settings = new GameSettingsManager();
            settings.LoadGameSettings();

            logic = new GameLogic();
            fiom = new FileIOManager();

            //TEMP_SetGraphicsSettings();

            Thread GLogic = new Thread(new ThreadStart(logic.MainLoop));
            GLogic.Start();

            Thread GfxThread = new Thread(new ThreadStart(GfxThreadLaunch));
            GfxThread.Start();

            while (!bWndSD)
            {
                utilities.CalculateGSPS();
                Thread.Sleep(1);
            }
        }

        [STAThread]
        private static void GfxThreadLaunch()
        {
            swGfxWatchdog.Start();
            Application.Run(new GraphicsControl());
        }

        private static void TEMP_SetGraphicsSettings()
        {
            settings.SetSetting("Gfx_FullScreen", false);
            settings.SetSetting("Gfx_ResX", 1000);
            settings.SetSetting("Gfx_ResY", 500);
            settings.SetSetting("Gfx_CDepth", 32);
            settings.SetSetting("Gfx_PosX", 100);
            settings.SetSetting("Gfx_PosY", 150);
            settings.SaveGameSettings(true, false);
        }
    }

    public class Util
    {
        // [WIP] Application self validation via checksum of executable.
        // TO DO:  This may need error handling for permissions based issues with reading the file.
        public bool CValidate()
        {
            string result, hash;

            using (FileStream stream = File.OpenRead(Application.ExecutablePath))
            {
                SHA256Managed sha = new SHA256Managed();
                StreamReader sr = new StreamReader(stream);
                string instr = sr.ReadToEnd();
                hash = instr.Substring(instr.Length - 64);
                instr = instr.Substring(0, instr.Length - 64);
                sr.Close();
                sr.Dispose();

                byte[] moou = System.Text.Encoding.ASCII.GetBytes(instr);
                byte[] chks = sha.ComputeHash(moou);
                result = BitConverter.ToString(chks).Replace("-", string.Empty);
            }

#           if DEBUG
                return true;
#           else
                return (hash == EncStr(result).Substring(0, 64));
#           endif
        }

        // [DONE] String encryptor
        public string EncStr(string sRaw, bool bIncludeExtra = false, int iCombo = 0)
        {
            string sWorking = null;
            string sK = EnCombo(iCombo);

            try
            {
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(sK));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateEncryptor();
                byte[] input = Encoding.Unicode.GetBytes(sRaw);

                sWorking = Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
                if (bIncludeExtra)
                    sWorking = sK + sWorking;
            }
            catch
            {
                sWorking = null;
            }

            return sWorking;
        }

        // [DONE] String decryptor
        public string DecStr(string sRaw, int iCombo = 0)
        {
            string sWorking = null;
            string sK = EnCombo(iCombo);

            try
            {
                byte[] b = Convert.FromBase64String(sRaw);
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(sK));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateDecryptor();
                byte[] output = ct.TransformFinalBlock(b, 0, b.Length);

                sWorking = Encoding.Unicode.GetString(output);
            }
            catch
            {
                sWorking = null;
            }

            return sWorking;
        }

        // [DONE] Byte decryptor
        public byte[] DecByte(byte[] bRaw, int iCombo = 0)
        {
            string sK = EnCombo(iCombo); 
            byte[] output = null; ;

            try
            {
                MD5 mdmod = new MD5CryptoServiceProvider();
                TripleDES des = new TripleDESCryptoServiceProvider();
                des.Key = mdmod.ComputeHash(Encoding.Unicode.GetBytes(sK));
                des.IV = new byte[des.BlockSize / 8];

                ICryptoTransform ct = des.CreateDecryptor();
                output = ct.TransformFinalBlock(bRaw, 0, bRaw.Length);
            }
            catch
            {
                output = null;
            }

            return output;
        }

        // [WIP] Encryption key combinations - must match those found in devtool or shit goes south
        private string EnCombo(int iCombo)
        {
            switch (iCombo)
            {
                case 1:
                    return rRes.sEnc6 + rRes.sEnc1;
                default: //0
                    return rRes.sEnc4 + rRes.sEnc3;
            }
        }
        
        // [DONE] Game cycles per second calutator
        public void CalculateGSPS()
        {
            if (core.swGSPS.IsRunning)
            {
                long elapsed = core.swGSPS.ElapsedMilliseconds;
                if (elapsed > 1000)
                {
                    core.lCalculatedGSPS = core.iGSCount * elapsed / 1000;
                    core.iGSCount = 0;
                    core.swGSPS.Reset();
                    core.swGSPS.Start();
                }

            }
            else
            {
                core.swGSPS.Start();
            }
        }

        // [WIP] Global error handler method
        public void ErrorHandler(Exception e)
        {
            throw e;
            //Application.Exit();
        }
    }
}
