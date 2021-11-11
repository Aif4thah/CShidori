using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CShidori
{
    class XSD
    {
        public List<SimpleElement> SimpleElements { get; set; }
        public List<ComplexType> ComplexTypes { get; set; }

        public XSD(XmlDocument doc)
        {
            string type = string.Empty;
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");


            //simple element
            this.SimpleElements = ParseSimpleElement(doc, nsManager);
            this.ComplexTypes = new List<ComplexType>();

            //ComplexTypes from elements
            foreach (SimpleElement s in this.SimpleElements)
            {

                this.ComplexTypes.Add(ParsecomplexType(doc, nsManager, s.name, s.type));
            }


            //ComplexTypes from ComplexeTypes
            List<ComplexType> tmp = new List<ComplexType>();
            List<ComplexType> oldthisComplexTypes = new List<ComplexType>();

            while (oldthisComplexTypes != this.ComplexTypes)
            {
                oldthisComplexTypes = this.ComplexTypes;
                foreach (ComplexType c in this.ComplexTypes)
                {
                    foreach (var k in c.parameters)
                    {
                        tmp.Add(ParsecomplexType(doc, nsManager, k.Key, k.Value)); // key = name, value = type
                    }
                }
                this.ComplexTypes.AddRange(tmp);
            }

        }



        private List<SimpleElement> ParseSimpleElement(XmlDocument doc, XmlNamespaceManager nsManager)
        {

            // simpleType
            List<SimpleElement> result = new List<SimpleElement>();
            string xpath = "/xsd:schema/xsd:element";
            string name = String.Empty;
            string type = String.Empty; ;
            XmlNodeList nodes = doc.DocumentElement.SelectNodes(xpath, nsManager);

            foreach (XmlNode n in nodes)
            {
                //Console.WriteLine(n.OuterXml);
                foreach (XmlAttribute a in n.Attributes)
                {
                    //Console.WriteLine(n.Name + ":" + a.Name + ":" + a.Value);
                    if (a.Name == "name")
                    {
                        name = a.Value;
                    }
                    else if (a.Name == "type")
                    {
                        type = a.Value;
                    }
                }
                if (name != String.Empty && type != String.Empty)
                {
                    //Console.WriteLine(n.Name + ":" + name + ":" + type);
                    result.Add(new SimpleElement(name, type));
                }

                name = String.Empty;
                type = String.Empty;


            }

            return result;
        }

        private ComplexType searchInNode(XmlNode node , ComplexType result)
        {
            string cname = String.Empty;
            string ctype = String.Empty; ;

            foreach (XmlNode child in node.ChildNodes) // usualy <sequence> in <complextype>
            {
                try
                {
                    foreach (XmlAttribute ca in child.Attributes) // if there is <attribute> with attributes
                    {

                        //Console.WriteLine(a.Value + ":" + ca.Name + ":" + ca.Value);
                        if (ca.Name == "name")
                        {
                            cname = ca.Value;
                        }
                        else if (ca.Name == "type")
                        {
                            ctype = ca.Value;
                        }

                    }

                if (cname != String.Empty && ctype != String.Empty)
                {
                    //Console.WriteLine("# DEBUG NewParameters:" + cname + " : " + ctype);
                    result.parameters.Add(cname, ctype);
                }
                }catch { }
                cname = String.Empty;
                ctype = String.Empty;
            }

         return result;
        }

        private ComplexType ParsecomplexType(XmlDocument doc, XmlNamespaceManager nsManager, string name, string type)
        {
            //complexType
            ComplexType result = new ComplexType(name, type);
            string cxpath = "/xsd:schema/xsd:complexType";
            XmlNodeList cnodes = doc.DocumentElement.SelectNodes(cxpath, nsManager);

            foreach (XmlNode cn in cnodes)
            {
                //Console.WriteLine(n.InnerXml);
                foreach (XmlAttribute a in cn.Attributes) // attribute of complexType node
                {
                    //Console.WriteLine("# DEBUG " + a.Value + "==" + type);
                    if (type.Contains(a.Value))
                    {
                        //Console.WriteLine("# DEBUG [OK] " + a.Value + "==" + type);
                        foreach (XmlNode secondn in cn.ChildNodes) // usualy <sequence> in <complextype>
                        {
                                result = searchInNode(secondn, result);
                            foreach (XmlNode thirdn in secondn.ChildNodes) //<element> in <sequence>
                            {
                                result = searchInNode(thirdn, result);

                                //specific case (<choice> etc...)
                                foreach (XmlNode fourthn in thirdn.ChildNodes) //<element> in <sequence>
                                {
                                    result = searchInNode(fourthn, result);
                                }
                            }
                        }
                    }

                }

            }
            return result;
        }
    }



    class SimpleElement
    {
        public string name { get; set; }
        public string type { get; set; }
        public SimpleElement(string name, string type)
        {

            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            return "[Name: " + this.name + "Type: " + this.type + "]";
        }

    }



    class ComplexType
    {
        public string name { get; set; }
        public string type { get; set; }
        public Dictionary<string, string> parameters { get; set; }
        public ComplexType(string name, string type)
        {
            this.name = name;
            this.type = type;
            this.parameters = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            string str = this.name + "(" + this.type + ")";
            foreach (var p in this.parameters)
            {
                str += " [Name: " + p.Key + "Type: " + p.Value + "]";
            }
            return str;
        }

    }
}