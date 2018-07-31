using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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

        public static string TimeElapsed(TimeSpan timeSpan, bool showMilliseconds)
        {
            var list = new List<string>();
            var timeParts = new string[(showMilliseconds) ? 5 : 4];
            timeParts[0] = "day";
            timeParts[1] = "hour";
            timeParts[2] = "minute";
            timeParts[3] = "second";
            if (showMilliseconds) timeParts[4] = "millisecond";
            for (var t = 0; t < timeParts.Length; t++)
            {
                var text = ElapsedPart(timeParts[t], GetTimeValue(t, timeSpan));
                if (text != null) list.Add(text);
            }
            for (var t = 0; t < list.Count - 2; t++) list[t] = string.Format("{0}, ", list[t]);
            if (list.Count > 1) list.Insert(list.Count - 1, " and ");
            var sb = new StringBuilder();
            for (var t = 0; t < list.Count; t++) sb.Append(list[t]);
            return sb.ToString();
        }

        private static int GetTimeValue(int timePart, TimeSpan timeSpan)
        {
            var result = 0;
            switch (timePart)
            {
                case 0:
                    result = timeSpan.Days;
                    break;
                case 1:
                    result = timeSpan.Hours;
                    break;
                case 2:
                    result = timeSpan.Minutes;
                    break;
                case 3:
                    result = timeSpan.Seconds;
                    break;
                case 4:
                    result = timeSpan.Milliseconds;
                    break;
            }
            return result;
        }

        private static string ElapsedPart(string partName, int numberOf)
        {
            string result = null;
            if (numberOf > 0)
                result = string.Format("{0} {1}{2}", numberOf, partName, (numberOf != 1) ? "s" : "");
            return result;
        }
    }
}
