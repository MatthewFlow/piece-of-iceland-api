using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace piece_of_iceland_api.Models;

public class Parcel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("x")]
    public int X { get; set; }

    [BsonElement("y")]
    public int Y { get; set; }

    [BsonElement("isAvailable")]
    public bool IsAvailable { get; set; } = true;

    [BsonElement("ownerId")]
    public string? OwnerId { get; set; }
}
