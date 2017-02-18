using System;
using System.IO;
using System.Windows.Forms;

namespace BatteryAlert
{
    public partial class Form1 : Form
    {
        public string currentSong = null;
        public Form1()
        {
            InitializeComponent();

            //Loads properties
            checkBox1.Checked = Properties.Settings.Default.useNotification;
            checkBox2.Checked = Properties.Settings.Default.launchOnStart;
            //Sets label
            if (!String.IsNullOrEmpty(Properties.Settings.Default.currentSound))
            {
                soundLabel.Text = new FileInfo(Properties.Settings.Default.currentSound).Name;
            }

            //Loads files from local folder into listBox
            listBox1.Items.Add("<None>");
            string[] files = FileUtils.GetFiles(FileUtils.ExecutablePath + "\\sounds\\", "*.mp3|*.wav|*.m4a");
            foreach(string s in files)
            {
                FileInfo info = new FileInfo(s);
                listBox1.Items.Add(FileUtils.TrimFilename(info.Name));
            }

            Program.startThread();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void UnhideWindow(object sender, EventArgs e)
        {
            UnhideWindow();
        }

        private void UnhideWindow()
        {
            if (WindowState != FormWindowState.Normal)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void HideWindow()
        {
            if (WindowState != FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                Hide();
            }
        }
        private void ToggleMinimize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HideWindow();
            }
            else
            {
                UnhideWindow();
            }
        }

        private void QuitAppClick(object sender, EventArgs e)
        {
            Program.ExitProgram();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Audio (*.mp3, *.wav, *.m4a)|*.mp3|*.wav|*.m4a";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                setCurrentSound(openFileDialog1.FileName);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string file = findSelectedSound();
            if (!String.IsNullOrEmpty(file))
            {
                FileUtils.playAudioFile(file);
            }
        }
        private string findSelectedSound()
        {
            string selected = listBox1.SelectedItem.ToString();
            if (selected == "<None>")
                return null;

            string[] files = FileUtils.GetFiles(FileUtils.ExecutablePath + "\\sounds\\", "*.mp3|*.wav|*.m4a");
            foreach (string s in files)
            {
                FileInfo info = new FileInfo(s);
                if (FileUtils.TrimFilename(info.Name) == selected)
                {
                    return s;
                }
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selected = findSelectedSound();
            setCurrentSound(selected);
            
        }
        public void setCurrentSound(string filename)
        {
            if (!String.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                Properties.Settings.Default.currentSound = filename;
                soundLabel.Text = new FileInfo(Properties.Settings.Default.currentSound).Name;
            }else
            {
                Properties.Settings.Default.currentSound = "";
                soundLabel.Text = "";
            }
            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.ExitProgram();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.useNotification = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FileUtils.RegisterInStartup(checkBox2.Checked);
            Properties.Settings.Default.launchOnStart = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }
        internal void sendNotification()
        {

            string audioFile = Properties.Settings.Default.currentSound;
            if (!String.IsNullOrEmpty(audioFile))
            {
                FileUtils.playAudioFile(audioFile);
            }
            if (Properties.Settings.Default.useNotification)
            {
                notifyIcon1.ShowBalloonTip(2000, "Battery Alerter", "AC cable unplugged!", ToolTipIcon.Warning);
            }

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //If starts on windows start, hide window
            if (Properties.Settings.Default.firstLaunch)
            {
                Properties.Settings.Default.firstLaunch = false;
                Properties.Settings.Default.Save();
                return;
            }
            if (Properties.Settings.Default.launchOnStart)
            {
                HideWindow();
            }
        }
    }
}
