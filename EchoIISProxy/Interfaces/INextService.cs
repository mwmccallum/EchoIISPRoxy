using CoreWCF;
using System.Runtime.Serialization;

namespace EchoIISProxy.Interfaces
{
  [DataContract]
  public class NextFault
  {
    private string text;

    [DataMember]
    public string Text
    {
      get => text;
      set => text = value;
    }
  }

  [ServiceContract]
  public interface INextService
  {
    [OperationContract]
    string Next ( string text );

    [OperationContract]
    string ComplexNext ( NextMessage text );

    [OperationContract]
    [FaultContract ( typeof ( NextFault ) )]
    string FailNext ( string text );

    [OperationContract]
    string NextForPermission ( string text );
  }

  [DataContract]
  public class NextMessage
  {
    [DataMember]
    public string Text { get; set; }
  }
}
