using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CShidori.MachineLearning;
using CsvHelper.Configuration;
using System.Globalization;

namespace CShidori.DataHandler
{
    public class DataLog
    {
        public Guid uuid { get; set; }
        public string response { get; set; }
        public string request { get; set; }
        public int responseLenght { get; set; }

    }

    public class DataLogWriter
    {
        public DataLogWriter(string f, Guid u, string req, string rsp)
        {

            string LogPath = "./Log/" + f.Replace(".", "-") + ".csv";

            var data = new List<DataLog> { 
                new DataLog
                {
                    uuid = u, request = req, response = rsp, responseLenght = rsp.Length
                }               
            };

            if (!File.Exists(LogPath)) //create new file or append data
            {
                using (var writer = new StreamWriter(LogPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }
            }
            else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {                   
                    HasHeaderRecord = false // Don't write the header again.
                };
                using (var stream = File.Open(LogPath, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(data);
                }
            }

            //beta: send data to ML
            new MLDataGen.MLDataHandler().MLDataWriter(u, req, rsp, true); // set last param to true to enable prediction            

        }

    }
}
