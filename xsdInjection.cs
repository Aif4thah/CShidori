using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace CShidori
{
    public class xsdInjection
    {
        public string Input { get; set; }
        public xsdInjection(string param)
        {
            this.Input = param;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(this.Input);
            XSD xsd = new XSD(xmldoc);

            //Console.WriteLine( String.Join("\n", xsd.SimpleElements));
            //Console.WriteLine( String.Join("\n", xsd.ComplexTypes));

            createxml(xsd);
        }

        private void createxml(XSD xsd)
        {
            XmlDocument outdoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = outdoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = outdoc.DocumentElement;
            outdoc.InsertBefore(xmlDeclaration, root);

            foreach (ComplexType c in xsd.ComplexTypes)
            {

                XmlElement eC = outdoc.CreateElement(string.Empty, c.name.Replace("tns:", ""), string.Empty);
                try
                {
                    outdoc.AppendChild(eC);
                }
                catch
                {
                    XmlNode refE = outdoc.DocumentElement.LastChild;
                    outdoc.DocumentElement.AppendChild(eC);
                }


                foreach (var p in c.parameters)
                {
                    if (p.Value.ToLower().Contains("string"))
                    {
                        XmlElement e2 = outdoc.CreateElement(string.Empty, p.Key, string.Empty);
                        XmlText text = outdoc.CreateTextNode("FUZZ");
                        e2.AppendChild(text);
                        eC.AppendChild(e2);


                    }
                    else if (p.Value.ToLower().Contains("integer"))
                    {
                        XmlElement e2 = outdoc.CreateElement(string.Empty, p.Key, string.Empty);
                        XmlText text = outdoc.CreateTextNode("1111");
                        e2.AppendChild(text);
                        eC.AppendChild(e2);
                    }
                    else
                    {
                        XmlElement e2 = outdoc.CreateElement(string.Empty, p.Key, string.Empty);
                        XmlText text = outdoc.CreateTextNode("(type?)");
                        e2.AppendChild(text);
                        eC.AppendChild(e2);
                    }
                }


                foreach (SimpleElement s in xsd.SimpleElements)
                {
                    //Console.WriteLine("# DEBUG add simple type "+  s.name );
                    try
                    {
                        if (s.type.ToLower().Contains("string"))
                        {
                            XmlElement eS = outdoc.CreateElement(string.Empty, s.name, string.Empty);
                            XmlText text = outdoc.CreateTextNode("FUZZ");
                            eS.AppendChild(text);
                            outdoc.AppendChild(eS);
                        }
                        else if (s.type.ToLower().Contains("integer"))
                        {
                            XmlElement eS = outdoc.CreateElement(string.Empty, s.name, string.Empty);
                            XmlText text = outdoc.CreateTextNode("1111");
                            eS.AppendChild(text);
                            outdoc.AppendChild(eS);
                        }

                    }catch{ }
                }

            }

            outdoc.Save(Console.Out);

        }
    }
}
