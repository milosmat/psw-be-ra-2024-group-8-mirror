using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TouristEquipmentDatabaseRepository : ITouristEquipmentRepository
{
    private readonly ToursContext _context;

    public TouristEquipmentDatabaseRepository(ToursContext context)
    {
        _context = context;
    }

    public TouristEquipment Create(TouristEquipment touristEquipment)
    {
        _context.TouristEquipments.Add(touristEquipment);
        _context.SaveChanges();
        return touristEquipment;
    }

    public void Delete(long id)
    {
        var entity = _context.TouristEquipments.Find(id);
        if (entity != null)
        {
            _context.TouristEquipments.Remove(entity);
            _context.SaveChanges();
        }
    }

    public TouristEquipment Get(long id)
    {
        return _context.TouristEquipments.Find(id);
    }

    public TouristEquipment GetByTouristAndEquipment(long touristId, long equipmentId)
    {
        return _context.TouristEquipments
            .FirstOrDefault(te => te.TouristId == touristId && te.EquipmentId == equipmentId);
    }
}
