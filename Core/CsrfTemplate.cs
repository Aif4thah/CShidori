using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class CsrfTemplate
    {
        string target { get; set; }
        string parameters { get; set;}

        public CsrfTemplate( string method, string target, string parameters)
        {
            this.target = target;
            this.parameters = parameters;


            if(method.ToLower() == "get")
            {
                getcsrf();
            }
            else if ( method.ToLower() == "post")
            {
                postcsrf();
            }
            else
            {
                Console.WriteLine("no template for http method {0}", method);
            }
        }

        private void getcsrf()
        {
            string csrf = @"
<html>
    <body>
        <img src=""§"">
    </body>
</html>
";
            Console.WriteLine(csrf.Replace("§", this.target + "?" + this.parameters));
        }




        private void postcsrf()
        {
            Dictionary<string, string> dparameters = new Dictionary<string, string>();

            List<string> data = new DataLoader().Dataloader("CsrfTemplate");
            string csrf = String.Join("\n", data);

            string FormParams = @"     <input type=""hidden"" name=""§NAME§"" value=""§VAMUE§"">
";
            string formPrep = string.Empty;

            if (this.parameters.Contains("&"))
            {
                foreach (string pv in this.parameters.Split("&"))
                    dparameters.Add(pv.Split("=")[0], pv.Split("=")[1]);
            }
            else
            {
                dparameters.Add(parameters.Split("=")[0], parameters.Split("=")[1]);
            }

            foreach (KeyValuePair<string, string> kv in dparameters)
                formPrep += FormParams.Replace("§NAME§", kv.Key).Replace("§VAMUE§", kv.Value);

            csrf = csrf.Replace("§PARAMETERS§", formPrep).Replace("§TARGET§", this.target);

            Console.WriteLine(csrf);
        }
    }
}
