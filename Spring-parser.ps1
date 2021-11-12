function Get-SpringArtefacts{
    <#
    .SYNOPSIS
        Basic Srping code grabber

    .DESCRIPTION
        collect wsdl, xsd, mapper and parameters

    .PARAMETER source
        source folder

    .PARAMETER output
        output folder

    .OUTPUTS
        Wordlists

    .EXAMPLE
        Get-SpringArtefact -source .\Spring-project -output .

    #>
    param(
        [Parameter(Mandatory=$true, Position=0)]
        [ValidateNotNullOrEmpty()]
        [string] $source,

        [Parameter(Mandatory=$true, Position=1)]
        [ValidateNotNullOrEmpty()]
        [string] $output   
    )

    begin{

        $endpointsPath = $output + "\endpoints.txt"
        $paramPath = $output + "\parametersNames.txt"
        $CshidoriWl = $output + "\fuzz.txt"
        $CshidoriPath = "..\CShidori\CShidori.exe"
        $endpoints = $param = @()
        $parentheseEndpoints = $parentheseParam = $false

        write-host "[*] geting files..." -ForegroundColor Yellow
        $files = get-ChildItem -recurse -file $source
                      
    }Process{

        if( -not (test-path $output)){ mkdir $output }

        write-host "[*] WSDL files ?" -ForegroundColor Yellow
        $files |% { 
            if($_ -match 'wsdl$'){ 
            write-host ("[!] WSDL: {0}" -f $_.FullName)
            $hash = (Get-FileHash -Algorithm SHA256 $_.FullName).Hash
            copy-item -Path $_.FullName -Destination "${output}\${hash}-$_"
            }
        }

        write-host "[*] DTD files ?" -ForegroundColor Yellow
        $files |% { 
            if($_ -match 'dtd$'){ 
            write-host ("[!] DTD: {0}" -f $_.FullName)
            $hash = (Get-FileHash -Algorithm SHA256 $_.FullName).Hash
            copy-item -Path $_.FullName -Destination "${output}\${hash}-$_"
            }
        }

        write-host "[*] XSD files ?" -ForegroundColor Yellow
        $files |% { 
            if($_ -match 'xsd$'){ 
            write-host ("[!] XSD: {0}" -f $_.FullName)
            $hash = (Get-FileHash -Algorithm SHA256 $_.FullName).Hash
            copy-item -Path $_.FullName -Destination "${output}\${hash}-$_"
            }
        }

        write-host "[*] API Mappers ?" -ForegroundColor Yellow

        $files |ForEach-Object {
            $file = $_.pspath
            if($file -match '(\.java$)'){
                $content = Get-Content -path "$file"
                $content |ForEach-Object {    
                    $line = $_
                 
                    if( ($line -match '@\w{3,8}mapp\w*\(.*\)' -or $line -match 'getAsStream\(' ) -and $line -match '"(.*)"'  ){ 
                        #($line -match "@RequestMapping" -or $line -match "@GetMapping" -or $line -match '@PostMapping' -or $line -match '@PutMapping' -or $line -match '@DeleteMapping') -and $line -match '"(.*)"')
                        $endpoints += parse-code -value ($Matches[0].split('"'))                  
                    }

                    elseif(($line -match '@RequestParam' -or $line -match '@PathVariables') -and $line -match '"(.*)"'  ){ 
                        $param += parse-code -value ($Matches[0].split('"'))                       
                    }
                    elseif($line -match '@Param '){ 
                        $param +=  $line.split(" ")[-1].trim("'`". ")                      
                    }

                    <# case:
                    @RequestMapping(
                        value = "/path",
                        method = RequestMethod.DELETE,
                        produces = MediaType.APPLICATION_JSON_VALUE)                    
                    #>
                    if( $line -match '@\w{3,8}mapp\w*\(' -and $line -notmatch '\)' ){
                        $parentheseEndpoints = $true
                    }
                    elseif( ( $line -match '@RequestParam' -or $line -match '@PathVariables' ) -and $line -notmatch '\)'){
                        $parentheseParam = $true
                    }
                    elseif( $line -match '\)' ){
                        $parentheseEndpoints = $parentheseParam = $false
                    }
                    if($parentheseEndpoints){
                        if($line -match '"(.*)"'){
                            $endpoints += parse-code -value ($Matches[0].split('"'))  
                        }
                    } 
                    if($parentheseParam){
                        if($line -match '"(.*)"'){
                            $param += parse-code -value ($Matches[0].split('"'))    
                        }
                    }              
                        
                }

            }

        }

        write-host "[*] Sorting results..." -ForegroundColor Yellow
        $endpoints = $endpoints |Select-Object -Unique 
        $param = $param |Select-Object -Unique 

        write-host "[*] Generate Wordlists..." -ForegroundColor Yellow
        $endpoints  >> $endpointsPath
        $param >> $paramPath
    
    }end{

        write-host "[*] CShidori [https://github.com/Aif4thah/CShidori]" -ForegroundColor Yellow

        $badchars = Invoke-expression("$CshidoriPath bc")

        $badchars >> $paramPath

        get-content $endpointsPath |ForEach-Object {
            $line = $_
            if($line -match "{\w*}"){
                $badchars|ForEach-Object{
                    $line -replace "{\w*}", $_ >> $CshidoriWl
                }               
            }
        }

        write-host "[*] Sorting wordlists..." -ForegroundColor Yellow

        $tmp = Get-Content $CshidoriWl |Select-Object -Unique ; $tmp > $CshidoriWl
        $tmp = Get-Content $endpointsPath |Select-Object -Unique ; $tmp > $endpointsPath
        $tmp = Get-Content $paramPath |Select-Object -Unique ; $tmp > $paramPath
        write-host "[*] end." -ForegroundColor Yellow
    }
}

function remove-slash($in){
    if($in[0] -eq '/'){ $in = $in.Substring(1)}
    return $in   
}

function parse-code($values){
 
    $dico = @()
    for($i=0; $i -lt $values.length; $i++){
        if(($i % 2) -eq 1){ 
            $dico += remove-slash($values[$i])
        }
    } 
    return $dico
}