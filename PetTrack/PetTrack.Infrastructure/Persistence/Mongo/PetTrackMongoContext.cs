using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Mongo;

/// <summary>
/// Contexto de acesso e gerenciamento da conexão com o banco NoSQL MongoDB.
/// </summary>
public class PetTrackMongoContext
{
    private readonly IMongoDatabase _database;
    private readonly IMongoClient _client;
    private readonly string _collectionName;

    public PetTrackMongoContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"] ?? "mongodb://127.0.0.1:27017";
        var databaseName = configuration["MongoDbSettings:DatabaseName"] ?? "pettrack_sprint4";
        _collectionName = configuration["MongoDbSettings:CollectionName"] ?? "pets";

        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3);

        _client = new MongoClient(settings);
        _database = _client.GetDatabase(databaseName);
    }

    /// <summary>Retorna a coleção de documentos 'pets'.</summary>
    public IMongoCollection<PetMongoDocument> Pets => _database.GetCollection<PetMongoDocument>(_collectionName);

    /// <summary>Executa comando de ping para verificar conectividade com o MongoDB.</summary>
    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
            return result.Contains("ok") && result["ok"].ToDouble() > 0;
        }
        catch
        {
            return false;
        }
    }
}
