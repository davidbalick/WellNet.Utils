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
            //TestFileAssociation();
            TestEmail();
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

        private static void EncryptEmail()
        {
            TripleDes.EncryptFileToFile(@"\\192.168.11.10\shared\apps\common\resources\emailauth.txt", @"\\192.168.11.10\shared\apps\common\resources\emailauth.dat");
        }

        private static void TestEmail()
        {
            EMailer.Email("Hi There", "davidbalick@gmail.com", "The rain in spain");
        }


    }
}
