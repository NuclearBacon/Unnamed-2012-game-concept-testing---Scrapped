using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using DI = Microsoft.DirectX.DirectInput;
using System.Reflection;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.IO;


namespace dasspiel
{
    public class GameLogic
    {
        
        // Variable declaration
        #region Vardec
        //==================================================================================================================================
        private Stopwatch swThrottle = new Stopwatch();
        public MManager MouseData = new MManager();
        public List<UIElement> UIEles = new List<UIElement>();
        
        // Keyboard input control
        DI.Device diKB;
        private bool[] bKeyHeld = new bool[256];
        private int[] iKeyMod = new int[256];
        private Key[] keyIntToDIK = new Key[256];
        
        // Console stuff
        public bool bConDisplay = false;
        public int iDebugTextMode = 2;
        public bool bConEnabled;
        private int iConKey;
        public string sConInput = "";
        private string[] sConCmdHist = new string[20];
        public string[] sConOutput = new string[200];
        public int iConOutputEntries = 0;
        public int iConCmdHistPos = -1;
        public int iConOutputScrollPos = 0;

        //==================================================================================================================================
        #endregion

        // Class contructor
        public GameLogic()
        {
            // sets up direct input keyboard monitor
            diKB = new DI.Device(SystemGuid.Keyboard);
            diKB.SetCooperativeLevel(null, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            diKB.Acquire();
            InitDIKeyRefs();
            
            // sets default keystate tracking
            for (int i = 0; i < 256; i++)
            {
                bKeyHeld[i] = false;
                iKeyMod[i] = 0;
            }

            // console stuff
            bConEnabled = Convert.ToBoolean(core.settings.GetSetting("EnableConsole", false));  // determines if the console is enabled
            iConKey = Convert.ToInt32(core.settings.GetSetting("Bind_Console", 41));  // loads custom assigned key or set default tilde key
            for (int i = 0; i <= sConCmdHist.GetUpperBound(0); i++)
                sConCmdHist[i] = "";
            for (int i = 0; i <= sConOutput.GetUpperBound(0); i++)
                sConOutput[i] = "";
                //AppendConsoleHistory("DEBUG LINE " + i.ToString());
            
        }


        // Scans DirectInput for key states and processes pressed keys
        private void ReadKeyboard()
        {
            // Gets current key state
            KeyboardState keys = diKB.GetCurrentKeyboardState();
            
            // Gets current mod key status
            bool bShift = keys[Key.LeftShift] | keys[Key.RightShift];
            bool bCtrl = keys[Key.LeftControl] | keys[Key.RightControl];
            bool bAlt = keys[Key.LeftAlt] | keys[Key.RightAlt];

            

            // Process key states
            for (int i = 0; i < 256; i++)
                if (keys[keyIntToDIK[i]] && core.bGfxFocus)
                {
                    if ((i == iConKey) && bConEnabled && !bKeyHeld[i])  // Check for console toggle
                        bConDisplay = !bConDisplay;
                    else if (bConDisplay && !bKeyHeld[i])  // console window is open, direct keystrokes there
                    #region processing console keystrokes
                    {
                        if ((keyIntToDIK[i] == Key.Escape) && (sConInput.Length > 0))  //escape -> clear entry
                        {
                            sConInput = "";
                            iConCmdHistPos = -1;
                        }
                        else if ((keyIntToDIK[i] == Key.BackSpace) && (sConInput.Length > 0))  // backspace -> remove last character
                        {
                            sConInput = sConInput.Substring(0, sConInput.Length - 1);
                        }
                        else if ((keyIntToDIK[i] == Key.Return) && (sConInput.Length > 0))  // enter -> process current text
                        {
                            ProcessConsoleCommand(sConInput);
                            sConInput = "";
                            iConCmdHistPos = -1;
                        }
                        else if (keyIntToDIK[i] == Key.Up)  // scrolling up through console command history
                        {
                            if (iConCmdHistPos <= sConCmdHist.GetUpperBound(0))
                            {
                                if (sConCmdHist[iConCmdHistPos + 1] != "")
                                {
                                    ++iConCmdHistPos;
                                    sConInput = sConCmdHist[iConCmdHistPos];
                                }
                                else if (iConCmdHistPos > 0)
                                {
                                    iConCmdHistPos = 0;
                                    sConInput = sConCmdHist[iConCmdHistPos];
                                }
                            }
                            else
                            {
                                iConCmdHistPos = 0;
                                sConInput = sConCmdHist[iConCmdHistPos];
                            }
                        }
                        else if (keyIntToDIK[i] == Key.Down)  // scrolling down through console command history
                        {
                            if (iConCmdHistPos <= 0)
                            {
                                for (int x = sConCmdHist.GetUpperBound(0); x > 0; --x)
                                    if (sConCmdHist[x] != "")
                                    {
                                        iConCmdHistPos = x;
                                        sConInput = sConCmdHist[iConCmdHistPos];
                                        x = 0;
                                    }
                            }
                            else if (iConCmdHistPos > 0)
                            {
                                --iConCmdHistPos;
                                sConInput = sConCmdHist[iConCmdHistPos];
                            }
                        }
                        else if (keyIntToDIK[i] == Key.PageUp) // scrolling up through console output history
                        {
                            iConOutputScrollPos++;
                        }
                        else if ((keyIntToDIK[i] == Key.PageDown) && (iConOutputScrollPos > 0)) // scrolling down through console output history
                        {
                            iConOutputScrollPos--;
                        }
                        else  // it's some other key, maybe printable
                        {
                            sConInput += DIKToAscii(keyIntToDIK[i], bShift);
                        }
                    }
                    #endregion
                    else
                    {
                        // TODO: add code for when a key is pressed outside of the console here
                    }
                }

            // Update held flags and key repeat codes
            for (int i = 0; i < 256; i++)
                if (keys[keyIntToDIK[i]] && core.bGfxFocus)
                {
                    if (!bKeyHeld[i])
                        bKeyHeld[i] = true;
                }
                else 
                {
                    if (bKeyHeld[i])
                        bKeyHeld[i] = false;
                }
        }

        // Console specific stuff
        private void ProcessConsoleCommand(string sInput)
        {
            string sWorking = sInput.ToLower();
            string sArgs = "";
            string sValidCmd = "";
            bool bValidParse = false;
            bool bInvokeFail = false;

            for (int i = 0; i <= core.settings.CoreSets.strCCmd.GetUpperBound(0); i++)
            {
                sValidCmd = core.settings.CoreSets.strCCmd[i];

                if ((sValidCmd != null) && sWorking.StartsWith(sValidCmd) && (sValidCmd != ""))
                {
                    if (sWorking == sValidCmd) // valid command found, exact match, no arguments passed with command
                    {
                        AppendConsoleHistory(sInput);
                        AppendCommandHistory(sInput);
                        bValidParse = true;
                    }
                    else if (sWorking.Substring(0, sValidCmd.Length + 1) == sValidCmd + " ") // valid command with extra afterwards, possibly args
                    {
                        if (sWorking.Length > (sValidCmd.Length + 1))
                            sArgs = sInput.Substring(sValidCmd.Length + 1).Trim();
                        AppendConsoleHistory(sValidCmd + " " + sArgs);
                        AppendCommandHistory(sValidCmd + " " + sArgs);
                        bValidParse = true;
                    }
                }

                if (bValidParse)
                {
                    if (sArgs == "?")
                        CmdUsage(i);
                    else
                    {
                        MethodInfo mi = this.GetType().GetMethod("CCmd" + i.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
                        try
                        {
                            string[] d = ParseArgs(sArgs);
                            mi.Invoke(this, new object[] { d });
                        }
                        catch (Exception e)
                        {
                            throw e;
                            //bInvokeFail = true;
                        }
                    }
                    break; // exit for loop
                }
            }

            if (!bValidParse || bInvokeFail)
            {
                if (!bInvokeFail)
                {
                    AppendConsoleHistory(sInput);
                    AppendCommandHistory(sInput);
                }
                AppendConsoleHistory(core.settings.CoreSets.strGenericParseFail[0]);
            }
        }
        private string[] ParseArgs(string sArgs)
        {
            List<string> sWorking = new List<string>();
            int iIndex = 0;
            int iQ1 = -1;
            int iQ2 = -1;
            for (int a = 0; a < sArgs.Length; a++)
            {
                if ((sArgs.Substring(a, 1) == "\"") && (iQ1 == -1) && (iQ2 == -1))
                    iQ1 = a;
                else if ((sArgs.Substring(a, 1) == "\"") && (iQ1 != -1) && (iQ2 == -1))
                {
                    sWorking.Add(sArgs.Substring(iQ1 + 1, (a - iQ1) - 1));
                    iQ2 = a;
                }
                else if (sArgs.Substring(a, 1) == ",")
                {
                    if (iQ2 != -1)
                    {
                        iQ1 = -1;
                        iQ2 = -1;
                        iIndex = a + 1;
                    }
                    else if (iQ1 == -1)
                    {
                        sWorking.Add(sArgs.Substring(iIndex, a - iIndex).Trim());
                        iIndex = a + 1;
                    }
                }
                else if (a == (sArgs.Length - 1))
                {
                    if (iQ1 == -1)
                        sWorking.Add(sArgs.Substring(iIndex).Trim());
                }
            }

            if (sWorking.Count > 0)
            {
                string[] sOut = new string[sWorking.Count];
                for (int i = 0; i < sWorking.Count; i++)
                    sOut[i] = sWorking[i];

                return sOut;
            }
            else
                return null;
        }
        private void AppendCommandHistory(string sNewLine)
        {
            for (int i = sConCmdHist.GetUpperBound(0); i > 0; --i)
                sConCmdHist[i] = sConCmdHist[i - 1];
            sConCmdHist[0] = sNewLine;
        }
        private void AppendConsoleHistory(string sNewLine)
        {
            for (int i = sConOutput.GetUpperBound(0); i > 0; --i)
                sConOutput[i] = sConOutput[i - 1];
            sConOutput[0] = sNewLine;
            iConOutputEntries++;
            if (iConOutputEntries > sConOutput.GetUpperBound(0) + 1)
                iConOutputEntries = sConOutput.GetUpperBound(0) + 1;
            core.logic.iConOutputScrollPos = 0;
        }
        private void ClearConsoleHistory()
        {
            for (int i = 0; i <= sConOutput.GetUpperBound(0); i++)
                sConOutput[i] = "";
            iConOutputEntries = 0;
            iConOutputScrollPos = 0;
        }
        private void CmdUsage(int iCmdID)
        {
            for (int i = 0; i <= core.settings.CoreSets.strUsage.GetUpperBound(1); i++)
            {
                if (core.settings.CoreSets.strUsage[iCmdID, i] != "")
                    AppendConsoleHistory(core.settings.CoreSets.strUsage[iCmdID, i]);
            }
        }
        
        private void CCmd0(string[] sArgs)    // exit
        {
            if ((sArgs != null) && (sArgs.GetUpperBound(0) >= 0))  // >0 arguments passed
            {
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[1], core.settings.CoreSets.strCCmd[0], 0));
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[0]));
            }
            else
                Application.Exit();
        }
        private void CCmd1(string[] sArgs)    // tdt
        {
            if ((sArgs == null) || (sArgs.GetUpperBound(0) > 0))  // 0 or >1 arguments passed
            {
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[1], core.settings.CoreSets.strCCmd[1], 1));
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[1]));
            }
            else
            {
                int iPage;
                try  // change the string to an int
                {
                    iPage = Convert.ToInt32(sArgs[0]);
                }
                catch  // non-integer text for arguments
                {
                    iPage = -1;
                }

                switch (iPage)  // check page number and update if it exists
                {
                    case 0:
                    case 1:
                    case 2:
                        iDebugTextMode = iPage;
                        break;
                    default:
                        AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[2], core.settings.CoreSets.strCCmd[1], sArgs[0]));
                        AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[1]));
                        break;
                }
            }
        }
        private void CCmd2(string[] sArgs)    // spam
        {
            for (int a = 1; a <= 200; a++)
                AppendConsoleHistory("Console spam line #" + a.ToString());
        }
        private void CCmd3(string[] sArgs)    // cls
        {
            ClearConsoleHistory();
        }
        private void CCmd4(string[] sArgs)    // getsetting
        {
            if ((sArgs == null) || (sArgs.GetUpperBound(0) > 0))  // 0 or >1 arguments passed
            {
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[1], core.settings.CoreSets.strCCmd[4], 1));
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[4]));
            }
            else
            {
                object oFetch = core.settings.GetSetting(sArgs[0]);
                if (oFetch == null)
                    AppendConsoleHistory(string.Format(core.settings.CoreSets.strCOutput[4,0], sArgs));
                else
                    AppendConsoleHistory(string.Format("{0}={1}", sArgs[0], oFetch.ToString()));
            }
        }
        private void CCmd5(string[] sArgs)    // setsetting
        {
            if ((sArgs == null) || (sArgs.GetUpperBound(0) != 1))  // looking for 2 arguments
            {
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[1], core.settings.CoreSets.strCCmd[5], 2));
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[5]));
            }
            else
            {
                // get the current setting to check existance and type comparison
                object obSetting = core.settings.GetSetting(sArgs[0]);
                object obNewSet = null;

                // check if setting actually exists
                if (obSetting == null)
                    AppendConsoleHistory(string.Format(core.settings.CoreSets.strCOutput[4,0], core.settings.CoreSets.strCCmd[5]));

                // check is setting is protected from editing
                else if (core.settings.IsProtected(sArgs[0]))
                    AppendConsoleHistory(string.Format(core.settings.CoreSets.strCOutput[5,0], sArgs[0]));

                // udpate setting
                else
                {
                    // see if the new value can be cast to the correct System.Type for this setting
                    Type tActual = obSetting.GetType();
                    try
                    {
                        if (tActual == typeof(string))
                            obNewSet = sArgs[1];
                        else if (tActual == typeof(int))
                            obNewSet = int.Parse(sArgs[1]);
                        else if (tActual == typeof(bool))
                            obNewSet = bool.Parse(sArgs[1]);
                        else if (tActual == typeof(Double))
                            obNewSet = Double.Parse(sArgs[1]);
                        else
                            throw new Exception(); // unhandled type, trip the catch

                        core.settings.SetSetting(sArgs[0], obNewSet);
                        AppendConsoleHistory(string.Format("{0}->{1}", sArgs[0], core.settings.GetSetting(sArgs[0])));
                    }
                    catch
                    {
                        AppendConsoleHistory(string.Format(core.settings.CoreSets.strCOutput[5, 1], sArgs[1], tActual.ToString()));
                    }

                    
                }
            }
        }
        private void CCmd6(string[] sArgs)    // rgfx
        {
            if ((sArgs != null) && (sArgs.GetUpperBound(0) >= 0))  // >0 arguments passed
            {
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[1], core.settings.CoreSets.strCCmd[6], 0));
                AppendConsoleHistory(string.Format(core.settings.CoreSets.strGenericParseFail[3], core.settings.CoreSets.strCCmd[6]));
            }
            else
                core.bGfxResetRequired = true;
        }
        private void CCmd7(string[] sArgs)    // unused
        {
            AppendConsoleHistory("You need to add the code for this command.");
        }
        private void CCmd8(string[] sArgs)    // unused
        {
            AppendConsoleHistory("You need to add the code for this command.");
        }
        private void CCmd9(string[] sArgs)    // unused
        {
            AppendConsoleHistory("You need to add the code for this command.");
        }


        // The main game logic loop.  This runs independently of the main screen renderer.
        [STAThread]
        public void MainLoop()
        {
            swThrottle.Start();
            while (!core.bWndSD)
            {

                if (swThrottle.ElapsedTicks > (Stopwatch.Frequency / 1000))
                {
                    swThrottle.Reset();
                    swThrottle.Start();

                    ReadKeyboard();
                    core.iGSCount++;
                }
                else if (core.bCPUUnload)
                    Thread.Sleep(1);
            }

            // The main display has closed, time to clean up and shut down
            diKB.Unacquire();
            diKB.Dispose();
        }

        /// <summary>
        /// Contains data relating to mouse status that can be shared between application threads.
        /// Processes mouse events passed by the graphics module.
        /// </summary>
        public class MManager
        {
            private int _iMX;
            private int _iMY;
            private bool[] _bMD = new bool[5];
            
            // Contructor
            public MManager()
            {
                _iMX = Cursor.Position.X;
                _iMY = Cursor.Position.Y;
                for (int i = 0; i < _bMD.GetUpperBound(0); i++)
                    _bMD[i] = false;
            }

            // _iMX accessor
            public int iMouseX
            {
                get { return _iMX; }
                set { _iMX = value; MouseMoved(); }
            }
            public int iXFromCenter = 0;

            // _iMY access
            public int iMouseY
            {
                get { return _iMY; }
                set { _iMY = value; MouseMoved(); }
            }
            public int iYFromCenter = 0;

            private void MouseMoved()
            {
                Point pCenter = new Point(1920 / 2, 1080 / 2);
                iXFromCenter = pCenter.X - _iMX;
                iYFromCenter = pCenter.Y - _iMY;
            }

            public bool GetButtonDown(MouseButtons mb)
            {
                switch (mb)
                {
                    case MouseButtons.Left:
                        return _bMD[0];
                    case MouseButtons.Right:
                        return _bMD[1];
                    case MouseButtons.Middle:
                        return _bMD[2];
                    case MouseButtons.XButton1:
                        return _bMD[3];
                    case MouseButtons.XButton2:
                        return _bMD[4];
                    default:
                        return false;
                }
            }
            public void SetButtonDown(MouseButtons mb, bool bIsDown)
            {
                switch (mb)
                {
                    case MouseButtons.Left:
                        _bMD[0] = bIsDown;
                        break;
                    case MouseButtons.Right:
                        _bMD[1] = bIsDown;
                        break;
                    case MouseButtons.Middle:
                        _bMD[2] = bIsDown;
                        break;
                    case MouseButtons.XButton1:
                        _bMD[3] = bIsDown;
                        break;
                    case MouseButtons.XButton2:
                        _bMD[4] = bIsDown;
                        break;
                    default:
                        break;
                }
            }
            public void Click(MouseEventArgs e)
            {
                
                // BULLSHIT TESTING CODE /////////////////////////////////////////////////////////////////////////////
                if (core.logic.UIEles.Count == 0)
                {
                    core.logic.UIEles.Add(new UIElement("UITest", "UI\\options.uie"));
                    int xX = (Screen.PrimaryScreen.WorkingArea.Width - core.logic.UIEles[0].rectBounds.Width) / 2;
                    int yY = (Screen.PrimaryScreen.WorkingArea.Height - core.logic.UIEles[0].rectBounds.Height) / 2;
                    core.logic.UIEles[0].SetXY(xX, yY);
                }
                // BULLSHIT TESTING CODE /////////////////////////////////////////////////////////////////////////////

            }
            public void DClick(MouseEventArgs e)
            {
                
            }
        }

        public class UIElement
        {
            // internal name of UI element - may be useful in finding other copies of element
            public string Name;
            
            // index of UI element calling this form
            public int iParentIndex;

            // represents the outer limits of the element for screen placement purposes (makes it easier with non-rectangular elements)
            public Rectangle rectBounds;

            // contains the vertext data needed to draw the frame
            public CustomVertex.TransformedColored[] vFrame;
            
            // contains text management elements
            public string[] sTxt;
            public Point[] ptTxt;
            public string[] sTag;
            public int[] iTxtColor;
            public int[] iFntIndex;

            // class contructor
            public UIElement(string sArchive = "", string sElement = "")
            {
                if (sArchive != "")
                    LoadPrefabElement(sArchive, sElement);
            }

            public void LoadPrefabElement(string sArchive, string sElement)
            {
                char cDelim = (char)0;
                byte[] bRaw = (byte[])core.fiom.LoadGenericResourceFromArchive(sArchive, sElement);
                string sRaw = "";
                for (int a = 0; a < bRaw.Length; a++)
                    sRaw += (char)(int)bRaw[a];
                string[] sCSV = sRaw.Split(cDelim);
                
                // sets the element's bounding rectangle
                rectBounds.Width = Convert.ToInt32(sCSV[0]);
                rectBounds.Height = Convert.ToInt32(sCSV[1]);

                // creates the color palette array
                int iNumColors = Convert.ToInt32(sCSV[2]);
                int[] iColorData = new int[iNumColors];

                // loads color data
                for (int c = 0; c < iNumColors; c++)
                {
                    int iA = Convert.ToInt32(sCSV[3 + (c * 4)]);
                    int iR = Convert.ToInt32(sCSV[4 + (c * 4)]);
                    int iG = Convert.ToInt32(sCSV[5 + (c * 4)]);
                    int iB = Convert.ToInt32(sCSV[6 + (c * 4)]);
                    iColorData[c] = Color.FromArgb(iA, iR, iG, iB).ToArgb();
                }

                // loads vertex data and dumps it directly in to the triangle list
                int iVDatOffset = 3 + (4 * iNumColors);
                int iNumVerts = Convert.ToInt32(sCSV[iVDatOffset]);
                if (iNumVerts > 0)
                {
                    vFrame = new CustomVertex.TransformedColored[iNumVerts];
                    for (int v = 0; v < iNumVerts; v++)
                    {
                        int iX = Convert.ToInt32(sCSV[iVDatOffset + 1 + (v * 3)]);
                        int iY = Convert.ToInt32(sCSV[iVDatOffset + 2 + (v * 3)]);
                        int iCIndex = Convert.ToInt32(sCSV[iVDatOffset + 3 + (v * 3)]);

                        vFrame[v].Position = new Vector4((float)iX, (float)iY, 0f, 1f);
                        vFrame[v].Color = iColorData[iCIndex];
                    }
                }

                // load text object data
                int iTxtOffset = iVDatOffset + (3 * iNumVerts) + 1;
                int iNumTxt = Convert.ToInt32(sCSV[iTxtOffset]);
                if (iNumTxt > 0)
                {
                    sTxt = new string[iNumTxt];
                    ptTxt = new Point[iNumTxt];
                    sTag = new string[iNumTxt];
                    iTxtColor = new int[iNumTxt];
                    iFntIndex = new int[iNumTxt];

                    for (int t = 0; t < iNumTxt; t++)
                    {
                        ptTxt[t].X = Convert.ToInt32(sCSV[iTxtOffset + 1 + (t * 6)]);
                        ptTxt[t].Y = Convert.ToInt32(sCSV[iTxtOffset + 2 + (t * 6)]);
                        iTxtColor[t] = iColorData[Convert.ToInt32(sCSV[iTxtOffset + 3 + (t * 6)])];
                        iFntIndex[t] = Convert.ToInt32(sCSV[iTxtOffset + 4 + (t * 6)]);
                        sTag[t] = sCSV[iTxtOffset + 5 + (t * 6)];
                        sTxt[t] = sCSV[iTxtOffset + 6 + (t * 6)];
                    }
                }

            }

            /// <summary>
            /// Populates the UIElement's vFrame variable with vertex data.
            /// </summary>
            /// <param name="vVerts">An array of Vertex4 elements to be used in a triangle. Count must be a multiple of 3.</param>
            /// <param name="iColor">An integer representation of the desired color for the vertex of matching index. Using .ToArgb() useful.</param>
            public void SetVertecies(Vector4[] vVerts, int[] iColor)
            {
                vFrame = new CustomVertex.TransformedColored[vVerts.GetUpperBound(0) + 1];
                for (int i = 0; i <= vFrame.GetUpperBound(0); i++)
                {
                    vFrame[i].Position = vVerts[i];
                    vFrame[i].Color = iColor[i]; 
                }
                
            }
            public void SetXY(int iX, int iY)
            {
                int iModX = iX - rectBounds.X;
                int iModY = iY - rectBounds.Y;
                rectBounds.X = iX;
                rectBounds.Y = iY;

                if (vFrame != null)
                    for (int a = 0; a <= vFrame.GetUpperBound(0); a++)
                    {
                        Vector4 v = vFrame[a].Position;
                        v.X += iModX;
                        v.Y += iModY;
                        vFrame[a].Position = v;
                    }

                if (ptTxt != null)
                    for (int a = 0; a <= ptTxt.GetUpperBound(0); a++)
                    {
                        ptTxt[a].X += iModX;
                        ptTxt[a].Y += iModY;
                    }
            }
        }

        // Translates the DirectInput "Key" enum back in to numbers.  
        // Used to prevent having to manully enter each key enum during full keyboard scans.
        private void InitDIKeyRefs()
        {
            keyIntToDIK[1] = Key.Escape;
            keyIntToDIK[2] = Key.D1;
            keyIntToDIK[3] = Key.D2;
            keyIntToDIK[4] = Key.D3;
            keyIntToDIK[5] = Key.D4;
            keyIntToDIK[6] = Key.D5;
            keyIntToDIK[7] = Key.D6;
            keyIntToDIK[8] = Key.D7;
            keyIntToDIK[9] = Key.D8;
            keyIntToDIK[10] = Key.D9;
            keyIntToDIK[11] = Key.D0;
            keyIntToDIK[12] = Key.Minus;
            keyIntToDIK[13] = Key.Equals;
            keyIntToDIK[14] = Key.BackSpace;
            keyIntToDIK[14] = Key.Back;
            keyIntToDIK[15] = Key.Tab;
            keyIntToDIK[16] = Key.Q;
            keyIntToDIK[17] = Key.W;
            keyIntToDIK[18] = Key.E;
            keyIntToDIK[19] = Key.R;
            keyIntToDIK[20] = Key.T;
            keyIntToDIK[21] = Key.Y;
            keyIntToDIK[22] = Key.U;
            keyIntToDIK[23] = Key.I;
            keyIntToDIK[24] = Key.O;
            keyIntToDIK[25] = Key.P;
            keyIntToDIK[26] = Key.LeftBracket;
            keyIntToDIK[27] = Key.RightBracket;
            keyIntToDIK[28] = Key.Return;
            keyIntToDIK[29] = Key.LeftControl;
            keyIntToDIK[30] = Key.A;
            keyIntToDIK[31] = Key.S;
            keyIntToDIK[32] = Key.D;
            keyIntToDIK[33] = Key.F;
            keyIntToDIK[34] = Key.G;
            keyIntToDIK[35] = Key.H;
            keyIntToDIK[36] = Key.J;
            keyIntToDIK[37] = Key.K;
            keyIntToDIK[38] = Key.L;
            keyIntToDIK[39] = Key.SemiColon;
            keyIntToDIK[40] = Key.Apostrophe;
            keyIntToDIK[41] = Key.Grave;
            keyIntToDIK[42] = Key.LeftShift;
            keyIntToDIK[43] = Key.BackSlash;
            keyIntToDIK[44] = Key.Z;
            keyIntToDIK[45] = Key.X;
            keyIntToDIK[46] = Key.C;
            keyIntToDIK[47] = Key.V;
            keyIntToDIK[48] = Key.B;
            keyIntToDIK[49] = Key.N;
            keyIntToDIK[50] = Key.M;
            keyIntToDIK[51] = Key.Comma;
            keyIntToDIK[52] = Key.Period;
            keyIntToDIK[53] = Key.Slash;
            keyIntToDIK[54] = Key.RightShift;
            keyIntToDIK[55] = Key.NumPadStar;
            keyIntToDIK[55] = Key.Multiply;
            keyIntToDIK[56] = Key.LeftMenu;
            keyIntToDIK[56] = Key.LeftAlt;
            keyIntToDIK[57] = Key.Space;
            keyIntToDIK[58] = Key.Capital;
            keyIntToDIK[58] = Key.CapsLock;
            keyIntToDIK[59] = Key.F1;
            keyIntToDIK[60] = Key.F2;
            keyIntToDIK[61] = Key.F3;
            keyIntToDIK[62] = Key.F4;
            keyIntToDIK[63] = Key.F5;
            keyIntToDIK[64] = Key.F6;
            keyIntToDIK[65] = Key.F7;
            keyIntToDIK[66] = Key.F8;
            keyIntToDIK[67] = Key.F9;
            keyIntToDIK[68] = Key.F10;
            keyIntToDIK[69] = Key.Numlock;
            keyIntToDIK[70] = Key.Scroll;
            keyIntToDIK[71] = Key.NumPad7;
            keyIntToDIK[72] = Key.NumPad8;
            keyIntToDIK[73] = Key.NumPad9;
            keyIntToDIK[74] = Key.NumPadMinus;
            keyIntToDIK[74] = Key.Subtract;
            keyIntToDIK[75] = Key.NumPad4;
            keyIntToDIK[76] = Key.NumPad5;
            keyIntToDIK[77] = Key.NumPad6;
            keyIntToDIK[78] = Key.NumPadPlus;
            keyIntToDIK[78] = Key.Add;
            keyIntToDIK[79] = Key.NumPad1;
            keyIntToDIK[80] = Key.NumPad2;
            keyIntToDIK[81] = Key.NumPad3;
            keyIntToDIK[82] = Key.NumPad0;
            keyIntToDIK[83] = Key.NumPadPeriod;
            keyIntToDIK[83] = Key.Decimal;
            keyIntToDIK[86] = Key.OEM102;
            keyIntToDIK[87] = Key.F11;
            keyIntToDIK[88] = Key.F12;
            keyIntToDIK[100] = Key.F13;
            keyIntToDIK[101] = Key.F14;
            keyIntToDIK[102] = Key.F15;
            keyIntToDIK[112] = Key.Kana;
            keyIntToDIK[115] = Key.AbntC1;
            keyIntToDIK[121] = Key.Convert;
            keyIntToDIK[123] = Key.NoConvert;
            keyIntToDIK[125] = Key.Yen;
            keyIntToDIK[126] = Key.AbntC2;
            keyIntToDIK[141] = Key.NumPadEquals;
            keyIntToDIK[144] = Key.Circumflex;
            keyIntToDIK[144] = Key.PrevTrack;
            keyIntToDIK[145] = Key.At;
            keyIntToDIK[146] = Key.Colon;
            keyIntToDIK[147] = Key.Underline;
            keyIntToDIK[148] = Key.Kanji;
            keyIntToDIK[149] = Key.Stop;
            keyIntToDIK[150] = Key.AX;
            keyIntToDIK[151] = Key.Unlabeled;
            keyIntToDIK[153] = Key.NextTrack;
            keyIntToDIK[156] = Key.NumPadEnter;
            keyIntToDIK[157] = Key.RightControl;
            keyIntToDIK[160] = Key.Mute;
            keyIntToDIK[161] = Key.Calculator;
            keyIntToDIK[162] = Key.PlayPause;
            keyIntToDIK[164] = Key.MediaStop;
            keyIntToDIK[174] = Key.VolumeDown;
            keyIntToDIK[176] = Key.VolumeUp;
            keyIntToDIK[178] = Key.WebHome;
            keyIntToDIK[179] = Key.NumPadComma;
            keyIntToDIK[181] = Key.Divide;
            keyIntToDIK[181] = Key.NumPadSlash;
            keyIntToDIK[183] = Key.SysRq;
            keyIntToDIK[184] = Key.RightMenu;
            keyIntToDIK[184] = Key.RightAlt;
            keyIntToDIK[197] = Key.Pause;
            keyIntToDIK[199] = Key.Home;
            keyIntToDIK[200] = Key.UpArrow;
            keyIntToDIK[200] = Key.Up;
            keyIntToDIK[201] = Key.Prior;
            keyIntToDIK[201] = Key.PageUp;
            keyIntToDIK[203] = Key.Left;
            keyIntToDIK[203] = Key.LeftArrow;
            keyIntToDIK[205] = Key.Right;
            keyIntToDIK[205] = Key.RightArrow;
            keyIntToDIK[207] = Key.End;
            keyIntToDIK[208] = Key.DownArrow;
            keyIntToDIK[208] = Key.Down;
            keyIntToDIK[209] = Key.Next;
            keyIntToDIK[209] = Key.PageDown;
            keyIntToDIK[210] = Key.Insert;
            keyIntToDIK[211] = Key.Delete;
            keyIntToDIK[219] = Key.LeftWindows;
            keyIntToDIK[220] = Key.RightWindows;
            keyIntToDIK[221] = Key.Apps;
            keyIntToDIK[222] = Key.Power;
            keyIntToDIK[223] = Key.Sleep;
            keyIntToDIK[227] = Key.Wake;
            keyIntToDIK[229] = Key.WebSearch;
            keyIntToDIK[230] = Key.WebFavorites;
            keyIntToDIK[231] = Key.WebRefresh;
            keyIntToDIK[232] = Key.WebStop;
            keyIntToDIK[233] = Key.WebForward;
            keyIntToDIK[234] = Key.WebBack;
            keyIntToDIK[235] = Key.MyComputer;
            keyIntToDIK[236] = Key.Mail;
            keyIntToDIK[237] = Key.MediaSelect;
        }
        
        // This monstrosity changes DirectInput Key codes into a printable ascii character.
        // Returns a string instead of char to allow zerolength results for keycodes without a printable character.
        private string DIKToAscii(Key kDIK, bool bS)
        {
            string sReturnVal = "";

            if (kDIK.ToString().Length == 1) 
                // letters
            {
                sReturnVal = kDIK.ToString();
                if (bS == System.Console.CapsLock)
                    sReturnVal = sReturnVal.ToLower();
            }
            else if ((kDIK.ToString().Length == 2) && (kDIK.ToString().Substring(0, 1) == "D")) 
                // number key on row
            {
                if (!bS)  // just the number
                    sReturnVal = kDIK.ToString().Substring(1, 1);
                else      // shift-modified number.  must be done manually, result characters are not sequential :(
                    switch (kDIK)
                    {
                        case Key.D1:
                            sReturnVal = "!"; break;
                        case Key.D2:
                            sReturnVal = "@"; break;
                        case Key.D3:
                            sReturnVal = "#"; break;
                        case Key.D4:
                            sReturnVal = "$"; break;
                        case Key.D5:
                            sReturnVal = "%"; break;
                        case Key.D6:
                            sReturnVal = "^"; break;
                        case Key.D7:
                            sReturnVal = "&"; break;
                        case Key.D8:
                            sReturnVal = "*"; break;
                        case Key.D9:
                            sReturnVal = "("; break;
                        case Key.D0:
                            sReturnVal = ")"; break;
                    }
            }
            else if ((kDIK.ToString().Length == 7) && (kDIK.ToString().Substring(0, 6) == Key.NumPad0.ToString().Substring(0, 6)) && System.Console.NumberLock)
            // numpad numbers, numlock turned on
            {
                sReturnVal = kDIK.ToString().Substring(6, 1);
            }
            else
            // miscellaneous keys acceptable for printing on screen
            {
                switch (kDIK)
                {
                    case Key.Grave:
                        if (bS)
                            sReturnVal = "~";
                        else
                            sReturnVal = "`";
                        break;
                    case Key.Minus:
                        if (bS)
                            sReturnVal = "_";
                        else
                            sReturnVal = "-";
                        break;
                    case Key.Equals:
                        if (bS)
                            sReturnVal = "+";
                        else
                            sReturnVal = "=";
                        break;
                    case Key.LeftBracket:
                        if (bS)
                            sReturnVal = "{";
                        else
                            sReturnVal = "[";
                        break;
                    case Key.RightBracket:
                        if (bS)
                            sReturnVal = "}";
                        else
                            sReturnVal = "]";
                        break;
                    case Key.BackSlash:
                        if (bS)
                            sReturnVal = "|";
                        else
                            sReturnVal = "\\";
                        break;
                    case Key.SemiColon:
                        if (bS)
                            sReturnVal = ":";
                        else
                            sReturnVal = ";";
                        break;
                    case Key.Apostrophe:
                        if (bS)
                            sReturnVal = "\"";
                        else
                            sReturnVal = "'";
                        break;
                    case Key.Comma:
                        if (bS)
                            sReturnVal = "<";
                        else
                            sReturnVal = ",";
                        break;
                    case Key.Period:
                        if (bS)
                            sReturnVal = ">";
                        else
                            sReturnVal = ".";
                        break;
                    case Key.Slash:
                        if (bS)
                            sReturnVal = "?";
                        else
                            sReturnVal = "/";
                        break;
                    case Key.Space:
                        sReturnVal = " ";
                        break;
                    case Key.NumPadSlash:
                        sReturnVal = "/";
                        break;
                    case Key.NumPadStar:
                        sReturnVal = "*";
                        break;
                    case Key.NumPadMinus:
                        sReturnVal = "-";
                        break;
                    case Key.NumPadPlus:
                        sReturnVal = "+";
                        break;
                    case Key.NumPadPeriod:
                        sReturnVal = ".";
                        break;
                }
	        }

            return sReturnVal;
        }
    }
}
