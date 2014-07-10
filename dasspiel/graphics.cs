// <=========================================================================================================================>
// The 'gfx.cs' code section controls all aspects of the Direct3D interface.  Pretty straight forward.
// <=========================================================================================================================>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Diagnostics;
using System.Threading;
using System.Drawing.Text;
using System.Runtime.InteropServices;

using d3d = Microsoft.DirectX.Direct3D;


namespace dasspiel
{
    public class GraphicsControl : Form
    {
        // Variable declaration
        #region Variables
        // <=========================================================================================================================>

        // DirectX classes
        d3d.Device device;
        d3d.PresentParameters pp;

        // This is used to track a one-time pull to front method meant to correct issues 
        // where the main form is not automatically brought to the front on runtime.
        bool PulledToFront = false;

        // These settings determine the allow windowed modes.  This are hard-coded based
        // on the needs of the program being written.
        bool bSizableAllowed = false;
        bool bFSWinAllowed = true;
        bool bFSAllowed = true;

        // Used for FPS calulations
        Stopwatch swFPS = new Stopwatch();
        int iRenders = 0;

        // Render components
        private Material matDefault;
 
        // Font stuff
        private System.Drawing.Font sdfHeading;
        private FontFamily ff;
        private d3d.Font fntHeading;

        // Console/OSD stuff
        private CustomVertex.TransformedColored[] cvConsole = new CustomVertex.TransformedColored[6];
        private int iConsoleFrameColor = Color.FromArgb(195, 0, 0, 0).ToArgb();
        private float fConsoleHeight = 300;
        private d3d.Font fntDebugOSD;
        private d3d.Font fntConsole;

        // DLL imports
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        // DEVELOPMENT VARIABLES - THESE SHOULD BE REMOVABLE AT FINAL BUILD
#       if DEBUG
        private Mesh skyboxmesh;
        private Material[] skyboxmaterials;
        private Texture[] skyboxtextures;
        int iDeg = 0;
        float fXMod = 0;
        int iYMod = 0;

#       endif

        // <=========================================================================================================================>
        #endregion

        // Class contructor & disposal
        public GraphicsControl()
        {
            this.LostFocus += new EventHandler(GraphicsControl_LostFocus);
            this.GotFocus += new EventHandler(GraphicsControl_GotFocus);
            this.MouseMove += new MouseEventHandler(GraphicsControl_MouseMove);
            this.MouseDown += new MouseEventHandler(GraphicsControl_MouseDown);
            this.MouseUp += new MouseEventHandler(GraphicsControl_MouseUp);
            this.MouseClick += new MouseEventHandler(GraphicsControl_MouseClick);
            this.MouseDoubleClick += new MouseEventHandler(GraphicsControl_MouseDoubleClick);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            pp = new PresentParameters();

            LoadFonts();
            InitializeD3D();
            LoadDefaultTextures();
            SetupConsoleWindow();
            SetupUIElements();

            // Bullshit lines
            SetUpCamera();
            float dummy = 0;
            core.fiom.LoadMeshFromArchive("UITest", "Meshes\\testskybox.x", ref skyboxmesh, ref skyboxmaterials, ref skyboxtextures, ref device, ref dummy);
            //LoadMesh("skybox2.x", ref skyboxmesh, ref skyboxmaterials, ref skyboxtextures, ref dummy);
        }
        protected override void Dispose(bool disposing)
        {
            core.bWndSD = true;
            device.Dispose();
            base.Dispose(disposing);
        }
        
