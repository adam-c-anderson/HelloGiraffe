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
    This configures build support for Windows, Linux, and OSX