using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class MedicamentoRepository : IMedicamentoRepository
{
    private readonly PetTrackContext _context;

    public MedicamentoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Medicamento> GetAll() =>
        _context.Medicamentos.ToList();

    public Medicamento? GetById(long id) =>
        _context.Medicamentos.FirstOrDefault(m => m.Id == id);

    public IReadOnlyList<Medicamento> GetByNome(string nome) =>
        _context.Medicamentos
            .Where(m => m.Nome.Contains(nome))
            .ToList();

    public IReadOnlyList<Medicamento> GetMedicamentosAtivosByPet(long idPet) =>
        _context.Medicamentos
            .Where(m => m.Evento.Pet.Id == idPet && (m.DataFim == null || m.DataFim >= DateTime.Now))
            .ToList();

    public void Add(Medicamento medicamento)
    {
        _context.Medicamentos.Add(medicamento);
        _context.SaveChanges();
    }

    public void Update(Medicamento medicamento)
    {
        _context.Medicamentos.Update(medicamento);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var medicamento = _context.Medicamentos.FirstOrDefault(m => m.Id == id);
        if (medicamento == null) return false;
        _context.Medicamentos.Remove(medicamento);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}