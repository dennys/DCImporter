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

        IniData parsedData;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileIniDataParser fileIniData = new FileIniDataParser();
            ;
            if (File.Exists(Directory.GetCurrentDirectory() + "\\DCImporter.ini"))
            {
                parsedData = fileIniData.ReadFile(Directory.GetCurrentDirectory() + "\\DCImporter.ini");
                string photoDirectory = parsedData["RX100"]["PHOTO_DIRECTORY"];
                string lastFilename = parsedData["RX100"]["LASTFILE"];

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

        }

        private void textBoxPhotoDirectory_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string targetDirectory = parsedData["RX100"]["TARGET_DIRECTORY"];

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
