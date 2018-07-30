using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace WellNet.Utils
{
    public static class UiLibrary
    {
        public static string TryGetExcelFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx|All Files|*.*",
                FilterIndex = 1
            };
            if (openFileDialog.ShowDialog() != true)
                return null;
            return openFileDialog.FileName;
        }

        public static string TryToStart(string fileName)
        {
            if (!File.Exists(fileName))
                return string.Format("{0}\ndoes not exist.", fileName);
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public static void AppendGuidToFilename(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);
            var nameWoExt = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            nameWoExt = string.Format("{0}_{1}", nameWoExt, Guid.NewGuid());
            var nameWoPath = string.Format("{0}{1}", nameWoExt, ext);
            var newName = Path.Combine(dir, nameWoPath);
            File.Move(fileName, newName);
        }
    }
}
