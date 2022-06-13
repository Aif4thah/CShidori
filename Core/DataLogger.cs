using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CShidori.Core
{
    public class DataLogger
    {
        public Guid uuid { get; set; }

        public string response { get; set; }
        public string request { get; set; }

        public bool vulnerable { get; set; } // for Machine Learning Training: true will set manually

    }

    public class DataLoggerWriter
    {
        public DataLoggerWriter(string f, Guid u, string req, string rsp)
        {

            string LogPath = "./Log/" + f.Replace(".", "-") + ".csv";

            var data = new List<DataLogger> { 
                new DataLogger
                {
                    uuid = u, request = req, response = rsp, vulnerable = false
                }               
            };

            if (!File.Exists(LogPath))
            {
                // create file
                using (var writer = new StreamWriter(LogPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }
            }
            else
            {
                // Append to the file.
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




        }

    }
}
