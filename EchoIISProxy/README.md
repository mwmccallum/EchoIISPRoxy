# Demonstration of CoreWCF with IISProxy
This project is intented to demonstrate how to run CoreWCF under three conditions:
  1.  Kestrel only for use on Linux or Windows without IIS
  2.  IIS running OutOfProcess
  3.  IIS running InProcess

This example uses options in appsettings.json [APIOptions] to set the run conditions of the project for the above three conditions. It is noted at the start of each run time what the values for the options need to be. 

This project will also demonstrate running multiple Services in same Project. The EchoService was cloned and changed to NextService.

These will be the URLs that access to the two services in all situations:

First Service EchoService:
URL to Service Page: https://localhost:44335/AppPath/EchoService
URL to WSDL: https://localhost:44335/AppPath/EchoService?singleWSDL

Second Service NextService:
URL to Service Page: https://localhost:44335/AppPath/NextService
URL to WSDL: https://localhost:44335/AppPath/NextService?singleWSDL

## Kestrel Only
appsettings.json settings:
 ```
 {
  isIISProxyUsed = false
  isIISInProcessUsed = false
}
```

Run Project with: EchoIISProxy

Console window will show both HTTP(s) listeners loaded ports (44335 and 59371)

## IIS running OutOfProcess
appsettings.json settings:
```
{
  isIISProxyUsed = true,
  isIISInProcessUsed = false
}
```

Also update web.config to show OutOfProcess for hosting model.

Run Project with: IIS Express OutOfProcess

From Output window, you will see the out of process Kestrel server loaded. Now listening on http://127.0.0.1:PortNo will be shown with the random port number.

Example:
```
info: CoreWCF.Channels.ServiceModelHttpMiddleware[0]
      Mapping CoreWCF branch app for path /AppPath/EchoService
info: CoreWCF.Channels.ServiceModelHttpMiddleware[0]
      Mapping CoreWCF branch app for path /AppPath/EchoService
info: CoreWCF.Channels.ServiceModelHttpMiddleware[0]
      Mapping CoreWCF branch app for path /AppPath/NextService
info: CoreWCF.Channels.ServiceModelHttpMiddleware[0]
      Mapping CoreWCF branch app for path /AppPath/NextService
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://127.0.0.1:3484
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

The Kestrel session is limited to access from the IIS Proxy server. If you try to connect to it using the 127.0.0.1 address, an error message will be displayed similar to this: "'MS-ASPNETCORE-TOKEN' does not match the expected pairing token '485b9c0d-97c1-468a-8035-d36cf006a370', request rejected."

## IIS running InProcess
appsettings.json settings:
```
{
  isIISProxyUsed = true
  isIISInProcessUsed = true
}
```

Also update web.config to show InProcess for hosting model.

Run Project with: IIS Express InProcess

If you forget to set isIISInProcessUsed to true, it will generate and error similar to this:

```
System.InvalidOperationException: Application is running inside IIS process but is not configured to use IIS server.
   at Microsoft.AspNetCore.Server.IIS.Core.IISServerSetupFilter.<>c__DisplayClass2_0.<Configure>b__0(IApplicationBuilder app)
   at Microsoft.AspNetCore.HostFilteringStartupFilter.<>c__DisplayClass0_0.<Configure>b__0(IApplicationBuilder app)
   at Microsoft.AspNetCore.Hosting.GenericWebHostService.StartAsync(CancellationToken cancellationToken)
Exception thrown: 'System.InvalidOperationException' in System.Private.CoreLib.dll
```
