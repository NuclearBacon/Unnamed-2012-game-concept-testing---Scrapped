// <=========================================================================================================================>
// The 'gamesettingsmanager.cs' code section stores game related settings during run time as well as saving/loading those
// settings during shutdown/startup.  Options exist in member methods to save/load using the system registry or local file
// system and to encrypt the saved settings.
// 
// This code does not manage save game files.
// <=========================================================================================================================>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.DirectX.Direct3D;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using rRes = dasspiel.Properties.Resources;
using System.Reflection;

namespace dasspiel
{
    public class GameSettingsManager
    {
        #region Variable Declaration
        // <=================================================================================================================>
        private RegistryKey rkMainKey = Registry.CurrentUser;
        private RegistryKey rkSubKey;
        private string sSettingsBlob = "";
        
        // Hard coded game settings container class
        public CoreSettings CoreSets = new CoreSettings();

        // Soft game settings loaded from registry/settings file
        private List<string> Settings_NameStrings = new List<string>();
        private List<object> Settings_DataObjects = new List<object>();
        private List<bool> Settings_Protected = new List<bool>();
        // <=================================================================================================================>
        #endregion

        // [DONE] Class construction stuff
        public GameSettingsManager()
        {
            ConfigCoreSettings();
            ConfigRegSettings();
        }
        private void ConfigCoreSettings()
        {
            // Load the names of all the fields in the CoreSets member.
            FieldInfo[] fiSettings = CoreSets.GetType().GetFields();

            // Pull the core settings string from the resource table and decrypt it
            sSettingsBlob = core.utilities.DecStr(rRes.strCore, 1);

            // Assign data
            foreach (FieldInfo fi in fiSettings)
            {
                if (!fi.FieldType.ToString().EndsWith("]"))  // not an array entry
                {
                    // find the index of the setting name if it exists
                    int iStartIndex = sSettingsBlob.IndexOf(fi.Name);

                    // if it does exist, start finding the bounds of the data and move that to the setting
                    if (iStartIndex >= 0)
                    {
                        int iDataIndex = iStartIndex + fi.Name.Length + 1;
                        for (int a = iDataIndex; a < sSettingsBlob.Length - 1; a++)
                        {
                            if (sSettingsBlob.Substring(a, 2) == core.cr)
                            {
                                fi.SetValue(CoreSets, sSettingsBlob.Substring(iDataIndex, a - iDataIndex));
                                break;
                            }
                        }
                    }
                    else
                        fi.SetValue(CoreSets, null);
                }
                else if (fi.FieldType.ToString().EndsWith("[]"))  // single dimension array
                {
                    string[] arr = (string[])fi.GetValue(CoreSets);
                    for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                    {
                        // find the index of the setting name if it exists
                        int iStartIndex = sSettingsBlob.IndexOf(string.Format("{0}[{1}]", fi.Name, i));

                        // if it does exist, start finding the bounds of the data and move that to the setting
                        if (iStartIndex >= 0)
                        {
                            int iDataIndex = iStartIndex + fi.Name.Length + i.ToString().Length + 3;
                            for (int a = iDataIndex; a < sSettingsBlob.Length - 1; a++)
                            {
                                if (sSettingsBlob.Substring(a, 2) == core.cr)
                                {
                                    arr[i] = sSettingsBlob.Substring(iDataIndex, a - iDataIndex);
                                    break;
                                }
                            }
                        }
                        else
                            arr[i] = null;
                    }
                    fi.SetValue(CoreSets, arr);
                }
                else if (fi.FieldType.ToString().EndsWith("[,]"))  // two dimension array
                {
                    string[,] arr = (string[,])fi.GetValue(CoreSets);
                    for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                    {
                        for (int j = arr.GetLowerBound(1); j <= arr.GetUpperBound(1); j++)
                        {
                            // find the index of the setting name if it exists
                            int iStartIndex = sSettingsBlob.IndexOf(string.Format("{0}[{1},{2}]", fi.Name, i, j));

                            // if it does exist, start finding the bounds of the data and move that to the setting
                            if (iStartIndex >= 0)
                            {
                                int iDataIndex = iStartIndex + fi.Name.Length + i.ToString().Length +  j.ToString().Length + 4;
                                for (int a = iDataIndex; a < sSettingsBlob.Length - 1; a++)
                                {
                                    if (sSettingsBlob.Substring(a, 2) == core.cr)
                                    {
                                        arr[i,j] = sSettingsBlob.Substring(iDataIndex, a - iDataIndex);
                                        break;
                                    }
                                }
                            }
                            else
                                arr[i,j] = "";
                        }
                    }
                    fi.SetValue(CoreSets, arr);                    
                }
                else  // shit got fucked up
                {
                    throw new Exception("Unrecognized variable dimensions:\r\n" + fi.Name);
                }
            }
        }
        private void ConfigRegSettings()
        {
            rkSubKey = rkMainKey.OpenSubKey(CoreSets.strRegPath, true);
            if (rkSubKey == null)
            {
                var bbb = rkMainKey.CreateSubKey(CoreSets.strRegPath);
                rkSubKey = rkMainKey.OpenSubKey(CoreSets.strRegPath, true);
            }
        }


