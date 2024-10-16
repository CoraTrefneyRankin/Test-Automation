This project uses Playwright. The first time you use it, build and then run the command below using PowerShell. Replace <path to the project> with the path to your project. 
(If it fails, install PowerShell 7 and then run the command.)

     pwsh <path to the project>\\PlaywrightAutomation\bin\Debug\net7.0-windows\playwright.ps1 install

To run Playwright tests, run these two commands from PowerShell.
    
    cd <path to the project>
	dotnet test --filter TestCategory=testsuite