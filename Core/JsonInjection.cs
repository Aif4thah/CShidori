using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CShidori.DataHandler;


namespace CShidori.Core
{
    public class JsonInjection
    {
        public string InputJson { get; set; }
        public string InputInj { get; set; }

        public JsonInjection(string param1, string param2)
        {
            this.InputJson = param1;
            this.InputInj = param2;

            List<string> results = new List<string>();
            JToken old;
            JObject json = JObject.Parse(File.ReadAllText(this.InputJson));

            foreach (JProperty p in json.DeepClone())
            {
                old = json[p.Name];
                json[p.Name] = this.InputInj;
                results.Add(json.ToString().Replace("\r\n",""));
                json[p.Name] = old;
            }


            if (int.TryParse(this.InputInj, out int v))   //if inupt is int
            {
                foreach (JProperty p in json.DeepClone())
                {
                    old = json[p.Name];
                    json[p.Name] = v;
                    results.Add(json.ToString().Replace("\r\n", ""));
                    json[p.Name] = old;
                }
            }
            results.ForEach(x => Console.WriteLine(x));
        }

    }
}
