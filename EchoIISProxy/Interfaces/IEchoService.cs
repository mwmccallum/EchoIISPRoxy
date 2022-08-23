﻿namespace EchoIISProxy.Interfaces
{
  [DataContract]
  public class EchoFault
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
  public interface IEchoService
  {
    [OperationContract]
    string Echo ( string text );

    [OperationContract]
    string ComplexEcho ( EchoMessage text );

    [OperationContract]
    [FaultContract ( typeof ( EchoFault ) )]
    string FailEcho ( string text );

    [OperationContract]
    string EchoForPermission ( string text );
  }

  [DataContract]
  public class EchoMessage
  {
    [DataMember]
    public string Text { get; set; }
  }
}
