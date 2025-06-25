using MessagePack;
using System.Data;
using System.IO;

namespace SQL_Extractor_for_Excel.Scripts
{

    [MessagePackObject]
    public class SE4EDTData
    {
        [MessagePack.Key(0)]
        public string DatabaseName { get; set; }

        [MessagePack.Key(1)]
        public string SqlQuery { get; set; }

        [MessagePack.Key(2)]
        public DataTable DataTable { get; set; }
    }

    public class DataTableExporter
    {
        public void ExportToSE4EDT(string filePath, string databaseName, string sqlQuery, DataTable dataTable)
        {
            var data = new SE4EDTData
            {
                DatabaseName = databaseName,
                SqlQuery = sqlQuery,
                DataTable = dataTable
            };

            var bytes = MessagePackSerializer.Serialize(data);
            File.WriteAllBytes(filePath, bytes);
        }

        public SE4EDTData ImportFromSE4EDT(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var data = MessagePackSerializer.Deserialize<SE4EDTData>(bytes);
            return data;
        }
    }
}
