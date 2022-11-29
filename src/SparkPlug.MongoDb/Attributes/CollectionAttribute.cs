namespace SparkPlug.MongoDb.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public class CollectionAttribute : Attribute
{
    public string Name { get; set; }
    public CollectionAttribute(string name)
    {
        Name = name;
    }
}
