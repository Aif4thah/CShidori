using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CShidori.DataHandler;


namespace CShidori.Generator
{
    public class GetInjection
    {
        public string InputGetparam { get; set; }
        public string InputInj { get; set; }

        public GetInjection(string param1, string param2)
        {
            InputGetparam = param1;
            InputInj = param2;

            List<string> results = new List<string>();
            string parameters = InputGetparam.Split("?")[1];
            Dictionary<string, string> dparameters = new Dictionary<string, string>();
            string getstr;

            if (parameters.Contains("&"))
            {
                foreach (string pv in parameters.Split("&"))
                    dparameters.Add(pv.Split("=")[0], pv.Split("=")[1]);
            }
            else
            {
                dparameters.Add(parameters.Split("=")[0], parameters.Split("=")[1]);
            }

            foreach (KeyValuePair<string, string> kv in dparameters)
            {
                getstr = parameters.Replace(kv.Key + "=" + kv.Value, kv.Key + "=" + InputInj);
                results.Add(getstr);

                // HTTP parameters polution
                results.Add(parameters + "&" + getstr);
                results.Add(parameters.Replace(kv.Key + "=" + kv.Value, kv.Key + "=" + kv.Value + "," + InputInj));
            }
            Console.WriteLine(string.Join("\n", results));
        }

    }
}
