function Start-Fuzzing
{
    <#
    .SYNOPSIS
        This function shows how to use multithreading and fuzz multiple endpoints with CShidori

    .DESCRIPTION
        version: 1.2
        https://github.com/Aif4thah/CShidori
        Tested on powerhsell 7.2

    .PARAMETER CShidoriPath
        Path of folder containing CShidori.exe, default value is "."

    .OUTPUTS
       the results are saved in the Cshidori Log directory

    .EXAMPLE
        Start-Fuzzing

    #>

    param(
        [Parameter(Position=0)]
        [ValidateNotNullOrEmpty()]
        [string] $CShidoriPath = '.',

        [Parameter(Position=1)]
        [ValidateNotNullOrEmpty()]
        [int] $MaxThreads = 2

    )


    begin
    {
        $CShidoriPath = "{0}/CShidori.exe" -f $CShidoriPath
        if(-Not( test-path -Path "$CShidoriPath")){
            Write-Host "CShidori.exe not found" -ForegroundColor Red
            exit
        }
        
        <#--- Here is your targets and CShidori options, respectively -m, -o, -i, -p, -d ---#>
        $FuzzingList = @(

            @( 'tls', '..\testing\ZAP-Post-req.raw',   '127.0.0.1',   '443', 'chars'    ),
            @( 'tcp', '..\testing\burp.req',   '127.0.0.1',   '80', 'chars'     ),
            @( 'tls', '..\testing\Short.txt',   '127.0.0.1',   '443', 'chars'   )

        )

    }
    Process
    {
        $FuzzingList|%{ 
            $c=$_

            $sb = {
                param($p0, $p1,$p2,$p3,$p4, $p5)
                "$p0 -m $p1 -o $p2 -i $p3 -p $p4 -d $p5"|iex
            }
            
            Start-ThreadJob -ScriptBlock $sb `
            -ArgumentList $CShidoriPath, $c[0], $c[1], $c[2], $c[3], $c[4]   `
            -ThrottleLimit $MaxThreads -Debug
        }
    }
    end
    {
        return 0    
    }
}

