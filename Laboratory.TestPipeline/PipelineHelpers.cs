using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.XPath;
using Microsoft.BizTalk.Bam.EventObservation;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.Test.BizTalk.PipelineObjects;
using Microsoft.XLANGs.BaseTypes;
using DocumentSpec = Microsoft.BizTalk.Component.Interop.DocumentSpec;
using PStage = Microsoft.Test.BizTalk.PipelineObjects.Stage;
using PipelineHelper = Microsoft.Test.BizTalk.PipelineObjects.PipelineFactory;
using ReceivePipeline = Microsoft.BizTalk.PipelineOM.ReceivePipeline;
using SendPipeline = Microsoft.BizTalk.PipelineOM.SendPipeline;


namespace BTSG.Pipeline.Helpers
{
  public class MsgColl : Collection<IBaseMessage>
  {

    public void AddRange(IEnumerable<IBaseMessage> list)
    {
      foreach (IBaseMessage msg in list)
        Add(msg);
    }
  } 


  public abstract class WPipelineBase : IEnumerable<IBaseComponent>
  {
    private readonly bool _isReceivePipeline;

    internal IPipeline Gppipeline
    {
      get { return _pipeline; }
    }





    private readonly IPipeline _pipeline;
    private readonly IPipelineContext _pipelineContext;

  


    protected WPipelineBase(IPipeline pipeline, bool isReceivePipeline)
    {
      if (pipeline == null)
        throw new ArgumentNullException("specifiy a correct pipeline object");
      _pipeline = pipeline;
      _pipelineContext = ppContext();
      _isReceivePipeline = isReceivePipeline;
    }

    public string Gbcertificate
    {
      get { return _pipelineContext.GetGroupSigningCertificate(); }
      set
      {
        var ctxt = (IppContext)_pipelineContext;
        ctxt.SetGroupSigningCertificate(value);
      }
    }
    public void Addppomponent(IBaseComponent _component, ppStage _stage)
    {
      if (_component == null)
        throw new ArgumentNullException("No component present");
      if (_stage == null)
        throw new ArgumentNullException(" No stage present");

      if (_stage.IsReceiveStage != _isReceivePipeline)
        throw new ArgumentException("Invalid Stage", "stage");

      PStage theStage = checkStage(_stage);
      theStage.AddComponent(_component);
    }


    //Add schema from assembly
    public void addppDocSpec(string typeName, string assemblyName)
    {
      if (String.IsNullOrEmpty(typeName))
        throw new ArgumentNullException("typeName");
      if (String.IsNullOrEmpty(assemblyName))
        throw new ArgumentNullException("assemblyName");
   
      IDocumentSpec spec = getSchemaSpec(typeName, assemblyName);
      docSpecinMsgContxt(spec);
    }

    public IDocumentSpec getSchemaSpec(Type schemaType)
    {
      if (schemaType == null)
        throw new ArgumentNullException("schemaType");
      if (!schemaType.IsSubclassOf(typeof(SchemaBase)))
        throw new ArgumentException("Type does not represent a schema", "schemaType");

      string typename = schemaType.FullName;
      string assemblyName = schemaType.Assembly.FullName;
      var docSpec = new DocumentSpec(typename, assemblyName);

      return docSpec;
    }

