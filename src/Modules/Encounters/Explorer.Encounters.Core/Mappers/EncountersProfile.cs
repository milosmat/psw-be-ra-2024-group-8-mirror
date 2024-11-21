using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;
using Explorer.Tours.Core.Domain;
using static Explorer.Encounters.API.Dtos.EncounterDTO;

namespace Explorer.Encounters.Core.Mappers
{
    public class EncountersProfile : Profile
    {
        public EncountersProfile()
        {
            // Mapiranje Encounter i EncounterDTO
            CreateMap<Encounter, EncounterDTO>().ReverseMap();

            // Ako postoje dodatni entiteti vezani za Encounter (npr. EncounterDetails), dodajte ovde:
            /*
            CreateMap<EncounterDetail, EncounterDetailDTO>().ReverseMap();
            */

            // Mapiranje za enum vrednosti, ako je potrebno za specifične slučajeve
            CreateMap<Domain.EncounterStatus, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Domain.EncounterStatus>().ConvertUsing(src => Enum.Parse<Domain.EncounterStatus>(src));

            CreateMap<Domain.EncounterType, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Domain.EncounterType>().ConvertUsing(src => Enum.Parse<Domain.EncounterType>(src));

            CreateMap<MapLocationDto, MapLocation>()
                .ConstructUsing(src => new MapLocation(src.Latitude, src.Longitude))
                .ReverseMap();
        }
    }
}
