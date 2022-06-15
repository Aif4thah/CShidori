using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Generic;
using CShidori.DataHandler;

namespace CShidori.Core
{
    public class BadStrings
    {
        public List<string> Output { get; set; }

        public BadStrings(string d)
        {
            List<string> results = new DataLoader().Dataloader(d);
            results = EncodeStrings.encodebadchars(results); //results.Distinct().ToList();
            this.Output = results;
        }       
    }
}
