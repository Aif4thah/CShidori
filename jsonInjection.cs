using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace CShidori
{
    public class jsonInjection
    {
        public string InputJson { get; set; }
        public string InputInj { get; set; }

        public jsonInjection(string param1, string param2)
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

            //intger
            if (int.TryParse(this.InputInj, out int v)){

                foreach (JProperty p in json.DeepClone())
                {
                    old = json[p.Name];
                    json[p.Name] = v;
                    results.Add(json.ToString().Replace("\r\n", ""));
                    json[p.Name] = old;
                }
            }
            Console.WriteLine(String.Join("\n", results));
        }

    }
}
