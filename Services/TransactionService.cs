using MongoDB.Driver;
using piece_of_iceland_api.Models;

namespace piece_of_iceland_api.Services;

public class TransactionService
{
    private readonly IMongoCollection<Transaction> _transactions;

    public TransactionService(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration["MongoDb:ConnectionString"]);
        var database = mongoClient.GetDatabase(configuration["MongoDb:DatabaseName"]);
        _transactions = database.GetCollection<Transaction>("transactions");
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactions.Find(_ => true).ToListAsync();

    public async Task<Transaction?> GetAsync(string id) =>
        await _transactions.Find(t => t.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Transaction newTransaction) =>
        await _transactions.InsertOneAsync(newTransaction);

    public async Task DeleteAsync(string id) =>
        await _transactions.DeleteOneAsync(t => t.Id == id);
}
