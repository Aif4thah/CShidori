using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CShidori.DataHandler;


namespace CShidori.Generator
{
    public class JsonInjection
    {
        public string InputJson { get; set; }
        public string InputInj { get; set; }

        public JsonInjection(string param1, string param2)
        {
            InputJson = param1;
            InputInj = param2;

            List<string> results = new List<string>();
            JToken old;
            JObject json = JObject.Parse(File.ReadAllText(InputJson));

            foreach (JProperty p in json.DeepClone())
            {
                old = json[p.Name];
                json[p.Name] = InputInj;
                results.Add(json.ToString().Replace("\r\n", ""));
                json[p.Name] = old;
            }


            if (int.TryParse(InputInj, out int v))   //if inupt is int
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
