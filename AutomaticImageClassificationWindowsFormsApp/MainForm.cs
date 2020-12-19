using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomaticImageClassificationWindowsFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            bool isFileDialog = true;
            ManageDialogs(isFileDialog, FilePathTextbox);


        }

        private void ChooseProjectPath_Click(object sender, EventArgs e)
        {
            bool isFileDialog = false;
            ManageDialogs(isFileDialog, projectPathTextbox);

            

        }
        
        private void ManageDialogs(bool isFileDialog, TextBox textBox )
        {
            DialogResult result;
            string path;
            if (isFileDialog)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Browse Text Files",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "txt",

                    Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                result = openFileDialog.ShowDialog();
                path = openFileDialog.FileName;
            }
            else
            {
               FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;

                result = folderDlg.ShowDialog();
                path = folderDlg.SelectedPath;

                Environment.SpecialFolder root = folderDlg.RootFolder;

            }


            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(path))
            {
                textBox.Text = path;

                ToolTip tip = new ToolTip();
                tip.Show(path, textBox, 0, 0, 2000);
                
            }
            else
            {

            }
            
        }

    }
}
