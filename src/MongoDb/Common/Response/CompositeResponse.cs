namespace SparkPlug.Common;

[Serializable]
public class CompositeResponse : Dictionary<string, IApiResponse>, ICompositeResponse
{
   
}