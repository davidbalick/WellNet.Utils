using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WellNet.Utils
{
    public class ConnectionManager : Dictionary<string, string>
    {
        private static string CONNECTION_FILE = @"\\192.168.11.10\Shared\APPS\Common\Resources\Connections.dat";
        private static string TABLENAME = "ConnectionStrings";
        private static string NAME = "Name";
        private static string CONNSTR = "EncryptedConnectionString";
        private static ConnectionManager _cachedConnectionMgr = null;

        public static ConnectionManager Create()
        {
            return _cachedConnectionMgr ?? (_cachedConnectionMgr = new ConnectionManager());
        }

        private ConnectionManager()
        {
            var dataSet = new DataSet();
            using (TextReader tr = new StringReader(TripleDes.DecryptFileToString(CONNECTION_FILE)))
            {
                dataSet.ReadXml(tr);
            }
            foreach (DataRow row in dataSet.Tables[0].Rows)
                this[row[NAME].ToString()] = row[CONNSTR].ToString();
        }

        private static void AddConnectionEntry(ref DataTable dataTable, string unencryptedName, string unencryptedConnString)
        {
            var row = dataTable.NewRow();
            row[NAME] = unencryptedName;
            row[CONNSTR] = unencryptedConnString;
            dataTable.Rows.Add(row);
        }

        public static void AddOrModifyConnectionEntry(string unencryptedName, string unencryptedConnStr)
        {
            var dataSet = new DataSet();
            using (TextReader tr = new StringReader(TripleDes.DecryptFileToString(CONNECTION_FILE)))
            {
                dataSet.ReadXml(tr);
            }
            var dataTable = dataSet.Tables[0];
            var row = dataTable.Rows.Cast<DataRow>().FirstOrDefault(r => r[NAME].ToString() .Equals(unencryptedName));
            if (row == null)
            {
                row = dataTable.NewRow();
                row[NAME] = unencryptedName;
                row[CONNSTR] = unencryptedConnStr;
                dataTable.Rows.Add(row);
            }
            else
                row[CONNSTR] = unencryptedConnStr;
            TripleDes.EncryptStringToFile(dataSet.GetXml(), CONNECTION_FILE);
        }
    }
}
