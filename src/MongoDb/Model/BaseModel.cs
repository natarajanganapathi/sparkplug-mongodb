namespace SparkPlug.MongoDb.Model;

[BsonIgnoreExtraElements]
public abstract class BaseModel
{
    [BsonElement("_id")]
    [BsonId]
    [BsonIgnoreIfDefault]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string? Id { get; set; }

    public virtual string? GetId()
    {
        return Id;
    }
}