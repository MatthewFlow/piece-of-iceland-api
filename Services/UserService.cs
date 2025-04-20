using MongoDB.Driver;
using piece_of_iceland_api.Models;

namespace piece_of_iceland_api.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration["MongoDb:ConnectionString"]);
        var database = mongoClient.GetDatabase(configuration["MongoDb:DatabaseName"]);
        _users = database.GetCollection<User>("users");
    }

    public async Task<List<User>> GetAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _users.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _users.ReplaceOneAsync(u => u.Id == id, updatedUser);

    public async Task DeleteAsync(string id) =>
        await _users.DeleteOneAsync(u => u.Id == id);
}
