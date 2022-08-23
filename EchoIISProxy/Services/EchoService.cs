using EchoIISProxy.Interfaces;

namespace EchoIISProxy.Services
{
  public class EchoService : IEchoService
  {
    public string Echo ( string text )
    {
      Console.WriteLine ( $"Received {text} from client!" );
      return text;
    }

    public string ComplexEcho ( EchoMessage text )
    {
      Console.WriteLine ( $"Received {text.Text} from client!" );
      return text.Text;
    }

    public string FailEcho ( string text )
        => throw new FaultException<EchoFault> ( new EchoFault () { Text = "WCF Fault OK" }, new FaultReason ( "FailReason" ) );

    [AuthorizeRole ( "CoreWCFGroupAdmin" )]
    public string EchoForPermission ( string echo )
    {
      return echo;
    }
  }
}