    public IDocumentSpec getSchemaSpec(string typeName, string assemblyName)
    {
      if (String.IsNullOrEmpty(typeName))
        throw new ArgumentNullException("typeName");
      if (String.IsNullOrEmpty(assemblyName))
        throw new ArgumentNullException("assemblyName");

      return new DocumentSpec(typeName, assemblyName);
    }
    internal IPipelineContext ppcontext
    {
      get { return _pipelineContext; }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<IBaseComponent>)this).GetEnumerator();
    }


    //Add schema
    public void addppDocSpec(Type schemaType)
    {
      if (schemaType == null)
        throw new ArgumentNullException("schemaType");

      Type[] roots = getSchRoots(schemaType);
      foreach (Type root in roots)
      {
        IDocumentSpec docSpec = getSchemaSpec(root);
        docSpecinMsgContxt(docSpec);
      }
    }
    protected IPipelineContext ppContext()
    {
      return new PipelineContext();
    }

    public IBaseComponent retppomponent(ppStage stage, int index)
    {
      foreach (PStage st in _pipeline.Stages)
      {
        if (st.Id == stage.ID)
        {
          IEnumerator enumerator = st.GetComponentEnumerator();
          while (enumerator.MoveNext())
          {
            if (index-- == 0)
            {
              return (IBaseComponent)enumerator.Current;
            }
          }
        }
      }
      return null;
    }



    protected PStage checkStage(ppStage stage)
    {
      PStage theStage = null;
      foreach (PStage pstage in _pipeline.Stages)
      {
        if (pstage.Id == stage.ID)
        {
          theStage = pstage;
          break;
        }
      }
      if (theStage == null)
      {
        theStage = new PStage(stage.Name, stage.ExecuteMethod, stage.ID, _pipeline);
        _pipeline.Stages.Add(theStage);
      }
      return theStage;
    }

    IEnumerator<IBaseComponent> IEnumerable<IBaseComponent>.GetEnumerator()
    {
      foreach (PStage stage in _pipeline.Stages)
      {
        IEnumerator enumerator = stage.GetComponentEnumerator();
        while (enumerator.MoveNext())
        {
          yield return (IBaseComponent)enumerator.Current;
        }
      }
    }


    private string getSchRoot(Type schemaType)
    {
      var attrs = (SchemaAttribute[])
        schemaType.GetCustomAttributes(typeof (SchemaAttribute), true);
      if (attrs.Length > 0)
      {
        if (String.IsNullOrEmpty(attrs[0].TargetNamespace))
          return attrs[0].RootElement;
        return string.Format("{0}#{1}", attrs[0].TargetNamespace, attrs[0].RootElement);
      }
      return null;
    }

    private Type[] getSchRoots(Type schemaType)
    {
      string root = getSchRoot(schemaType);
      if (root != null)
      {
        return new[] { schemaType };
      }
      Type[] rts = schemaType.GetNestedTypes();
      return rts;
    }


    private void docSpecinMsgContxt(IDocumentSpec docSpec)
    {
      var ctxt = (IppContext) ppcontext;
      ctxt.AddDocSpecByType(docSpec.DocType, docSpec);
      // Pipelines referencing local schemas in the same
      // assembly don't have use the assembly qualified name
      // of the schema when trying to find it.
      ctxt.AddDocSpecByName(docSpec.DocSpecStrongName, docSpec);
      ctxt.AddDocSpecByName(docSpec.DocSpecName, docSpec);
    }




  }



  //Override BAM eventstream
  internal class ppStream : EventStream
  {
    public override void AddReference(string activityName, string activityID, string referenceType, string referenceName,
      string referenceData)
    {
    }

    public override void AddReference(string activityName, string activityID, string referenceType, string referenceName,
      string referenceData, string longreferenceData)
    {
    }

    public override void AddRelatedActivity(string activityName, string activityID, string relatedActivityName,
      string relatedTraceID)
    {
    }

    public override void BeginActivity(string activityName, string activityInstance)
    {
    }

    public override void Clear()
    {
    }

    public override void EnableContinuation(string activityName, string activityInstance, string continuationToken)
    {
    }

    public override void EndActivity(string activityName, string activityInstance)
    {
    }

    public override void Flush()
    {
    }

    public override void Flush(SqlConnection connection)
    {
    }

    public override void StoreCustomEvent(IPersistQueryable singleEvent)
    {
    }

    public override void UpdateActivity(string activityName, string activityInstance, params object[] data)
    {
    }
  }

  public static class msgHelper
  {
    private static readonly IBaseMessageFactory msgfactory = new MessageFactory();

    //Return message from string
    public static IBaseMessage createMessageFromString(string body)
    {
      if (body == null)
        throw new ArgumentNullException("Error creating body message");

      byte[] content = Encoding.Unicode.GetBytes(body);
      Stream stream = new MemoryStream(content);

      IBaseMessage msg = createMessageFromStream(stream);
      msg.BodyPart.Charset = "UTF-16";
      return msg;
    }

    //Return message from stream
    public static IBaseMessage createMessageFromStream(Stream body)
    {
      if (body == null)
        throw new ArgumentNullException("body");

      IBaseMessage message = msgfactory.CreateMessage();
      message.Context = msgfactory.CreateMessageContext();

      IBaseMessagePart bodyPart = getMsgFromStream(body);

      message.AddPart("body", bodyPart, true);

      return message;
    }

    public static void executeStream(IBaseMessagePart part)
    {
      if (part == null)
        throw new ArgumentNullException("part");
      executeStream(part.Data);
    }

 
    public static IBaseMessage createMsg(params String[] parts)
    {
      if (parts == null || parts.Length < 1)
        throw new ArgumentException("Need to specify at least one part", "parts");

      IBaseMessage message = createMessageFromString(parts[0]);
      for (int i = 1; i < parts.Length; i++)
        message.AddPart("part" + i, getMsgFromString(parts[i]), false);
      return message;
    }


 

    public static void executeStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      var buffer = new byte[4096];
      int read = 0;
      while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        ;
    }




 
    public static string readString(Stream stream, Encoding encoding)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      if (encoding == null)
        throw new ArgumentNullException("encoding");
      using (var reader = new StreamReader(stream, encoding))
        return reader.ReadToEnd();
    }


    public static string getString(IBaseMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");
      return getString(message.BodyPart);
    }

    public static IBaseMessagePart getMsgFromString(string body)
    {
      if (body == null)
        throw new ArgumentNullException("body");

      byte[] content = Encoding.Unicode.GetBytes(body);
      Stream stream = new MemoryStream(content);

      IBaseMessagePart part = getMsgFromStream(stream);
      part.Charset = "UTF-16";
      return part;
    }
    public static String getString(IBaseMessagePart part)
    {
      if (part == null)
        throw new ArgumentNullException("part");
      Encoding enc = Encoding.UTF8;
      if (!String.IsNullOrEmpty(part.Charset))
        enc = Encoding.GetEncoding(part.Charset);
      return readString(part.Data, enc);
    }

    public static void executeStream(IBaseMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");
      executeStream(message.BodyPart);
    }


    private static void getMsgPart(IBaseMessage msg, XPathNavigator node, string contextFile)
    {
      // don't care about the id because we can't set it anyway
      string name = node.GetAttribute("Name", "");
      string filename = node.GetAttribute("FileName", "");
      string charset = node.GetAttribute("Charset", "");
      string contentType = node.GetAttribute("ContentType", "");
      bool isBody = XmlConvert.ToBoolean(node.GetAttribute("IsBodyPart", ""));

      XmlResolver resolver = new XmlUrlResolver();
      Uri realfile = resolver.ResolveUri(new Uri(contextFile), filename);
      IBaseMessagePart part = getMsgFromStream(File.OpenRead(realfile.LocalPath));
      part.Charset = charset;
      part.ContentType = contentType;
      msg.AddPart(name, part, isBody);
    }
    public static IBaseMessage createMsg(params Stream[] parts)
    {
      if (parts == null || parts.Length < 1)
        throw new ArgumentException("Need to specify at least one part", "parts");

      IBaseMessage message = createMessageFromStream(parts[0]);
      for (int i = 1; i < parts.Length; i++)
        message.AddPart("part" + i, getMsgFromStream(parts[i]), false);
      return message;
    }

    public static IBaseMessage getMsg(string filecontext)
    {
      IBaseMessage msg = msgfactory.CreateMessage();
      IBaseMessageContext ctxt = msgfactory.CreateMessageContext();
      msg.Context = ctxt;

      var doc = new XPathDocument(filecontext);
      XPathNavigator nav = doc.CreateNavigator();
      XPathNodeIterator props = nav.Select("//Property");
      foreach (XPathNavigator prop in props)
      {
        ctxt.Write(
          prop.GetAttribute("Name", ""),
          prop.GetAttribute("Namespace", ""),
          prop.GetAttribute("Value", "")
          );
      }

      XPathNodeIterator parts = nav.Select("//MessagePart");
      foreach (XPathNavigator part in parts)
      {
        getMsgPart(msg, part, filecontext);
      }
      return msg;
    }

    public static IBaseMessagePart getMsgFromStream(Stream body)
    {
      if (body == null)
        throw new ArgumentNullException("body");

      IBaseMessagePart part = msgfactory.CreateMessagePart();
      part.Data = body;
      return part;
    }

  } 

  public class trxPipeline : IDisposable
  {
    internal trxPipeline(CommittableTransaction transaction)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");

      _trxpp = transaction;
    }

    private bool _istrxdone;
    private readonly CommittableTransaction _trxpp;


    public void trxomplete()
    {
      _istrxdone = true;
    }
    public void Dispose()
    {
      if (_istrxdone)
        _trxpp.Commit();
      else
        _trxpp.Rollback();
      _trxpp.Dispose();
    }

  } 

  internal interface IppContext : IPipelineContext, IPipelineContextEx
  {
    void AddDocSpecByName(string name, IDocumentSpec documentSpec);
    void AddDocSpecByType(string type, IDocumentSpec documentSpec);
    void SetAuthenticationRequiredOnReceivePort(bool value);
    void SetGroupSigningCertificate(string certificate);
    trxPipeline EnableTransactionSupport();
  } 




  public sealed class ppStage
  {
    private readonly ExecuteMethod _executeMethod;
    private readonly Guid _id;
    private readonly bool _isReceiveStage;
    private readonly string _name;

    private static readonly IDictionary<Guid, ppStage> _stages =
      new Dictionary<Guid, ppStage>();

    public static readonly ppStage Decode = new ppStage(CategoryTypes.CATID_Decoder, "Decode", ExecuteMethod.All, true);

    public static readonly ppStage Disassemble = new ppStage(CategoryTypes.CATID_DisassemblingParser, "Disassemble",
      ExecuteMethod.FirstMatch, true);

    public static readonly ppStage Validate = new ppStage(CategoryTypes.CATID_Validate, "Validate", ExecuteMethod.All,
      true);


    public static readonly ppStage Encode = new ppStage(CategoryTypes.CATID_Encoder, "Encode", ExecuteMethod.All, false);



    public static readonly ppStage ResolveParty = new ppStage(CategoryTypes.CATID_PartyResolver, "ResolveParty",
  ExecuteMethod.All, true);

    public static readonly ppStage PreAssemble = new ppStage(CategoryTypes.CATID_Any, "Pre-Assemble", ExecuteMethod.All,
      false);

    public static readonly ppStage Assemble = new ppStage(CategoryTypes.CATID_AssemblingSerializer, "Assemble",
      ExecuteMethod.All, false);



    private ppStage(string id, string name, ExecuteMethod method, bool isReceiveStage)
    {
      _id = new Guid(id);
      _name = name;
      _executeMethod = method;
      _isReceiveStage = isReceiveStage;
      _stages.Add(_id, this);
    }

 
    public Guid ID
    {
      get { return _id; }
    }

    public string Name
    {
      get { return _name; }
    }

    public ExecuteMethod ExecuteMethod
    {
      get { return _executeMethod; }
    }

    public bool IsReceiveStage
    {
      get { return _isReceiveStage; }
    }

    internal static ppStage Lookup(Guid stageId)
    {
      return _stages[stageId];
    }
  } 


  public static class fPipeline
  {


    public static wReceivePipeline retReceivePP(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("Pipeline not present, deploy it");

      if (!type.IsSubclassOf(typeof(ReceivePipeline)))
        throw new InvalidOperationException("No Receive Pipeline type");

      var helper = new PipelineHelper();
      IPipeline pipeline = helper.CreatePipelineFromType(type);
      return new wReceivePipeline(pipeline);
    }


    public static wSendPipeline retSendPP(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("Pipeline not present, deploy it");

      if (!type.IsSubclassOf(typeof(SendPipeline)))
        throw new InvalidOperationException("No Send Pipeline type");

      var helper = new PipelineHelper();
      IPipeline pipeline = helper.CreatePipelineFromType(type);
      return new wSendPipeline(pipeline);
    }
  }

  public class wReceivePipeline : WPipelineBase
  {
    internal wReceivePipeline(IPipeline pipeline)
      : base(pipeline, true)
    {
      checkStage(ppStage.Decode);
      checkStage(ppStage.Disassemble);
      checkStage(ppStage.Validate);
      checkStage(ppStage.ResolveParty);
    }
    //put the stream
    private Stream putStream(Stream source)
    {
      var buff = new byte[64 * 1024];
      var stream = new MemoryStream();
      int btRead = 0;
      while ((btRead = source.Read(buff, 0, buff.Length)) > 0)
      {
        stream.Write(buff, 0, btRead);
      }
      stream.Position = 0;
      return stream;
    }

    //Execute pipeline
    public MsgColl ExecutePP(IBaseMessage inputMessage)
    {
      if (inputMessage == null)
        throw new ArgumentNullException("No input Message");

      Gppipeline.InputMessages.Add(inputMessage);
      var output = new MsgColl();
      Gppipeline.Execute(ppcontext);

      IBaseMessage om = null;
      while ((om = Gppipeline.GetNextOutputMessage(ppcontext)) != null)
      {
        output.Add(om);

        if (om.BodyPart != null && om.BodyPart.Data != null)
        {
          om.BodyPart.Data = putStream(om.BodyPart.Data);
        }
      }

      return output;
    }

  } 


  public class wSendPipeline : WPipelineBase
  {
    internal wSendPipeline(IPipeline pipeline)
      : base(pipeline, false)
    {
      checkStage(ppStage.PreAssemble);
      checkStage(ppStage.Assemble);
      checkStage(ppStage.Encode);
    }


    //Exetute pp
    public IBaseMessage ExecutePpeline(params IBaseMessage[] iMessages)
    {
      var inputs = new MsgColl();
      inputs.AddRange(iMessages);
      return Executepp(inputs);
    }

 
    public IBaseMessage Executepp(MsgColl inputMessages)
    {
      if (inputMessages == null)
        throw new ArgumentNullException("inputMessages");
      if (inputMessages.Count <= 0)
        throw new ArgumentException("Must provide at least one input message", "inputMessages");

      foreach (IBaseMessage inputMessage in inputMessages)
      {
        Gppipeline.InputMessages.Add(inputMessage);
      }

      var output = new MsgColl();
      Gppipeline.Execute(ppcontext);

      IBaseMessage om = Gppipeline.GetNextOutputMessage(ppcontext);
      return om;
    }
  }
}