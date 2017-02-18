using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BatteryAlert
{
    class FileUtils
    {
        static public readonly string ExecutablePath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

        public static void playAudioFile(string url)
        {
            WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            player.URL = url;
            player.controls.play();
        }

        public static string TrimFilename(string str)
        {
            int index = str.LastIndexOf('.');
            if (index == -1)
                return str;
            else
            {
                return str.Substring(0, index);
            }
        }

        public static string[] GetFiles(string SourceFolder, string Filter)
        {
            // ArrayList will hold all file names
            ArrayList alFiles = new ArrayList();

            // Create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            // for each filter find mathing file names
            foreach (string FileFilter in MultipleFilters)
            {
                // add found file names to array list
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter));
            }

            // returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }

        //Register application in registry for windows start
        public static void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("BatteryAlerter", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("BatteryAlerter");
            }
        }
    }
}
