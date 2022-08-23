namespace EchoIISProxy.Interfaces;

public interface IAPIOptions
{
  #region Application Options

  /// <summary>
  /// Echo Service URI for the Non-SSL API Interface
  /// </summary>
  public string EchoServiceURI { get; set; }
  /// <summary>
  /// Echo Service URI for the SSL API Interface
  /// </summary>
  public string EchoServiceURISSL { get; set; }
  /// <summary>
  /// Next Service URI for the Non-SSL API Interface
  /// </summary>
  public string NextServiceURI { get; set; }
  /// <summary>
  /// Next Service URI for the SSL API Interface
  /// </summary>
  public string NextServiceURISSL { get; set; }
  /// <summary>
  /// Whether this is using IIS as a Proxy or not
  /// </summary>
  public bool isIISProxyUsed { get; set; }
  /// <summary>
  /// Whether the IISProxy is In Process, vs 
  /// Out of Process.
  /// </summary>
  public bool isIISInProcessUsed { get; set; }
  #endregion

}
