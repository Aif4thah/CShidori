using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace CShidori
{
    public class SoapInjection
    {
        public string Input { get; set; }

        public SoapInjection(string param)
        {
            this.Input = param;

            List<string> results = new List<string>();
            XmlDocument wsdlDoc = new XmlDocument();
            wsdlDoc.Load(this.Input);
            //Console.WriteLine("wsdlDoc :" + wsdlDoc.OuterXml);
            Wsdl wsdl = new Wsdl(wsdlDoc);
            //Console.WriteLine("wsdl :" + wsdl.Messages);

            foreach (SoapService service in wsdl.Services)
            {

                foreach ( SoapPort port in service.Ports)
                {

                    SoapBinding binding = wsdl.Bindings.Single(b => b.Name == port.Binding.Split(":")[1]);
                    SoapPortType portType = wsdl.PortTypes.Single(pt => pt.Name == binding.Type.Split(':')[1]);

                    foreach (SoapBindingOperation op in binding.Operations)
                    {
                        /*
                        Console.Write("# => service Name : {0}", service.Name);
                        Console.WriteLine("# location: {0}", port.Location);
                        Console.WriteLine("# => port Name : {0}", port.Name);
                        Console.WriteLine("# => binding Name : {0}", binding.Name);
                        Console.WriteLine("#\tOperation: {0}", op.Name);
                        */

                        SoapOperation po = portType.Operations.Single(p => p.Name == op.Name);
                        SoapMessage input = wsdl.Messages.Single(m => m.Name == po.Input.Split(':')[1]);
                        XNamespace soapNS = "http://schemas.xmlsoap.org/soap/envelope/";
                        XNamespace xmlNS = "http://www.w3.org/2001/XMLSchema/";
                        try
                        {
                            xmlNS = op.SoapAction.Replace(op.Name, string.Empty);
                        }
                        catch
                        {
                            Console.WriteLine("# ERROR xmlns with: {0}", op.Name);
                        }
                        finally
                        {
                            Console.WriteLine("# DEBUG at the moment namespaces must be manualy verified");
                        }
 

                        XElement soapBody = new XElement(soapNS + "Body");
                        XElement soapOperation = new XElement(xmlNS + op.Name);
                        soapBody.Add(soapOperation); 
                        
                        foreach( SoapComplexType complext in wsdl.SoapComplexTypes )
                        {
                            if (op.Name == complext.ParentElementName)
                            {
                                foreach (KeyValuePair<string,string> kv in complext.ChildElement)
                                {
                                    XElement soapParam = new XElement(xmlNS + kv.Key);
                                    soapParam.SetValue("FUZZ");
                                    soapOperation.Add(soapParam);
                                }
                            }
                        }


                        /*
                        SoapType type = wsdl.Types.Single(t => t.Name == input.Parts[0].Element.Split(':')[1]);
                        foreach (SoapTypeParameter param in type.Parameters)
                        {
                            XElement soapParam = new XElement(xmlNS + param.Name);
                            soapParam.SetValue("FUZZ");
                            soapOperation.Add(soapParam);
                        }
                        */


                        XDocument soapDoc = new XDocument(new XDeclaration("1.0", "ascii", "true"),
                        new XElement(soapNS + "Envelope",
                            new XAttribute(XNamespace.Xmlns + "soap", soapNS),
                            new XAttribute("xmlns", xmlNS),
                            soapBody));

                       soapDoc.Save(Console.Out);
                       Console.WriteLine(String.Empty);

                    }
                }

            }
        }
    }
}