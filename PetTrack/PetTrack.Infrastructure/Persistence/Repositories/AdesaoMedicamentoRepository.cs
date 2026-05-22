using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class AdesaoMedicamentoRepository : IAdesaoMedicamentoRepository
{
    private readonly PetTrackContext _context;

    public AdesaoMedicamentoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<AdesaoMedicamento> GetAll() =>
        _context.AdesoesMedicamento.ToList();

    public AdesaoMedicamento? GetById(long id) =>
        _context.AdesoesMedicamento.FirstOrDefault(a => a.Id == id);

    public IReadOnlyList<AdesaoMedicamento> GetByMedicamentoId(long idMedicamento) =>
        _context.AdesoesMedicamento
            .Where(a => a.Medicamento.Id == idMedicamento)
            .ToList();

    public IReadOnlyList<AdesaoMedicamento> GetByStatus(SimNaoEnum status) =>
        _context.AdesoesMedicamento
            .Where(a => a.Status == status)
            .ToList();

    public void Add(AdesaoMedicamento adesao)
    {
        _context.AdesoesMedicamento.Add(adesao);
        _context.SaveChanges();
    }

    public void Update(AdesaoMedicamento adesao)
    {
        _context.AdesoesMedicamento.Update(adesao);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var adesao = _context.AdesoesMedicamento.FirstOrDefault(a => a.Id == id);
        if (adesao == null) return false;
        _context.AdesoesMedicamento.Remove(adesao);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}