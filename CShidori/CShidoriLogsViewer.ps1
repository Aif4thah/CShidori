function Get-CShidoriLogs
{
    <#
    .SYNOPSIS
        Parse CShidori logs

    .DESCRIPTION
        version: 1.0
        https://github.com/Aif4thah/CShidori
        Tested on powerhsell 7.2

    .PARAMETER File
        CShidori csv log file

    .PARAMETER All
        return all logs

    .PARAMETER ResponseMatch
        search string in response

    .PARAMETER $ExcludeSize
        exclude all response with this size

    .OUTPUTS
       the results are saved in the Cshidori Log directory

    .EXAMPLE
        Start-Fuzzing

    #>

    param(
        [Parameter(Mandatory=$true,Position=0)]
        [ValidateNotNullOrEmpty()]
        [string] $File,

        [Parameter(Position=1)]
        [ValidateNotNullOrEmpty()]
        [switch] $All=$false,

        [Parameter(Position=2)]
        [ValidateNotNullOrEmpty()]
        [string] $ResponseMatch,

        [Parameter(Position=3)]
        [ValidateNotNullOrEmpty()]
        [int] $ExcludeSize=-1

    )


    begin
    {
        if(test-path -Path $File){
            $csv = Import-Csv -Path $File -Delimiter ',' -Verbose
        }
        else
        {
            write-host "[!] File not found" -ForegroundColor Red
            exit
        }

        $result = @()
        
    }
    Process
    {
        if($All){
            return $csv
        
        }
        elseif(($ResponseMatch.Length -ne 0) -and ($ExcludeSize -ne -1) ){
            $csv|% {
                if(($_.response -match $ResponseMatch) -and ([int]$_.responseLenght -ne $ExcludeSize ) ){
                    Write-Debug ("{0} match {1} AND {2} notEqual {3}" -f $_.response,$ResponseMatch, $_.responseLenght,$ExcludeSize)
                    $result += $_
                }
            }
            return $Result
        }

        elseif($ResponseMatch.Length -ne 0){   
            $csv|% {
                if($_.response -match $ResponseMatch){
                    Write-Debug ("{0} match {1}" -f $_.response,$ResponseMatch)
                    $result += $_
                }
            }
            return $Result
        }

        elseif($ExcludeSize -ne $null){
            $csv|% {
                if([int]$_.responseLenght -ne $ExcludeSize ){
                    Write-Debug ("{0} notEqual {1}" -f $_.responseLenght,$ExcludeSize)
                    $result += $_
                }
            }
            return $Result
        }


    }
    end
    {
  
    }
}