        // Overrides & event handlers
        protected override void OnResize(EventArgs e)
        {
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.Sizable)
                try
                {
                    SetupConsoleWindow(); // allows for the console window to be resized horizontally on a window resize
                }
                catch { }
        }
        void GraphicsControl_MouseMove(object sender, MouseEventArgs e)
        {
            core.logic.MouseData.iMouseX = e.X;
            core.logic.MouseData.iMouseY = e.Y;
        }
        void GraphicsControl_MouseDown(object sender, MouseEventArgs e)
        {
            core.logic.MouseData.SetButtonDown(e.Button, true);
        }
        void GraphicsControl_MouseUp(object sender, MouseEventArgs e)
        {
            core.logic.MouseData.SetButtonDown(e.Button, false);
        }
        void GraphicsControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            core.logic.MouseData.DClick(e);
        }
        void GraphicsControl_MouseClick(object sender, MouseEventArgs e)
        {
            core.logic.MouseData.Click(e);
        }
        void GraphicsControl_GotFocus(object sender, EventArgs e)
        {
            core.bGfxFocus = true;
        }
        void GraphicsControl_LostFocus(object sender, EventArgs e)
        {
            core.bGfxFocus = false;
        }

        // Startup crap
        private void LoadFonts()
        {
            // Create the byte array and get its length
            byte[] fontArray = dasspiel.Properties.Resources.fntrec01;
            int dataLength = dasspiel.Properties.Resources.fntrec01.Length;


            // ASSIGN MEMORY AND COPY  BYTE[] ON THAT MEMORY ADDRESS
            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontArray, 0, ptrData, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

            //PASS THE FONT TO THE  PRIVATEFONTCOLLECTION OBJECT
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(ptrData, dataLength);

            //FREE THE  "UNSAFE" MEMORY
            Marshal.FreeCoTaskMem(ptrData);

            // asigning font data to members
            ff = pfc.Families[0];
            sdfHeading = new System.Drawing.Font(ff, 24f, FontStyle.Bold);
            
            // Cleanup
            //ff.Dispose();
            //pfc.Dispose();
        }
        private void InitializeD3D()
        {
            //GetDisplayInformation();

            while (device == null)
            {
                ConfigPP();

                try
                {
                    device = new d3d.Device(Convert.ToInt32(core.settings.GetSetting("Gfx_Adapter", -1)), DeviceType.Hardware, this, d3d.CreateFlags.HardwareVertexProcessing, pp);
                }
                catch
                {
                    MessageBox.Show("Graphics mode revert!");
                    RevertToBaseGraphicsSettings();
                }
            }
        }
        private void ConfigPP()
        {
            // Fetch last used graphics settings
            int ad = Convert.ToInt32(core.settings.GetSetting("Gfx_Adapter", -1));
            int rx = Convert.ToInt32(core.settings.GetSetting("Gfx_ResX", -1));
            int ry = Convert.ToInt32(core.settings.GetSetting("Gfx_ResY", -1));
            int cd = Convert.ToInt32(core.settings.GetSetting("Gfx_CDepth", -1));
            int px = Convert.ToInt32(core.settings.GetSetting("Gfx_PosX", 50));
            int py = Convert.ToInt32(core.settings.GetSetting("Gfx_PosY", 50));
            int dm = Convert.ToInt32(core.settings.GetSetting("Gfx_DisplayMode", -1));

            // Error check critial loaded graphics settings
            if (((ad < 0) || (ad >= Manager.Adapters.Count) || (rx < 1) || (ry < 1) || ((cd != 16) && (cd != 32)) || ((dm < 0) || (dm > 2))))
            {
                RevertToBaseGraphicsSettings();
            }

            // Check loaded settings vs display modes allowed
            if ((!bFSAllowed && (dm == 3)) || (!bFSWinAllowed && (dm == 2)))
            {
                dm = 0;
                core.settings.SetSetting("Gfx_DisplayMode", 0);
            }
            if (!bSizableAllowed && (dm == 0))
            {
                dm = 1;
                core.settings.SetSetting("Gfx_DisplayMode", 1);
            }

            // Configure window
            this.Size = new Size(rx, ry);
            switch (dm)
            {
                case 0: // normal window, sizable
                    this.MaximizeBox = true;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    pp.Windowed = true;
                    this.Size = new Size(rx, ry);
                    pp.BackBufferHeight = this.Height;
                    pp.BackBufferWidth = this.Width;
                    this.Left = px;
                    this.Top = py;
                    this.MinimumSize = new Size(100, 100);
                    break;
                case 1: // normal window, fixed
                    this.MaximizeBox = false;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                    pp.Windowed = true;
                    this.Size = new Size(rx, ry);
                    pp.BackBufferHeight = this.Height;
                    pp.BackBufferWidth = this.Width;
                    this.Left = px;
                    this.Top = py;
                    break;
                case 2: // maximized fullscreen
                    this.MaximizeBox = false;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    pp.Windowed = true;
                    this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                    pp.BackBufferWidth = rx;
                    pp.BackBufferHeight = ry;
                    this.Left = 0;
                    this.Top = 0;
                    break;
                case 3: // true fullscreen
                    this.MaximizeBox = false;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    pp.Windowed = false;
                    this.Size = new Size(rx, ry);
                    pp.BackBufferHeight = this.Height;
                    pp.BackBufferWidth = this.Width;
                    break;
            }

            if (cd == 16)
                pp.BackBufferFormat = Format.R5G6B5;
            else
                pp.BackBufferFormat = Format.X8R8G8B8;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.SwapEffect = SwapEffect.Flip;
            pp.EnableAutoDepthStencil = true;
        }
        private void SetUpCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 2, (float)this.Width / (float)this.Height, 0.3f, 500f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 0), new Vector3(10, 2, -2), new Vector3(0, 0, 1));
        } 
        private void LoadDefaultTextures()
        {
            matDefault = new Material();
            matDefault.Diffuse = Color.White;
            matDefault.Ambient = Color.White;
        }
        private void SetupConsoleWindow()
        {
            fntDebugOSD = new d3d.Font(device, new System.Drawing.Font("Arial", 18f, FontStyle.Bold));
            fntConsole = new d3d.Font(device, 24, 0, FontWeight.Normal, 0, false, CharacterSet.Default, Precision.String, 
                FontQuality.ClearTypeNatural, PitchAndFamily.VariablePitch, "Ariel");

            cvConsole[0].Position = new Vector4(0f, 0f, 0f, 1f);
            cvConsole[1].Position = new Vector4(this.Width, 0f, 0f, 1f);
            cvConsole[2].Position = new Vector4(0f, fConsoleHeight, 0f, 1f);
            cvConsole[3].Position = new Vector4(this.Width, 0f, 0f, 1f);
            cvConsole[4].Position = new Vector4(this.Width, fConsoleHeight, 0f, 1f);
            cvConsole[5].Position = new Vector4(0f, fConsoleHeight, 0f, 1f);

            for (int i = 0; i <= 5; i++)
                cvConsole[i].Color = iConsoleFrameColor;
        }
        private void SetupUIElements()
        {
            //fntHeading = new d3d.Font(device, sdfHeading);
            fntHeading = new d3d.Font(device, 40, 0, FontWeight.ExtraBold, 0, false, CharacterSet.Default, 
                Precision.String, FontQuality.ClearTypeNatural, PitchAndFamily.VariablePitch, ff.Name);
        }

        // Some graphics settings got dorked or the vid display can no longer support them.  Revert to base settings.
        private void RevertToBaseGraphicsSettings()
        {
            core.settings.SetSetting("Gfx_DisplayMode", 0);
            core.settings.SetSetting("Gfx_ResX", 800);
            core.settings.SetSetting("Gfx_ResY", 600);
            core.settings.SetSetting("Gfx_CDepth", 16);
            core.settings.SetSetting("Gfx_PosX", 50);
            core.settings.SetSetting("Gfx_PosY", 50);
            core.settings.SetSetting("Gfx_Adapter", 0);
        }

        // Puts dem pixels on teh screen!!
        protected override void OnPaint(PaintEventArgs e)
        {
            int result;

            if (device.CheckCooperativeLevel(out result))
            {
                try
                {
                    RenderViewport(e);
                }
                catch (d3d.DeviceLostException)
                {
                    device.CheckCooperativeLevel(out result);
                }
                catch (d3d.DeviceNotResetException)
                {
                    device.CheckCooperativeLevel(out result);
                }
            }

            if (result == (int)d3d.ResultCode.DeviceLost)
            {
                Thread.Sleep(500);
            }
            else if (result == (int)d3d.ResultCode.DeviceNotReset)
            {
                device.Reset(pp);
                SetupConsoleWindow();
                SetUpCamera();
            }
            else
            {
                if (!device.PresentationParameters.Windowed)
                    Cursor.Clip = new Rectangle(this.Location, this.Size);
                else
                    Cursor.Clip = new Rectangle();
            }

            // used to apply new graphics settings when changed by user
            if (core.bGfxResetRequired)
            {
                core.bGfxResetRequired = false;
                ConfigPP();
                try
                {
                    device.Reset(pp);
                    SetupConsoleWindow();
                    SetUpCamera();
                }
                catch
                {
                    RevertToBaseGraphicsSettings();
                    ConfigPP();
                }
            }

            // Resolves issue where window is not displayed as top-most on launch.
            // May be able to remove this on released build, appears to be a visual studio hosting issue.
            if (!PulledToFront)
            {
                Cursor.Position = new Point(this.Width / 2, this.Height / 2);
                this.TopMost = true;
                this.TopMost = false;
                PulledToFront = true;
                core.bGfxFocus = this.Focused;
            }

            // Program utility - calculate FPS and reset graphics watchdog timer
            CalcFPS();
            core.swGfxWatchdog.Reset();
            core.swGfxWatchdog.Start();
            this.Invalidate();
        }
        private void RenderViewport(PaintEventArgs e)
        {
            // Sets up render window for new scene
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkBlue, 1.0f, 0);
            device.BeginScene();
            //device.VertexFormat = CustomVertex.TransformedColored.Format;
            
            // Skybox testing ////////////////////////////////
            device.RenderState.Ambient = Color.White;
            device.Transform.World = Matrix.RotationX((float)Math.PI / 2) * Matrix.RotationYawPitchRoll((float)0, (float)0, (float)0);
            DrawMesh(skyboxmesh, skyboxmaterials, skyboxtextures);

            fXMod += (float)core.logic.MouseData.iXFromCenter / 1920f;
            iYMod += core.logic.MouseData.iYFromCenter;

            float iX = (float)(10 * Math.Cos(Math.PI * fXMod / 180));
            float iY = 1;// (float)(10 * Math.Sin(Math.PI * iYMod / 720));
            float iZ = 0; // iX / 4f;

            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4f, (float)this.Width / (float)this.Height, 0.3f, 500f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 0), new Vector3(iX, iY, iZ), new Vector3(0, 0, 1));
            iDeg++;
            if (iDeg == 1440)
                iDeg = 0;

            //////////////////////////////////////////////////

            fntHeading.DrawText(null, string.Format("iX: {0}\r\niY: {1}\r\niZ: {2}\r\niDeg: {3}", iX, iY, iZ, iDeg) , 199, 401, Color.White);
            fntHeading.DrawText(null, string.Format("_iMX: {0}\r\n_iMY: {1}\r\nfXMod: {2}", core.logic.MouseData.iXFromCenter, core.logic.MouseData.iYFromCenter, fXMod), 199, 801, Color.White);

            // Procedure calls for putting objects up on the screen
            DrawUIElements(e);

            
            
            // Draw the console last
            if (core.logic.bConEnabled)
            {
                PrintOSDDebugText(e);
                if (core.logic.bConDisplay)
                    DrawConsoleWindow(e);
            }

            // Present scene, gets next graphics cycle to start
            device.EndScene();
            device.Present();
        }
        private void DrawUIElements(PaintEventArgs e)
        {
            // Store current device renderstate for later
            StateBlock sb = new StateBlock(device, StateBlockType.All);

            // Set needed renderstate for drawing the console window
            device.RenderState.DiffuseMaterialSource = ColorSource.Color1;
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceColor;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            device.VertexFormat = CustomVertex.TransformedColored.Format;
            
            // Draw the elements - THIS NEEDS WORK OMG!
            for (int i = 0; i < core.logic.UIEles.Count; i++)
            {
                if (core.logic.UIEles[0].vFrame != null)
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, ((core.logic.UIEles[i].vFrame.GetUpperBound(0) + 1) / 3), core.logic.UIEles[i].vFrame);
                if (core.logic.UIEles[i].sTxt != null)
                    for (int j = 0; j <= core.logic.UIEles[i].sTxt.GetUpperBound(0); j++)
                        fntHeading.DrawText(null, core.logic.UIEles[i].sTxt[j], core.logic.UIEles[i].ptTxt[j], core.logic.UIEles[i].iTxtColor[j]);
            }

            // revert the renderstate and clean up
            sb.Apply();
            sb.Dispose();
        }
        private void DrawConsoleWindow(PaintEventArgs e)
        {
            // Store current device renderstate for later
            StateBlock sb = new StateBlock(device, StateBlockType.All);

            // Set needed renderstate for drawing the console window
            device.RenderState.DiffuseMaterialSource = ColorSource.Color1;
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceColor;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            device.VertexFormat = CustomVertex.TransformedColored.Format;

            // draw the window
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 2, cvConsole);

            // calculate max displayed records
            int iMaxDisplayed = (((int)fConsoleHeight - 54) / 20) + 1;

            int iMaxPages = core.logic.iConOutputEntries / iMaxDisplayed;
            if ((iMaxPages < 1) || ((core.logic.iConOutputEntries % iMaxDisplayed) > 0))
                iMaxPages++;

            if (core.logic.iConOutputScrollPos >= iMaxPages)
                core.logic.iConOutputScrollPos = iMaxPages - 1;
            if (core.logic.iConOutputScrollPos < 0)
                core.logic.iConOutputScrollPos = 0;


            // print command and history text
            fntConsole.DrawText(null, core.logic.sConInput + "_", 10, (int)fConsoleHeight - 28, Color.FromArgb(0, 180, 0));
            for (int i = 0; i < iMaxDisplayed; i++)
            {
                // adjusts read index for history based on scroll position
                int iMod = iMaxDisplayed * core.logic.iConOutputScrollPos;
                if ((iMod + (iMaxDisplayed - 1)) > core.logic.sConOutput.GetUpperBound(0))
                    iMod -= (iMod + (iMaxDisplayed - 1)) - core.logic.sConOutput.GetUpperBound(0);

                fntConsole.DrawText(null, core.logic.sConOutput[i + iMod], 10, (int)fConsoleHeight - 54 - (i * 20), Color.FromArgb(0, 180, 0));
            }

            // revert the renderstate and clean up
            sb.Apply();
            sb.Dispose();
        }
        private void PrintOSDDebugText(PaintEventArgs e)
        {
            switch (core.logic.iDebugTextMode)
            {
                case 0:
                    break;
                case 1:
                    fntDebugOSD.DrawText(null, "Debug Text 1 - Random Shit", 10, 20, Color.White);
                    fntDebugOSD.DrawText(null, "Calculated FPS: " + core.lCalculatedFPS, 20, 40, Color.White);
                    fntDebugOSD.DrawText(null, "Calculated GSPS: " + core.lCalculatedGSPS, 20, 60, Color.White);
                    break;
                case 2:
                    fntDebugOSD.DrawText(null, "Debug Text 2 - Mouse Data", 10, 20, Color.White);
                    fntDebugOSD.DrawText(null, "iMouseX: " + core.logic.MouseData.iMouseX, 20, 40, Color.White);
                    fntDebugOSD.DrawText(null, "iMouseY: " + core.logic.MouseData.iMouseY, 20, 60, Color.White);
                    fntDebugOSD.DrawText(null, "LMB Down: " + core.logic.MouseData.GetButtonDown(System.Windows.Forms.MouseButtons.Left), 20, 80, Color.White);
                    fntDebugOSD.DrawText(null, "RMB Down: " + core.logic.MouseData.GetButtonDown(System.Windows.Forms.MouseButtons.Right), 20, 100, Color.White);
                    fntDebugOSD.DrawText(null, "MMB Down: " + core.logic.MouseData.GetButtonDown(System.Windows.Forms.MouseButtons.Middle), 20, 120, Color.White);
                    fntDebugOSD.DrawText(null, "1MB Down: " + core.logic.MouseData.GetButtonDown(System.Windows.Forms.MouseButtons.XButton1), 20, 140, Color.White);
                    fntDebugOSD.DrawText(null, "2MB Down: " + core.logic.MouseData.GetButtonDown(System.Windows.Forms.MouseButtons.XButton2), 20, 160, Color.White);
                    break;
            }
        }
        private void DrawMesh(Mesh mesh, Material[] meshmaterials, Texture[] meshtextures)
        {
            StateBlock sb = new StateBlock(device, StateBlockType.All);
            for (int i = 0; i < meshmaterials.Length; i++)
            {
                device.Material = meshmaterials[i];
                device.SetTexture(0, meshtextures[i]);
                mesh.DrawSubset(i);
            }
            sb.Apply();
            sb.Dispose();
        }
        
        // Calculates rendered frames per second
        private void CalcFPS()
        {
            if (swFPS.IsRunning)
            {
                iRenders++;
                long elapsed = swFPS.ElapsedMilliseconds;
                if (elapsed > 1000)
                {
                    core.lCalculatedFPS = iRenders * elapsed / 1000;
                    iRenders = 0;
                    swFPS.Reset();
                    swFPS.Start();
                }
            }
            else
                swFPS.Start();

        }

        // [JUNK]
        public void GetDisplayInformation()
        {
            string stemp = "";

            int passcount = 0;
            foreach (AdapterInformation ai in Manager.Adapters)
            {
                stemp += "ADAPTER LISTING " + passcount.ToString() + "\r\n===========================\r\n";
                stemp += ai.Information.Description + "\r\n";
                stemp += ai.Information.DeviceId + "\r\n";
                stemp += ai.Information.DeviceIdentifier + "\r\n";
                stemp += ai.Information.DeviceName  + "\r\n";
                stemp += ai.Information.DriverName  + "\r\n";
                stemp += ai.Information.DriverVersion  + "\r\n";
                stemp += Manager.Adapters[passcount].CurrentDisplayMode.ToString() + "\r\n";
                foreach (DisplayMode dm in Manager.Adapters[passcount].SupportedDisplayModes)
                {
                    stemp += "  " + dm.ToString() + "\r\n";
                }
                stemp += "\r\n";
                passcount++;
            }

            

            core.TEMP1 = stemp;
            
        }
        
    }
}
