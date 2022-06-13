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
using System.Web;




namespace CShidori.MachineLearning
{
    internal class MLDataGen
    {
        public class MLData
        {
            public Guid uuid { get; set; }

            public string response { get; set; }
            public string request { get; set; }

            public Boolean vulnerable { get; set; }

        }

        public class MLDataWriter
        {
            public MLDataWriter(Guid u, string req, string rsp)
            {
                // Call from datalog class, guid ML = guid logs

                string MLDataPath = "./MachineLearning/MLData.csv";

                var data = new List<MLData> {
                    new MLData
                    {
                        uuid = u, 
                        request = MLAsciiEncode(req), 
                        response = MLAsciiEncode(rsp), 
                        vulnerable = false
                    }
                };

                if (!File.Exists(MLDataPath)) //create new file or append data
                {
                    using (var writer = new StreamWriter(MLDataPath))
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
                    using (var stream = File.Open(MLDataPath, FileMode.Append))
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        csv.WriteRecords(data);
                    }
                }

            }

            public static string MLAsciiEncode(string str)
            {
                string htmlASCIIEncoded = string.Empty; ;
                foreach (char c in str)
                {
                    try
                    {
                        int val = Convert.ToInt32(c);
                        htmlASCIIEncoded += val.ToString();

                    }
                    catch { }
                }
                return htmlASCIIEncoded;
            }

        }
    }
}
