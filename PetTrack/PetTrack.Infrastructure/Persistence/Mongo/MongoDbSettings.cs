namespace PetTrack.Infrastructure.Persistence.Mongo;

/// <summary>
/// Configurações de conexão para o banco NoSQL MongoDB.
/// </summary>
public class MongoDbSettings
{
    /// <summary>String de conexão com o servidor MongoDB (padrão: mongodb://127.0.0.1:27017).</summary>
    public string ConnectionString { get; set; } = "mongodb://127.0.0.1:27017";

    /// <summary>Nome da base de dados (padrão: pettrack_sprint4).</summary>
    public string DatabaseName { get; set; } = "pettrack_sprint4";

    /// <summary>Nome da coleção principal de documentos (padrão: pets).</summary>
    public string CollectionName { get; set; } = "pets";
}
