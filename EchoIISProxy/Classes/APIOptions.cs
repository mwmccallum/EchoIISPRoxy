using EchoIISProxy.Interfaces;

namespace EchoIISProxy.Classes;

public class APIOptions : IAPIOptions
{
  #region Application Options
  /// <summary>
  /// Echo Service URI for the Non-SSL API Interface
  /// </summary>
  public string EchoServiceURI { get; set; } = null;
  /// <summary>
  /// Echo Service URI for the SSL API Interface
  /// </summary>
  public string EchoServiceURISSL { get; set; } = null;
  /// <summary>
  /// Next Service URI for the Non-SSL API Interface
  /// </summary>
  public string NextServiceURI { get; set; } = null;
  /// <summary>
  /// Next Service URI for the SSL API Interface
  /// </summary>
  public string NextServiceURISSL { get; set; } = null;
  /// <summary>
  /// Whether this is using IIS as a Proxy or not
  /// </summary>
  public bool isIISProxyUsed { get; set; } = false;
  /// <summary>
  /// Whether the IISProxy is In Process, vs 
  /// Out of Process.
  /// </summary>
  public bool isIISInProcessUsed { get; set; } = false;

  #endregion

}