        // [WIP] This loads the settings as saved in the system registry and dumps them in to the bucket
        // TO DO:  See about removing the error message to a common error handler
        public void LoadGameSettings(LoadSource ForcedSource = LoadSource.DoNotForceLoad)
        {
            bool LoadFromRegFailed = false;
            sSettingsBlob = "";

            // try to load from registry if forced or source not specified
            if ((ForcedSource == LoadSource.ForceLoadFromRegistry) || (ForcedSource ==  LoadSource.DoNotForceLoad))
            {
                sSettingsBlob = rkSubKey.GetValue(CoreSets.strRegSettings, "").ToString();
                if (sSettingsBlob == "")
                    LoadFromRegFailed = true;
            }

            // try to load from file if force or the registry fails when a source is not specified
            if ((ForcedSource == LoadSource.ForceLoadFromFile) || ((ForcedSource == LoadSource.DoNotForceLoad) && LoadFromRegFailed))
            {
                try
                {
                    string sSP = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CoreSets.strMyDocsPath);
                    FileStream fs = new FileStream(sSP + CoreSets.strFNSettings, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    sSettingsBlob = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                catch { }
                    // We really don't care if this fails
            }

            // Decrypt loaded data if needed
            string dtmp = core.utilities.DecStr(sSettingsBlob);
            if (dtmp != null)
                if (dtmp.Substring(0, (rRes.sEnc4 + rRes.sEnc3).Length) == (rRes.sEnc4 + rRes.sEnc3))
                    sSettingsBlob = core.utilities.DecStr(sSettingsBlob);
    
            // Pull entries from the loaded string and created entries
            string vartype = "";
            string varname = "";
            string varval = "";
            bool CorrDet = false;
            int searchpos = 0;
            int typefoundat = 0;
            int equalfoundat = 0;

            foreach (char c in sSettingsBlob)
            {
                if (CorrDet)  // something bad was in the data last run - abort the loading
                    break;

                if ((vartype == "") && ((int)c != 10) && ((int)c != 13))  // we don't have a variable type yet.  get one.  oh, but watch out for the tail end of a carriage return.
                {
                    vartype = c.ToString();
                    typefoundat = searchpos;
                }
                else if ((varname == "") && (c.ToString() == "="))  // we've got a variable type, but not a name.  search until we get an equals sign.
                {
                    varname = sSettingsBlob.Substring(typefoundat + 1, searchpos - typefoundat - 1);
                    equalfoundat = searchpos;
                }
                else if ((vartype != "") && (varval == "") && ((int)c == 13))  // just need a value now.  search until finding a carriage return.
                {
                    varval = sSettingsBlob.Substring(equalfoundat + 1, searchpos - equalfoundat - 1);

                    Settings_NameStrings.Add(varname);
                    Settings_Protected.Add(core.settings.CoreSets.strProtectedSettings.Contains(varname));

                    try
                    {
                        switch (vartype)
                        {
                            case "i":
                                Settings_DataObjects.Add(int.Parse(varval));
                                break;
                            case "b":
                                Settings_DataObjects.Add(bool.Parse(varval));
                                break;
                            case "d":
                                Settings_DataObjects.Add(Double.Parse(varval));
                                break;
                            case "s":
                                Settings_DataObjects.Add(varval);
                                break;
                            default:
                                CorrDet = true;
                                break;
                        }
                    }
                    catch
                    {
                        CorrDet = true;
                    }
                    varname = "";
                    vartype = "";
                    varval = "";
                }
                searchpos++;
            }

            if (CorrDet)
            {
                Settings_DataObjects.Clear();
                Settings_NameStrings.Clear();
                Settings_Protected.Clear();
                System.Windows.Forms.MessageBox.Show("Configuration settings corrupted.  Loading default settings.", "Scheisse!");
            }
            sSettingsBlob = "";
        }

        // [DONE] This saves the settings to the system registry or local save file
        public void SaveGameSettings(bool SkipEncryption = false, bool SaveToRegistry = false)
        {
            // Build the blob string
            sSettingsBlob = "";
            int loopint = 0;

            foreach (string str in Settings_NameStrings)
            {
                if (str != "")
                    sSettingsBlob += TypeToCode(Settings_DataObjects[loopint].GetType()) + str + "=" + Settings_DataObjects[loopint].ToString() + core.cr;
                loopint++;
            }

            // encrypt the settings information
            if (!SkipEncryption)
            {
                sSettingsBlob = core.utilities.EncStr(sSettingsBlob, true);
            }
            
            // Save to registry
            if (SaveToRegistry)
                rkSubKey.SetValue(CoreSets.strRegSettings, sSettingsBlob);
            // Save to file
            else
            {
                string sSP = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CoreSets.strMyDocsPath);
                Directory.CreateDirectory(sSP);
                FileStream fs = new FileStream(sSP + CoreSets.strFNSettings, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sSettingsBlob);
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }

            // Cleanup
            sSettingsBlob = "";
        }

