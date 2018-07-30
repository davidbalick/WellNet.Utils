using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellNet.Utils.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestConnMgr();
            //TestAddGuidToFilename();
            TestFileAssociation();
        }

        private static void TestFileAssociation()
        {
            Debug.WriteLine(FileExtensionInfo.ExtHasAppAssociation(".xlsx") ? "Yes" : "No");
        }

        private static void TestAddGuidToFilename()
        {
            var testFile = @"H:\Projects\CapitolTech\CTS WN Managed Services Proposal July 20 '18.pdf";
            UiLibrary.AppendGuidToFilename(testFile);
        }

        //private static void TestConnMgr()
        //{
        //    ConnectionManager.CreateInitialConnectionEntryFile();
        //    var connMgr = ConnectionManager.Create();
        //    foreach (var key in connMgr.Keys)
        //        Debug.WriteLine(string.Format("{0}: {1}", key, connMgr[key]));
        //}
    }
}
