using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class TouristEquipment : Entity
{
    public long TouristId { get; private set; }
    public long EquipmentId { get; private set; }

    public TouristEquipment(long touristId, long equipmentId)
    {
        if (touristId <= 0) throw new ArgumentException("Invalid TouristId.");
        if (equipmentId <= 0) throw new ArgumentException("Invalid EquipmentId.");

        TouristId = touristId;
        EquipmentId = equipmentId;
    }
}
