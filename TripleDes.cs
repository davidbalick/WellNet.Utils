using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WellNet.Utils
{
    public static class TripleDes
    {
        private static byte[] _key;
        private static byte[] Key
        {
            set { _key = value; }
            get
            {
                if (_key != null)
                    return _key;
                GetKeyAndIv();
                return _key;
            }
        }
        private static byte[] _Iv;
        private static byte[] Iv
        {
            set { _Iv = value; }
            get
            {
                if (_Iv != null)
                    return _Iv;
                GetKeyAndIv();
                return _Iv;
            }
        }

        private static void GetKeyAndIv()
        {
            var lines = File.ReadAllLines(@"\\192.168.11.10\shared\apps\common\resources\td.txt");
            _key = lines[0].Split(',').Select(i => Convert.ToByte(i.Trim())).ToArray();
            _Iv = lines[1].Split(',').Select(i => Convert.ToByte(i.Trim())).ToArray();
        }

        private static readonly TripleDESCryptoServiceProvider MDes = new TripleDESCryptoServiceProvider();

        public static void EncryptFileToFile(string inputFile, string outputfile)
        {
            var output = Transform(File.ReadAllBytes(inputFile), MDes.CreateEncryptor(Key, Iv));
            File.WriteAllBytes(outputfile, output);
        }

        public static void DecryptFileToFile(string inputFile, string outputfile)
        {
            var output = Transform(File.ReadAllBytes(inputFile), MDes.CreateDecryptor(Key, Iv));
            File.WriteAllBytes(outputfile, output);
        }

        public static void EncryptStringToFile(string inputString, string outputfile)
        {
            var output = Transform(Encoding.ASCII.GetBytes(inputString), MDes.CreateEncryptor(Key, Iv));
            File.WriteAllBytes(outputfile, output);
        }

        public static string DecryptFileToString(string inputFile)
        {
            return Encoding.ASCII.GetString(Transform(File.ReadAllBytes(inputFile), MDes.CreateDecryptor(Key, Iv)));
        }

        public static string EncryptString(string inputString)
        {
            return Encoding.ASCII.GetString(Transform(Encoding.ASCII.GetBytes(inputString), MDes.CreateEncryptor(Key, Iv)));
        }

        public static string DecryptString(string inputString)
        {
            return Encoding.ASCII.GetString(Transform(Encoding.ASCII.GetBytes(inputString), MDes.CreateDecryptor(Key, Iv)));
        }

        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            using (var memStream = new MemoryStream())
            {
                using (var cryptStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptStream.Write(input, 0, input.Length);
                    cryptStream.FlushFinalBlock();
                    memStream.Position = 0;
                    var result = memStream.ToArray();
                    memStream.Close();
                    cryptStream.Close();
                    return result;
                }
            }
        }
    }
}
