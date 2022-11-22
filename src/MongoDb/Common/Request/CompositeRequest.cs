namespace SparkPlug.Common;

[Serializable]
public class CompositeRequest : Dictionary<string, IApiRequest>,  ICompositeRequest
{
  
}