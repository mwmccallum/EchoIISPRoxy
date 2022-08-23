using EchoIISProxy.Interfaces;

namespace EchoIISProxy.Services
{
  public class NextService : INextService
  {
    public string Next ( string text )
    {
      Console.WriteLine ( $"Received {text} from client!" );
      return text;
    }

    public string ComplexNext ( NextMessage text )
    {
      Console.WriteLine ( $"Received {text.Text} from client!" );
      return text.Text;
    }

    public string FailNext ( string text )
        => throw new FaultException<NextFault> ( new NextFault () { Text = "WCF Fault OK" }, new FaultReason ( "FailReason" ) );

    [AuthorizeRole ( "CoreWCFGroupAdmin" )]
    public string NextForPermission ( string echo )
    {
      return echo;
    }
  }
}
