# BlazorKit Application Template

This is a Visual Studio Project Template designed for Oqtane development projects. This template relies on the native templating capabilities of the .NET Command Line Interface (CLI):

```
dotnet new install BlazorKit.Application.Template
dotnet new blazorkit-app -o MyCompany.MyProject
cd MyCompany.MyProject
dotnet build
cd Server
dotnet run
browse to Url
```

The solution contains Client, Server, and Shared folders which is where you you would implement your custom functionality. An example module and theme are included for reference, and you can add additional modules and themes within the same projects by following the standard Oqtane folder/namespace conventions. 

