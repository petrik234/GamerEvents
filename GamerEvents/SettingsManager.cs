using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GamerEvents
{
    public class SettingsManager// : ISettingsManager
    {
        public string PersonalFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        // Write Information to a Local File
        public void WriteLocalFile(string FileName, string Data)
        {
            string filePath = Path.Combine(PersonalFolderPath, FileName);
            File.WriteAllText(filePath, Data);
        }

        public void DeleteLocalFile(string FileName)
        {
            string filePath = Path.Combine(PersonalFolderPath, FileName);
            try
            {
                File.Delete(filePath);
            }
            catch
            { }
            
        }

        // Load Information from a Local File
        public string LoadLocalFile(string FileName)
        {
            string filePath = Path.Combine(PersonalFolderPath, FileName);
            if (File.Exists(filePath)) return File.ReadAllText(filePath);
            return null;
        }

        // File Exists Check
        public bool DoesFileExist(string FileName)
        {
            if (File.Exists(FileName)) return true;
            else return false;
        }

    }
}