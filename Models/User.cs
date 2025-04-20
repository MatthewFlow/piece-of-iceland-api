using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace piece_of_iceland_api.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("username")]
    public string Username { get; set; } = null!;

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = null!;

    [BsonElement("ownedParcels")]
    public List<string> OwnedParcels { get; set; } = new();
}
