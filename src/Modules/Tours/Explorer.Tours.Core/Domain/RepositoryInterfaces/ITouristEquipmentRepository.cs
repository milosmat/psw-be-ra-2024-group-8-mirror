using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITouristEquipmentRepository
{
    TouristEquipment Get(long id);
    TouristEquipment GetByTouristAndEquipment(long touristId, long equipmentId);
    void Delete(long id);
    TouristEquipment Create(TouristEquipment touristEquipment);
}