        // [DONE] Stores a settings value to the active settings bucket
        public void SetSetting(string SettingName, object SettingValue, bool Protected = false)
        {
            bool AlreadyExisted = false;
            int forloopindex = 0;

            // Search existing settings for a matching name and append if already existing
            foreach (string varname in Settings_NameStrings)
            {
                if (varname == SettingName)
                {
                    AlreadyExisted = true;
                    Settings_DataObjects[forloopindex] = SettingValue;
                }
                forloopindex++;
            }

            // So, this setting has never been set this session. 
            if (!AlreadyExisted)
            {
                Settings_NameStrings.Add(SettingName);
                Settings_DataObjects.Add(SettingValue);
                Settings_Protected.Add(Protected);
            }
        }

        // [DONE] Retrieves a game setting from the settings bucket for a calling procedure.
        // Will return a null value if the setting does not exist, hopefully without breaking the calling method.
        public object GetSetting(string SettingName, object DefaultValue = null)
        {
            object returnobj = DefaultValue;
            int searchindex = 0;

            foreach (string str in Settings_NameStrings)
            {
                if (str == SettingName)
                    returnobj = Settings_DataObjects[searchindex];
                searchindex++;
            }

            return returnobj;
        }

        // [WIP] Special search method for locating keybindings
        public string GetKeyBinding(int iKeyIndex, bool bS, bool bA, bool bC)
        {
            // Build settings file version of binding code
            string sStoredInt = (Convert.ToInt32(bS) + 2 * Convert.ToInt32(bA) + 4 * Convert.ToInt32(bC)).ToString() + iKeyIndex.ToString();


            return sStoredInt;
        }

        // [DONE] Returns the protected status of a setting.  Primarily intended to prevent editing via console commands.
        public bool IsProtected(string SettingName)
        {
            bool returnval = false;
            int searchindex = 0;

            foreach (string str in Settings_NameStrings)
            {
                if (str == SettingName)
                    returnval = Settings_Protected[searchindex];
                searchindex++;
            }

            return returnval;
        }

        // [WIP] Translates the variable type to a unique string numeral for a shorter registry key.
        // It's a little ghetto, but this prevents having to make a seperate overload for each
        // type of variable that could possibly sent to SetSettings(...) method.
        private string TypeToCode(System.Type st)
        {
            string returncode = null;
            switch (st.ToString())
            {
                case "System.Int32":
                    returncode = "i";
                    break;
                case "System.String":
                    returncode = "s";
                    break;
                case "System.Boolean":
                    returncode = "b";
                    break;
                case "System.Double":
                    returncode = "d";
                    break;
                case "System.Float":
                    returncode = "f";
                    break;
            }

            return returncode;
        }

        public enum LoadSource
        {
            DoNotForceLoad = 0,
            ForceLoadFromFile = 1,
            ForceLoadFromRegistry = 2
        }

        public class CoreSettings
        {
            // soft game settings storage variables
            public string strMyDocsPath, strFNSettings;
            public string strRegPath, strRegSettings;

            // settings management
            public string strProtectedSettings;

            // console crap
            private const int iCmds = 10; // defines the maximum number of console commands
            public string[] strCCmd = new string[iCmds];
            public string[,] strUsage = new string[iCmds, 5];
            public string[] strGenericParseFail = new string[4];
            public string[,] strCOutput = new string[iCmds,2];

        }
    }
}


