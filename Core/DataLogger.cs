using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CShidori.Core
{
    public class DataLogger
    {
        public Guid uuid { get; set; }

        public string response { get; set; }
        public string request { get; set; }

    }

    public class DataLoggerWriter
    {
        public DataLoggerWriter(string f, Guid u, string req, string rsp)
        {
            var data = new DataLogger{ uuid = u, request = req, response = rsp };
            string jsonStr = JsonSerializer.Serialize(data);

            File.AppendAllText("./Log/" + f.Replace(".","-") + ".json", jsonStr + "\n") ;

        }

    }
}
