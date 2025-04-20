using MongoDB.Driver;
using piece_of_iceland_api.Models;
using Microsoft.Extensions.Options;

namespace piece_of_iceland_api.Services;

public class ParcelService
{
    private readonly IMongoCollection<Parcel> _parcels;

    public ParcelService(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration["MongoDb:ConnectionString"]);
        var database = mongoClient.GetDatabase(configuration["MongoDb:DatabaseName"]);
        _parcels = database.GetCollection<Parcel>("parcels");
    }

    public async Task<List<Parcel>> GetAsync() =>
        await _parcels.Find(_ => true).ToListAsync();

    public async Task<Parcel?> GetAsync(string id) =>
        await _parcels.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Parcel newParcel) =>
        await _parcels.InsertOneAsync(newParcel);

    public async Task UpdateAsync(string id, Parcel updatedParcel) =>
        await _parcels.ReplaceOneAsync(p => p.Id == id, updatedParcel);

    public async Task DeleteAsync(string id) =>
        await _parcels.DeleteOneAsync(p => p.Id == id);
}
