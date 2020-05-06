using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoitaSaver
{
    public partial class NoitaSaver : Form
    {
        public string UserName;

        public string NollaPath;

        public string SavePath;

        public string SaveSlotSelection = "1";

        public string SecureSavePath;

        public NoitaSaver()
        {
            InitializeComponent();

            UserName = Environment.UserName;

            NollaPath = Path.Combine("C:\\Users\\", string.Format("{0}\\AppData\\LocalLow\\Nolla_Games_Noita\\", UserName));
            SavePath = Path.Combine(NollaPath, "save00\\");
            SecureSavePath = Path.Combine(NollaPath + SaveSlotSelection);
            DirectoryInfo directoryInfo = new DirectoryInfo(SecureSavePath);

            if (directoryInfo.Exists)
            {
                LoadButton.Enabled = true;

                DeleteButton.Enabled = true;
            }
            else
            {
                LoadButton.Enabled = false;

                DeleteButton.Enabled = false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (IsGameRunning())
            {
                MessageBox.Show("Stop the game first!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Sure?", "Saving", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                EmptyFolder(new DirectoryInfo(SecureSavePath));

                CopyFolderSave(SavePath, SecureSavePath);

                LoadButton.Enabled = true;

                DeleteButton.Enabled = true;
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (IsGameRunning())
            {
                MessageBox.Show("Stop the game first!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Sure?", "Loading", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                EmptyFolder(new DirectoryInfo(SavePath));

                CopyFolderLoad(SecureSavePath, SavePath);

                LoadButton.Enabled = true;

                DeleteButton.Enabled = true;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure?????", "Deleting", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                EmptyFolder(new DirectoryInfo(NollaPath + SaveSlotSelection));

                LoadButton.Enabled = true;

                DeleteButton.Enabled = true;
            }
        }

        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder);
            }
        }

        public void CopyFolderSave(string SourcePath, string DestinationPath)
        {

            DirectoryInfo destinationDirectoryInfo = new DirectoryInfo(DestinationPath);
            if (!destinationDirectoryInfo.Exists)
            {
                Directory.CreateDirectory(destinationDirectoryInfo.FullName);
            }

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
            {
                string newDirPath = dirPath.Replace("save00", SaveSlotSelection);
                Directory.CreateDirectory(newDirPath);
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace("save00", SaveSlotSelection), true);
        }

        public void CopyFolderLoad(string SourcePath, string DestinationPath)
        {

            DirectoryInfo destinationDirectoryInfo = new DirectoryInfo(DestinationPath);
            if (!destinationDirectoryInfo.Exists)
            {
                Directory.CreateDirectory(destinationDirectoryInfo.FullName);
            }

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
            {
                string newDirPath = dirPath.Replace(SaveSlotSelection, "save00");
                Directory.CreateDirectory(newDirPath);
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SaveSlotSelection, "save00"), true);
        }

        private bool IsGameRunning()
        {
            bool isRunning = Process.GetProcessesByName("noita")
                .FirstOrDefault(p => p.MainModule.FileName.StartsWith(@"C:\Program Files (x86)\Steam\steamapps\common\Noita", StringComparison.InvariantCultureIgnoreCase)) != default(Process);

            if (!isRunning)
                isRunning = Process.GetProcessesByName("noita_dev")
                    .FirstOrDefault(p => p.MainModule.FileName.StartsWith(@"C:\Program Files (x86)\Steam\steamapps\common\Noita", StringComparison.InvariantCultureIgnoreCase)) != default(Process);

            return isRunning;
        }

        private void ShowFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(SecureSavePath))
                    Process.Start(SecureSavePath);
                else
                    Process.Start(NollaPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Seems like you are missing the folder:\n" + NollaPath);
            }
        }

        private void saveSlotRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            SaveSlotSelection = radioButton.Text.Substring(5);
            SecureSavePath = Path.Combine(NollaPath + SaveSlotSelection);
            DirectoryInfo directoryInfo = new DirectoryInfo(SecureSavePath);

            if (directoryInfo.Exists)
            {
                LoadButton.Enabled = true;

                DeleteButton.Enabled = true;
            }
            else
            {
                LoadButton.Enabled = false;

                DeleteButton.Enabled = false;
            }
        }
    }
}
