using System.Runtime.InteropServices;
using System.Security.Authentication;
using EchoIISProxy.Classes;
using EchoIISProxy.Interfaces;
using EchoIISProxy.Services;

try
{
  var builder = WebApplication.CreateBuilder ();

  var config = new ConfigurationBuilder ()
      .SetBasePath ( Directory.GetCurrentDirectory () )
      .AddJsonFile ( "appsettings.json", optional: false, reloadOnChange: true )
      .AddCommandLine ( args )
      .Build ();

  // Options
  var apiSection = builder.Configuration.GetSection ( nameof ( APIOptions ) );
  APIOptions apiOptions = apiSection.Get<APIOptions> ();
  builder.Services.Configure<APIOptions> ( apiSection );
  builder.Services.AddSingleton ( apiOptions );

  #region WebHost Settings based on OS and Option
  // AllowSynchronousIO required for CoreWCF

  if ( RuntimeInformation.IsOSPlatform ( OSPlatform.Windows )
    && apiOptions.isIISProxyUsed )
  {
    if ( apiOptions.isIISInProcessUsed )
    {
      builder.WebHost
        .UseConfiguration ( config )
        .UseIIS ();
    }
    else
    {
      builder.WebHost
        .UseConfiguration ( config )
        .UseIISIntegration ();
    }
    builder.Services.Configure<IISServerOptions> ( options =>
    {
      options.AllowSynchronousIO = true;
    } );
  }
  else
  {
    Console.WriteLine ( "EchoIISProxy is using Kestrel without Proxy." );
    builder.WebHost
      .UseConfiguration ( config )
      .ConfigureKestrel ( ( context, options ) =>
      {
        options.AllowSynchronousIO = true;
        options.ConfigureHttpsDefaults ( httpsOptions =>
        {
          httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
        } );
      } )
      .UseKestrel ();
  }
  #endregion

  #region Add Configuration file and WSDL support
  builder.Services.AddServiceModelServices ()
    .AddServiceModelMetadata ()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior> ();

  #endregion


  // Add Each WCF Service for DI into CoreWCF
  // each new service must be added to this extension method
  builder.Services.AddEchoIISProxyServices ();

  var app = builder.Build ();

  #region Add WCF Services

  app.UseServiceModel ( builder =>
  {
    // Add each Service Here:
    #region EchoService

    builder.AddService<EchoService> ( ( serviceOptions ) =>
    {
      // CoreWCF doesn't recognize host entry for base address in configuration file. It 
      // must be set in code. app.settings will hold the base URI to use     
      if ( !string.IsNullOrEmpty ( apiOptions.EchoServiceURI ) )
      {
        serviceOptions.BaseAddresses.Add ( new Uri ( apiOptions.EchoServiceURI ) );
      }
      if ( !string.IsNullOrEmpty ( apiOptions.EchoServiceURISSL ) )
      {
        serviceOptions.BaseAddresses.Add ( new Uri ( apiOptions.EchoServiceURISSL ) );
      }
    } );
    // Sets URLS: http://localhost/AppPath/EchoService; https://localhost:44334/AppPath/EchoService;
    builder.AddServiceEndpoint<EchoService, IEchoService> ( new BasicHttpBinding (), "" );
    builder.AddServiceEndpoint<EchoService, IEchoService> ( new BasicHttpBinding ( BasicHttpSecurityMode.Transport ), "" );

    #endregion

    #region NextService   

    builder.AddService<NextService> ( ( serviceOptions ) =>
    {
      // CoreWCF doesn't recognize host entry for base address in configuration file. It 
      // must be set in code. app.settings will hold the base URI to use     
      if ( !string.IsNullOrEmpty ( apiOptions.NextServiceURI ) )
      {
        serviceOptions.BaseAddresses.Add ( new Uri ( apiOptions.NextServiceURI ) );
      }
      if ( !string.IsNullOrEmpty ( apiOptions.NextServiceURISSL ) )
      {
        serviceOptions.BaseAddresses.Add ( new Uri ( apiOptions.NextServiceURISSL ) );
      }
    } );
    // Sets URLS: http://localhost/AppPath/NextService; https://localhost:44334/AppPath/NextService;
    builder.AddServiceEndpoint<NextService, INextService> ( new BasicHttpBinding (), "" );
    builder.AddServiceEndpoint<NextService, INextService> ( new BasicHttpBinding ( BasicHttpSecurityMode.Transport ), "" );

    #endregion

  } );

  #region Configure WSDL to be available over http & https
  var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior> ();
  // Set Meta Get based on Configured URL
  if ( !string.IsNullOrEmpty ( apiOptions.EchoServiceURI ) || !string.IsNullOrEmpty ( apiOptions.NextServiceURI ) )
  {
    serviceMetadataBehavior.HttpGetEnabled = true;
  }
  if ( !string.IsNullOrEmpty ( apiOptions.EchoServiceURISSL ) || !string.IsNullOrEmpty ( apiOptions.NextServiceURISSL ) )
  {
    serviceMetadataBehavior.HttpsGetEnabled = true;
  }
  #endregion

  #endregion

  app.Run ();
}
catch ( Exception ex )
{
  Console.WriteLine ( $"ExchIISProxy failed to start. Reason: {ex.Message}" );
}
