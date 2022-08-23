using EchoIISProxy.Services;

namespace EchoIISProxy.Interfaces;

public static class IServiceCollectionExtension
{
  /// <summary>
  /// Injects the Each WCF Service into the Service Collection
  /// </summary>
  /// <param name="services">Service Collection</param>
  /// <returns></returns>
  public static IServiceCollection AddEchoIISProxyServices ( this IServiceCollection services )
  {
    services.AddSingleton<EchoService> ();
    services.AddSingleton<NextService> ();
    return services;
  }
}
