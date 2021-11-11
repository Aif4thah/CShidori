using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace CShidori
{
    public class xmlInjection
    {
        public string InputXml { get; set; }
        public string InputInj { get; set; }

        public xmlInjection( string param1, string param2 )
        {
            this.InputXml = param1;
            this.InputInj = param2;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(this.InputXml);
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            XmlNodeList nodes = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlNode n in nodes)
            {
                inject(n, xmldoc);

                if (n.HasChildNodes)
                {
                    foreach ( XmlNode n2 in n.ChildNodes)
                    {
                        inject(n2, xmldoc);

                        if (n2.HasChildNodes)
                        {
                            foreach (XmlNode n3 in n2.ChildNodes)
                            {
                                inject(n3, xmldoc);
                            }
                        }
                    }
                }

            }
        }

        private void inject( XmlNode n , XmlDocument xmldoc)
        {
            if( n.InnerXml == string.Empty)
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                //replace value
                string old = n.InnerText;
                n.InnerText = this.InputInj;

                //write in oneline
                xmldoc.WriteTo(xmlTextWriter);
                Console.WriteLine(stringWriter);
                stringWriter = new StringWriter();
                xmlTextWriter = new XmlTextWriter(stringWriter);

                //restore xml
                n.InnerText = old;

            }

 
        }

    }
}
