using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WellNet.Utils
{
    public static class DataToFile
    {
        private static Dictionary<string, string> _typeToSqlTypes;
        private static Dictionary<string, string> TypeToSqlTypes
        {
            get { return _typeToSqlTypes ?? (_typeToSqlTypes = GetSqlTypes()); }
        }

        private static Dictionary<string, string> GetSqlTypes()
        {
            var result = new Dictionary<string, string>();
            result["Int64"] = "BigInt";
            result["Decimal"] = "Money";
            result["String"] = "VarChar";
            result["bool"] = "Bit";
            result["Char"] = "Char";
            result["DateTime"] = "DateTime";
            result["Int"] = "Int";
            result["Int32"] = "Int";
            result["Guid"] = "UniqueIdentifier";
            return result;
        }

        public static void TabDelimited(DataTable dataTable, string fileName)
        {
            const string MASK = "{0}\t{1}";
            var tempFile = Path.GetTempFileName();
            using (TextWriter tw = new StreamWriter(tempFile))
            {
                tw.WriteLine(dataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName)
                    .Aggregate((a, b) => string.Format(MASK, a, b)));
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var data = dataRow.ItemArray.Select(o => o.ToString().Trim().Replace("\t", string.Empty)).ToArray();
                    tw.WriteLine(data.Aggregate((a, b) => string.Format(MASK, a, b)));
                }
            }
            File.Delete(fileName);
            File.Move(tempFile, fileName);
        }
        public static void SqlScript(DataTable dataTable, string fileName, string tableName="{TableName}")
        {
            var tempFile = Path.GetTempFileName();
            var dataColumns = dataTable.Columns.Cast<DataColumn>().ToArray();
            using (TextWriter tw = new StreamWriter(tempFile))
            {
                tw.WriteLine(GetDropCode(tableName));
                tw.WriteLine("CREATE TABLE [{0}] (", tableName);
                tw.WriteLine(dataColumns.Select(c => GetColDefArg(c)).Aggregate((a, b) => string.Format("{0},\r\n{1}",a,b)));
                tw.WriteLine(")");
                var insertStart = string.Format("INSERT INTO {0} ({1}) VALUES (", tableName,
                    dataColumns.Select(dc => dc.ColumnName).Select(c => string.Format("[{0}]",c)).Aggregate((a, b) => string.Format("{0}, {1}", a, b)));
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var data = dataColumns.Select(dc => DataToInsertArg(dataRow, dc));
                    tw.WriteLine(insertStart);
                    tw.WriteLine(data.Aggregate((a, b) => string.Format("{0}, {1}", a, b)) + ")");
                }
            }
            File.Delete(fileName);
            File.Move(tempFile, fileName);
        }

        private static string GetDropCode(string tableName)
        {
            var objIdArg = string.Format("{0}{1}", tableName.StartsWith("#") ? "tempdb.." : string.Empty, tableName);
            return string.Format("IF OBJECT_ID('{0}') IS NOT NULL DROP TABLE {1}", objIdArg, tableName);
        }

        private static string DataToInsertArg(DataRow dataRow, DataColumn dataCol)
        {
            var data = dataRow[dataCol];
            if (data == DBNull.Value)
                return "NULL";
            var result = data.ToString().Trim().Replace("\t", string.Empty).Replace("'", "''");
            if (string.IsNullOrEmpty(result))
                return "NULL";
            if (DataTypeNeedsQuotes(dataCol.DataType.ToString()))
                return string.Format("'{0}'", result);
            return result;
        }
        private static string GetColDefArg(DataColumn c)
        {
            return string.Format("\t[{0}] {1}", c.ColumnName, GetDataTypeArg(c.DataType.ToString()));
        }

        private static string GetDataTypeArg(string dataType)
        {
            dataType = dataType.Replace("System.", string.Empty);
            string sqlDt;
            if (TypeToSqlTypes.ContainsKey(dataType))
                sqlDt = TypeToSqlTypes[dataType.ToString()];
            else
                sqlDt = string.Format("!{0}!", dataType);
            var len = 0;
            if (sqlDt == "VarChar")
                len = 100;
            if (sqlDt == "Char")
                len = 10;
            return string.Format("{0}{1}", sqlDt, len == 0 ? string.Empty : string.Format("({0})", len));
        }

        private static bool DataTypeNeedsQuotes(string dataType)
        {
            if (!TypeToSqlTypes.ContainsKey(dataType))
                return true;
            return (new[] { "String", "Char", "DateTime", "Guid" }).Contains(dataType);
        }
    }
}
