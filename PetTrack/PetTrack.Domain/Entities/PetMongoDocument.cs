using MongoDB.Bson.Serialization.Attributes;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Documento NoSQL armazenado na coleção 'pets' do MongoDB (banco 'pettrack_sprint4').
/// Representa a visão desnormalizada e de alta performance de Pets e seus vínculos.
/// </summary>
public class PetMongoDocument
{
    /// <summary>Identificador primário no MongoDB (_id).</summary>
    [BsonId]
    public long Id { get; set; }

    /// <summary>Identificador relacional correspondente no Oracle.</summary>
    [BsonElement("idPet")]
    public long IdPet { get; set; }

    /// <summary>Nome do animal.</summary>
    [BsonElement("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>Espécie (ex: Cão, Gato).</summary>
    [BsonElement("especie")]
    public string Especie { get; set; } = string.Empty;

    /// <summary>Raça do pet.</summary>
    [BsonElement("raca")]
    public string? Raca { get; set; }

    /// <summary>Peso em kg.</summary>
    [BsonElement("pesoKg")]
    public double? PesoKg { get; set; }

    /// <summary>Dados embutidos do tutor responsável.</summary>
    [BsonElement("tutor")]
    public PetMongoTutor Tutor { get; set; } = new();

    /// <summary>Dados embutidos da clínica veterinária vinculada.</summary>
    [BsonElement("clinica")]
    public PetMongoClinica Clinica { get; set; } = new();

    /// <summary>Data e hora da última sincronização/atualização no NoSQL.</summary>
    [BsonElement("dataAtualizacao")]
    public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
}

/// <summary>Subdocumento do Tutor para o MongoDB.</summary>
public class PetMongoTutor
{
    [BsonElement("id")]
    public long Id { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; } = string.Empty;
}

/// <summary>Subdocumento da Clínica para o MongoDB.</summary>
public class PetMongoClinica
{
    [BsonElement("id")]
    public long Id { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; } = string.Empty;
}
