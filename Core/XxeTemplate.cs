﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class XxeTemplate
    {

        string target { get; set; }
        public XxeTemplate(string TemplateType)
        {
            this.target = Misc.GetIp();
            List<string> XXE = new DataLoader().Dataloader("XxeTemplate");
            foreach(string l in XXE)
                Console.WriteLine(l.Replace("§", this.target).Replace("foo", Misc.RandomString(3)));

        }
    }
}