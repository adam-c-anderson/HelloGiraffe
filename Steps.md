# Steps taken to produce project

1. Install `giraffe-template`
    ```
    dotnet new -i "giraffe-template::*"
    ```
1. Scaffold project
    ```
    dotnet new giraffe --ViewEngine none --IncludeTests --UsePaket --language F#
    ```
    Need to specify `-language F#` until SDK >= 2.1.300 due to a bug
1. (If using VS Code) Configure `tasks.json`
    
    Replace `"command"` with the following JSON
    ```json
    "command": "./build.sh",
    "windows": {
        "command": ".\\build.bat"
    },
    ```
    
    This configures build support for Windows, Linux, and OSX.

    At this point, you can build with `Ctrl+Shift+B` and run default project to run the Giraffe sample API app
1. Install and Configure FAKE 5
    1. Install Chocolatey
        ```    
        @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"
        ```
    1. Install FAKE
        ```
        choco install fake -pre
        ```
        
        There are bootstrapping options as well if preinstalling globally is not an option. See https://fake.build/fake-gettingstarted.html#Install-FAKE.
    1. Create `build.fsx`
    1. Modify build task in `tasks.json` to run build script
        ```
        fake run build.fsx
        ```
    1. Initial implementation cleans, builds, runs tests, then publishes -- all in debug configuration
