using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser;
using System.IO;
using IniParser.Model;

namespace DCImporter
{

    //sss
    
    public partial class Form1 : Form
    {

        IniData iniGlobal;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            FileIniDataParser fileIniData = new FileIniDataParser();
            string diskId = null;
            
            if (File.Exists(Directory.GetCurrentDirectory() + "\\DCImporter.ini"))
            {
                iniGlobal = fileIniData.ReadFile(Directory.GetCurrentDirectory() + "\\DCImporter.ini");

                string importDirectory = iniGlobal["GLOBAL"]["IMPORT_DIRECTORY"];
                if (importDirectory.Substring(importDirectory.Length - 1, 1) != "\\")
                {
                    importDirectory = importDirectory + "\\";
                }


                if (File.Exists(importDirectory + "\\DCImport.ini"))
                {
                    FileIniDataParser fileIniDataImport = new FileIniDataParser();
                    IniData iniImport = fileIniData.ReadFile(importDirectory + "DCImport.ini");
                    diskId = iniImport["DCIMPORT"]["ID"];
                }
                else // Create file
                {
                    FileIniDataParser fileIniDataImport = new FileIniDataParser();
                    IniData iniImport = new IniData();
                    diskId = Guid.NewGuid().ToString();
                    iniImport.Sections.AddSection("DCIMPORT");
                    iniImport["DCIMPORT"]["ID"] = diskId;
                    fileIniDataImport.WriteFile(importDirectory + "DCImport.ini", iniImport);
                }


                string photoDirectory = iniGlobal["RX100"]["PHOTO_DIRECTORY"];
                string lastFilename = iniGlobal["RX100"]["LASTFILE"];

                textBoxPhotoDirectory.Text = photoDirectory;
                textBoxLastFilename.Text = lastFilename;

                string[] filePaths = Directory.GetFiles(@photoDirectory, "*.jpg", SearchOption.AllDirectories);
                Boolean start = false;
                foreach (string i in filePaths) {
                    if (i.IndexOf(lastFilename) != -1)
                    {
                        start = true;
                    }
                    if (start == true) {
                        listBox1.Items.Add(i);
                    }
                }
                listBox1.Items.RemoveAt(0); // Remove 1st item

            }
            else
            {

            }

            Guid id = Guid.NewGuid();

        }

        private void textBoxPhotoDirectory_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string targetDirectory = iniGlobal["RX100"]["TARGET_DIRECTORY"];

            if (!System.IO.Directory.Exists(targetDirectory))
            {
                System.IO.Directory.CreateDirectory(targetDirectory);
            }

            progressBar1.Maximum = listBox1.Items.Count;
            progressBar1.Value = 0;
            foreach (string i in listBox1.Items)
            {
                string filename = Path.GetFileName(i);
                string targetFile = System.IO.Path.Combine(targetDirectory, filename);
                //System.IO.File.Copy(i, targetFile); // not overwrite
                System.IO.File.Copy(i, targetFile, true);  // overwrite
                progressBar1.Value += 1;
            }

            string newLastFilename = Path.GetFileName(listBox1.Items[listBox1.Items.Count - 1].ToString());

            MessageBox.Show("Processing was completed");
            progressBar1.Value = 0;
        }
    }
}
