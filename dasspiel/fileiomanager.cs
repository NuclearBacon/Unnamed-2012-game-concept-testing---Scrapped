// <=========================================================================================================================>
// The 'fileiomanager.cs' code section loads records from archive files and handles save file loading/saving.
// <=========================================================================================================================>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace dasspiel
{
    public class FileIOManager
    {
        // Variable declarations
        #region VARDEC

        private List<ArchiveData> lArchData = new List<ArchiveData>();
        private const string sArchiveExtension = ".jda";

        #endregion

        // Class contructor
        public FileIOManager()
        {

        }

        // Load data regarding a resource archive
        public void CreateArchiveRecord(string sArchiveName)
        {
            // Create temp archive while loading info
            ArchiveData adTmp = new ArchiveData();

            // Attempt to open archive file, pass error if not able
            FileStream fsArch = null;
            BinaryReader brArch = null;
            FileInfo finfo = null;
            try
            {
                fsArch = new FileStream(Application.StartupPath + "\\data\\" + sArchiveName + sArchiveExtension, FileMode.Open, FileAccess.Read);
                brArch = new BinaryReader(fsArch);

                finfo = new FileInfo(Application.StartupPath + "\\data\\" + sArchiveName + sArchiveExtension);
            }
            catch (Exception e)
            {
                core.utilities.ErrorHandler(e);
            }

            // Get encryption mode
            int iEncMode = (int)brArch.ReadByte();
            if (iEncMode == 255)
                adTmp.EncMode = -1;
            else
                adTmp.EncMode = iEncMode;

            // Get header information
            int iHeaderLen = BitConverter.ToInt32(brArch.ReadBytes(4), 0);
            byte[] bHeader = brArch.ReadBytes(iHeaderLen);
            

            // Decrypt header information, if needed
            if (adTmp.EncMode > -1)
                bHeader = core.utilities.DecByte(bHeader, adTmp.EncMode);

            // Split header info
            byte[] bHeaderData = new byte[bHeader.Length - 8];
            byte[] bHeaderHash = new byte[8];
            for (int a = 0; a < bHeader.Length; a++)
            {
                if (a < bHeader.Length - 8)
                    bHeaderData[a] = bHeader[a];
                else
                    bHeaderHash[a - bHeaderData.Length] = bHeader[a];
            }

            // Check header hash
            byte[] bCheckHash = ToSHA256(bHeaderData);
            if (!HashCompare(bCheckHash, bHeaderHash))
                core.utilities.ErrorHandler(new Exception("Archive header corrupted"));

            // Get version information
            adTmp.VerMajor = BitConverter.ToInt32(bHeaderData, 0);
            adTmp.VerMinor = BitConverter.ToInt32(bHeaderData, 4);
            adTmp.VerBuild = BitConverter.ToInt32(bHeaderData, 8);
            adTmp.VerRevision = BitConverter.ToInt32(bHeaderData, 12);

            // Get internal archive name
            for (int a = 16; a < bHeaderData.Length; a++)
                adTmp.Name += (char)(int)bHeaderData[a];
            if (adTmp.Name != sArchiveName)
                core.utilities.ErrorHandler(new Exception("Internal archive name mismatch."));

            // Read raw record data
            int iRecordDataLength = BitConverter.ToInt32(brArch.ReadBytes(4), 0);
            byte[] bRawRecordData = brArch.ReadBytes(iRecordDataLength);

            // Decrypt record data, if needed
            int iEncOffsetMod = bRawRecordData.Length;
            if (adTmp.EncMode > -1)
                bRawRecordData = core.utilities.DecByte(bRawRecordData, adTmp.EncMode);
            iEncOffsetMod -= bRawRecordData.Length;

            // Split header info
            byte[] bRecordData = new byte[bRawRecordData.Length - 8];
            byte[] bRecordHash = new byte[8];
            for (int a = 0; a < bRawRecordData.Length; a++)
            {
                if (a < bRawRecordData.Length - 8)
                    bRecordData[a] = bRawRecordData[a];
                else
                    bRecordHash[a - bRecordData.Length] = bRawRecordData[a];
            }

            // Check header hash
            bCheckHash = ToSHA256(bRecordData);
            if (!HashCompare(bCheckHash, bRecordHash))
                core.utilities.ErrorHandler(new Exception("Record data corrupted"));

            // Store record data
            adTmp.EntryCount = BitConverter.ToInt32(bRecordData, 0);
            string sName = "";
            for (int a = 0; a < adTmp.EntryCount; a++)
            {
                // record offset
                adTmp.Offset.Add(BitConverter.ToInt32(bRecordData, 4 + (268 * a)) + iEncOffsetMod);
                
                // record length
                if (a < adTmp.EntryCount - 1)
                    adTmp.RecordLength.Add(BitConverter.ToInt32(bRecordData, 4 + (268 * (a + 1))) - BitConverter.ToInt32(bRecordData, 4 + (268 * a)));
                else
                    adTmp.RecordLength.Add(((int)finfo.Length - BitConverter.ToInt32(bRecordData, 4 + (268 * a))) + iEncOffsetMod);

                // hash data
                byte[] bHash = new byte[8];
                for (int b = 0; b < 8; b++)
                    bHash[b] = bRecordData[8 + (268 * a) + b];
                adTmp.HashData.Add(bHash);

                // record name
                sName = "";
                for (int b = 0; b < 256; b++)
                    sName += (char)(int)bRecordData[16 + (268 * a) + b];
                adTmp.RecordName.Add(sName.TrimEnd(new char[] { (char)0 }));
            }

            brArch.Close();
            fsArch.Close();

            lArchData.Add(adTmp);
        }

        // Archive resource loaders
        public object LoadGenericResourceFromArchive(string sArchiveName, string sResourceName)
        {
            // Get the referenced archive information or set it up if it hasn't been done already
            ArchiveData adResult = lArchData.Find(delegate(ArchiveData ad) { return ad.Name.ToLower() == sArchiveName.ToLower(); });
            if (adResult == null)
            {
                CreateArchiveRecord(sArchiveName);
                adResult = lArchData.Find(delegate(ArchiveData ad) { return ad.Name == sArchiveName; });
            }

            // Find record entry in archive listing
            int iRecNum = adResult.RecordName.FindIndex(delegate(string s) { return s.ToLower() == (sArchiveName + sArchiveExtension + "\\" + sResourceName).ToLower(); });
            if (iRecNum == -1)
                core.utilities.ErrorHandler(new Exception("Record does not exist in archive."));
            
            // Attempt opening archive file
            FileStream fsDataLoad = null;
            BinaryReader brDataLoad = null;
            try
            {
                fsDataLoad = new FileStream(Application.StartupPath + "\\Data\\" + sArchiveName + sArchiveExtension, FileMode.Open, FileAccess.Read);
                brDataLoad = new BinaryReader(fsDataLoad);
            }
            catch (Exception e)
            {
                core.utilities.ErrorHandler(e);
            }
            
            // Read data from file
            brDataLoad.BaseStream.Position = adResult.Offset[iRecNum];
            byte[] bLoadData = brDataLoad.ReadBytes((int)adResult.RecordLength[iRecNum]);

            // Close file
            brDataLoad.Close();
            fsDataLoad.Close();
            
            // Decrypt record if needed
            if (adResult.EncMode > -1)
                bLoadData = core.utilities.DecByte(bLoadData, adResult.EncMode);
            if (bLoadData == null)
                core.utilities.ErrorHandler(new Exception("Error decrypting record."));

            // Check record hash
            byte[] bDataHash = ToSHA256(bLoadData);
            if (!HashCompare(bDataHash, adResult.HashData[iRecNum]))
                core.utilities.ErrorHandler(new Exception("Data record hash invalid."));

            return bLoadData;
        }
        public void LoadMeshFromArchive(string archive, string filename, ref Mesh mesh, ref Material[] meshmaterials, ref Texture[] meshtextures, ref Device device, ref float meshradius)
        {
            ExtendedMaterial[] materialarray;
            MemoryStream ms = new MemoryStream((byte[])LoadGenericResourceFromArchive(archive, filename));
            mesh = Mesh.FromStream(ms, MeshFlags.Managed, device, out materialarray);

            if ((materialarray != null) && (materialarray.Length > 0))
            {
                meshmaterials = new Material[materialarray.Length];
                meshtextures = new Texture[materialarray.Length];

                for (int i = 0; i < materialarray.Length; i++)
                {
                    meshmaterials[i] = materialarray[i].Material3D;
                    meshmaterials[i].Ambient = meshmaterials[i].Diffuse;

                    if ((materialarray[i].TextureFilename != null) && (materialarray[i].TextureFilename != string.Empty))
                    {
                        meshtextures[i] = TextureLoader.FromFile(device, materialarray[i].TextureFilename);
                        /*
                        // This is some really bad code.  Destroys system memory, likely related to shitty FIOM code. Static-ness bad?
                        ms = new MemoryStream((byte[])LoadGenericResourceFromArchive(archive, filename + "_tex\\" + materialarray[i].TextureFilename));
                        if (i == 0)
                            meshtextures[i] = TextureLoader.FromStream(device, ms);
                        else
                            meshtextures[i] = meshtextures[0];
                        */
                        
                    }
                }
            }

            mesh = mesh.Clone(mesh.Options.Value, CustomVertex.PositionNormalTextured.Format, device);
            mesh.ComputeNormals();

            VertexBuffer vertices = mesh.VertexBuffer;
            GraphicsStream stream = vertices.Lock(0, 0, LockFlags.None);
            Vector3 meshcenter;
            meshradius = Geometry.ComputeBoundingSphere(stream, mesh.NumberVertices, mesh.VertexFormat, out meshcenter) * 0.0005f;
        }

        // Random internals
        private class ArchiveData
        {
            public string Name = "";
            public int EncMode = -1;
            
            public int VerMajor = 0;
            public int VerMinor = 0;
            public int VerBuild = 0;
            public int VerRevision = 0;

            public int EntryCount = 0;
            public List<string> RecordName = new List<string>();
            public List<byte[]> HashData = new List<byte[]>();
            public List<int> Offset = new List<int>();
            public List<int> RecordLength = new List<int>();
        }
        private bool HashCompare(byte[] bHash1, byte[] bHash2)
        {
            bool bResult = true;

            if (bHash1.Length == bHash2.Length)
            {
                for (int a = 0; a < bHash1.Length; a++)
                    if (bHash1[a] != bHash2[a])
                        bResult = false;
            }
            else
                bResult = false;

            return bResult;
        }
        private byte[] ToSHA256(byte[] bRaw)
        {
            SHA256Managed sha = new SHA256Managed();

            byte[] chks = sha.ComputeHash(bRaw);
            byte[] shortness = new byte[8];
            for (int a = 0; a < 8; a++)
                shortness[a] = chks[a];

            return shortness;
        }
    }

}
