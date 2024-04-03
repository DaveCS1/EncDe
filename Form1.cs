using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptDecryptAndRandomize
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //need a dragover event
        private void frmMain_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    lstFiles.Items.Add(file);

                }
                if (lstFiles.Items.Count > 0)
                {
                    lstFiles.SelectedIndex = 0;
                }

            }


            lstFiles.SelectedIndexChanged += (sender2, e2) =>
            {
                if (lstFiles.SelectedItem != null)
                {
                    string filePath = lstFiles.SelectedItem.ToString();
                    string fileContent = System.IO.File.ReadAllText(filePath);
                    rtfFileContents.Text = fileContent;
                }
            };

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBox1.Checked;
        }

        private void tsAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    lstFiles.Items.Add(file);
                }
            }

        }

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabEncrypt;
        if (lstFiles.SelectedItems.Count > 0)
        {
            List<string> selectedFiles = new List<string>();
            foreach (var item in lstFiles.SelectedItems)
            {
                selectedFiles.Add(item.ToString());
            }
            
            OfuscateAndEncrypt encryptor = new OfuscateAndEncrypt(selectedFiles);
                txtEncryptionKey.Text = encryptor.EncryptionKey;
        }
        else
        {
            MessageBox.Show("Please select at least one file to encrypt.", "No Files Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = txtEncryptionKey.Text;
           tabControl1.SelectedTab = tabDecrypt;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
