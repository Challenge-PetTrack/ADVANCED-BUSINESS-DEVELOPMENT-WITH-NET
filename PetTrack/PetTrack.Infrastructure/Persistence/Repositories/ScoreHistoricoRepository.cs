using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class ScoreHistoricoRepository : IScoreHistoricoRepository
{
    private readonly PetTrackContext _context;

    public ScoreHistoricoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ScoreHistorico> GetAll() =>
        _context.ScoresHistorico.ToList();

    public ScoreHistorico? GetById(long id) =>
        _context.ScoresHistorico.FirstOrDefault(s => s.Id == id);

    public IReadOnlyList<ScoreHistorico> GetHistoricoByPet(long idPet) =>
        _context.ScoresHistorico
            .Where(s => s.Pet.Id == idPet)
            .OrderByDescending(s => s.DataRegistro)
            .ToList();

    public double GetMediaScoreByPet(long idPet)
    {
        var scores = _context.ScoresHistorico
            .Where(s => s.Pet.Id == idPet)
            .Select(s => s.Score)
            .ToList();

        return scores.Any() ? scores.Average() : 0;
    }

    public void Add(ScoreHistorico score)
    {
        _context.ScoresHistorico.Add(score);
        _context.SaveChanges();
    }

    public void Update(ScoreHistorico score)
    {
        _context.ScoresHistorico.Update(score);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var score = _context.ScoresHistorico.FirstOrDefault(s => s.Id == id);
        if (score == null) return false;
        _context.ScoresHistorico.Remove(score);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}