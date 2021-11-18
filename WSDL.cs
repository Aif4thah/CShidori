using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace CShidori
{

	/*
	 * Modified Class from: 
	 * 
	 * Copyright (c) 2017, Brandon Perry
 	 * All rights reserved.
	 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 	 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
	 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
	 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	 */
	public class Wsdl
	{
		public Wsdl(XmlDocument doc)
		{
			XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
			nsManager.AddNamespace("wsdl", doc.DocumentElement.NamespaceURI);
			nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

			ParseTypes(doc, nsManager);
			ParseMessages(doc, nsManager);
			ParsePortTypes(doc, nsManager);
			ParseBindings(doc, nsManager);
			ParseServices(doc, nsManager);
			ParseServices(doc, nsManager);
			//modif
			ParseComplexeTypes(doc, nsManager);

		}

		public List<SoapType> Types { get; set; }
		public List<SoapMessage> Messages { get; set; }
		public List<SoapPortType> PortTypes { get; set; }
		public List<SoapBinding> Bindings { get; set; }
		public List<SoapService> Services { get; set; }

		//modif
		public List<SoapComplexType> SoapComplexTypes { get; set; }


		private void ParseComplexeTypes(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.Types = new List<SoapType>();
			string xpath = "/wsdl:definitions/wsdl:types/xs:schema";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			this.SoapComplexTypes = new List<SoapComplexType>();
			foreach (XmlNode elem in nodes)
			{
				foreach( XmlNode type in elem.ChildNodes)
                {
					/*
					Console.WriteLine("# DEBUG ComplexeType name: " + type.Attributes["name"].Value);
					Console.WriteLine("# DEBUG ComplexeType: innerxml " + type.InnerXml);
					*/
					try
					{
						this.SoapComplexTypes.Add(new SoapComplexType(type));
					}
					catch
					{
						Console.WriteLine("# ERROR SoapComplexType with {0}", type.InnerXml);
					}
				}

			}
		}
		//Endmodif


		private void ParseTypes(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.Types = new List<SoapType>();
			string xpath = "/wsdl:definitions/wsdl:types/xs:schema/xs:element";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			foreach (XmlNode type in nodes)
			{
				this.Types.Add(new SoapType(type));
			}
		}

		private void ParseMessages(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.Messages = new List<SoapMessage>();
			string xpath = "/wsdl:definitions/wsdl:message";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			foreach (XmlNode node in nodes)
			{
				//Console.WriteLine("#Message: " + node.InnerXml);
				this.Messages.Add(new SoapMessage(node));
			}
		}

		private void ParsePortTypes(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.PortTypes = new List<SoapPortType>();
			string xpath = "/wsdl:definitions/wsdl:portType";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			foreach (XmlNode node in nodes)
			{
				//Console.WriteLine("#PortType: " + node.InnerXml);
				this.PortTypes.Add(new SoapPortType(node));
			}
		}

		private void ParseBindings(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.Bindings = new List<SoapBinding>();
			string xpath = "/wsdl:definitions/wsdl:binding";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			foreach (XmlNode node in nodes)
			{
				//Console.WriteLine("#Binding: " + node.InnerXml);
				this.Bindings.Add(new SoapBinding(node));
			}

		}

		private void ParseServices(XmlDocument wsdl, XmlNamespaceManager nsManager)
		{
			this.Services = new List<SoapService>();
			string xpath = "/wsdl:definitions/wsdl:service";
			XmlNodeList nodes = wsdl.DocumentElement.SelectNodes(xpath, nsManager);
			foreach (XmlNode node in nodes)
			{
				//Console.WriteLine("#Service: " + node.InnerXml);
				this.Services.Add(new SoapService(node));
			}

		}
	}
	public class SoapTypeParameter
	{
		public SoapTypeParameter(XmlNode node)
		{
			try {
				if (node.Attributes["maxOccurs"].Value == "unbounded")
					this.MaximumOccurence = int.MaxValue;
				else
					this.MaximumOccurence = int.Parse(node.Attributes["maxOccurs"].Value);

				this.MinimumOccurence = int.Parse(node.Attributes["minOccurs"].Value);
				this.Name = node.Attributes["name"].Value;
				this.Type = node.Attributes["type"].Value;
			}
			catch
			{
				Console.WriteLine("# ERROR: SoapTypeParameter(XmlNode node) with {0}", node.InnerXml);
			}
		}

		public int MinimumOccurence { get; set; }
		public int MaximumOccurence { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
	}

	//modif
	public class SoapComplexType
    {
		public string ParentElementName { get; set; }
		public Dictionary<string,string> ChildElement { get; set; }

		public SoapComplexType(XmlNode type)
        {
			this.ParentElementName = type.Attributes["name"].Value;
			this.ChildElement = new Dictionary<string, string>();

			foreach (XmlNode comp in type.ChildNodes)
			{
				foreach (XmlNode seq in comp.ChildNodes)
				{
					foreach (XmlNode p in seq.ChildNodes)
					{
						//Console.WriteLine("# DEBUG ComplexType: {0} -> arguments found: {1} (type: {2} ) ", this.ParentElementName, p.Attributes["name"].Value,  p.Attributes["type"].Value);
						this.ChildElement.Add(p.Attributes["name"].Value, p.Attributes["type"].Value);
					}
				}
			}
		}
    }

	//endModif

	public class SoapType
	{
		public SoapType(XmlNode type)
		{
			try
			{
				this.Name = type.Attributes["name"].Value;
				this.Parameters = new List<SoapTypeParameter>();

				if (type.HasChildNodes && type.FirstChild.HasChildNodes)
				{
					foreach (XmlNode node in type.FirstChild.FirstChild.ChildNodes)
						this.Parameters.Add(new SoapTypeParameter(node));
				}
            }
            catch
            {
				Console.WriteLine("# ERROR: SoapType(XmlNode type) with {0}", type.InnerXml);
			}
		}

		public string Name { get; set; }
		public List<SoapTypeParameter> Parameters { get; set; }
	}

	public class SoapService
	{
		public SoapService(XmlNode node)
		{
			try
			{
				this.Name = node.Attributes["name"].Value;
				this.Ports = new List<SoapPort>();

				foreach (XmlNode port in node.ChildNodes)
					this.Ports.Add(new SoapPort(port));
			}
			catch
			{
				Console.WriteLine("# ERROR: SoapService(XmlNode node) with {0}", node.InnerXml);
			}
		}

		public string Name { get; set; }
		public List<SoapPort> Ports { get; set; }
	}

	public class SoapPortType
	{
		public SoapPortType(XmlNode node)
		{
			try
			{
				this.Name = node.Attributes["name"].Value;
				this.Operations = new List<SoapOperation>();
				foreach (XmlNode op in node.ChildNodes)
					this.Operations.Add(new SoapOperation(op));
			}
			catch
			{
				Console.WriteLine("# ERROR: SoapPortType(XmlNode node) with {0}", node.InnerXml);
			}
		}

		public string Name { get; set; }

		public List<SoapOperation> Operations { get; set; }

	}

	public class SoapPort
	{
		public SoapPort(XmlNode port)
		{
			try
			{
				this.Name = port.Attributes["name"].Value;
				this.Binding = port.Attributes["binding"].Value;
				this.ElementType = port.FirstChild.Name;
				this.Location = port.FirstChild.Attributes["location"].Value;
			}
			catch
			{
				Console.WriteLine("# ERROR: SoapPort(XmlNode port) with {0}", port.InnerXml);
			}
		}

		public string Name { get; set; }
		public string Binding { get; set; }
		public string ElementType { get; set; }
		public string Location { get; set; }
	}

	public class SoapOperation
	{
		public SoapOperation(XmlNode op)
		{
			this.Name = op.Attributes["name"].Value;

			foreach (XmlNode message in op.ChildNodes)
			{
				if (message.Name.EndsWith("input"))
					this.Input = message.Attributes["message"].Value;
				else if (message.Name.EndsWith("output"))
					this.Output = message.Attributes["message"].Value;
			}
		}

		public string Name { get; set; }
		public string Input { get; set; }
		public string Output { get; set; }
	}

	public class SoapMessagePart
	{
		public SoapMessagePart(XmlNode part)
		{
			this.Name = part.Attributes["name"].Value;

			if (part.Attributes["element"] != null)
				this.Element = part.Attributes["element"].Value;
			else if (part.Attributes["type"] != null)
				this.Type = part.Attributes["type"].Value;
			else
				throw new ArgumentException("Neither element nor type attribute exist", nameof(part));
		}

		public string Name { get; set; }
		public string Element { get; set; }
		public string Type { get; set; }
	}

	public class SoapMessage
	{
		public SoapMessage(XmlNode node)
		{
			this.Name = node.Attributes["name"].Value;
			this.Parts = new List<SoapMessagePart>();

			if (node.HasChildNodes)
			{
				foreach (XmlNode part in node.ChildNodes)
                {
					this.Parts.Add(new SoapMessagePart(part));

				}

			}
		}

		public string Name { get; set; }

		public List<SoapMessagePart> Parts { get; set; }
	}

	public class SoapBindingOperation
	{
		public SoapBindingOperation(XmlNode op)
		{
			this.Name = op.Attributes["name"].Value;

			foreach (XmlNode node in op.ChildNodes)
			{
				if (node.Name == "http:operation")
					this.Location = node.Attributes["location"].Value;
				if (node.Name == "soap:operation" || node.Name == "soap12:operation")
					this.SoapAction = node.Attributes["soapAction"].Value;
			}
		}

		public string Name { get; set; }
		public string Location { get; set; }
		public string SoapAction { get; set; }
	}

	public class SoapBinding
	{
		public SoapBinding(XmlNode node)
		{
			this.Name = node.Attributes["name"].Value;
			this.Type = node.Attributes["type"].Value;
			this.IsHTTP = false;
			this.Operations = new List<SoapBindingOperation>();
			foreach (XmlNode op in node.ChildNodes)
			{
				if (op.Name.EndsWith("operation"))
				{
					this.Operations.Add(new SoapBindingOperation(op));
				}
				else if (op.Name == "http:binding")
				{
					this.Verb = op.Attributes["verb"].Value;
					this.IsHTTP = true;
				}
			}
		}

		public string Name { get; set; }
		public List<SoapBindingOperation> Operations { get; set; }
		public bool IsHTTP { get; set; }
		public string Verb { get; set; }
		public string Type { get; set; }
	}

}