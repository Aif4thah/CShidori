using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using CShidori.DataHandler;

namespace CShidori.Generator
{
    public class XmlInjection
    {
        public string InputXml { get; set; }
        public string InputInj { get; set; }

        public XmlInjection(string p, string i)
        {
            InputXml = p;
            InputInj = i;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(InputXml);

            XmlNodeList nodes = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlNode n in nodes)
            {
                inject(n, xmldoc);

                if (n.HasChildNodes)
                {
                    foreach (XmlNode n2 in n.ChildNodes)
                    {
                        inject(n2, xmldoc);

                        if (n2.HasChildNodes)
                        {
                            foreach (XmlNode n3 in n2.ChildNodes)
                                inject(n3, xmldoc);

                        }
                    }
                }

            }
        }

        private void inject(XmlNode n, XmlDocument xmldoc)
        {
            if (n.InnerXml == string.Empty)
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                //replace value
                string old = n.InnerText;
                n.InnerText = InputInj;

                //write in oneline
                xmldoc.WriteTo(xmlTextWriter);
                Console.WriteLine(stringWriter);

                //restore xml
                n.InnerText = old;

            }


        }

    }
}
