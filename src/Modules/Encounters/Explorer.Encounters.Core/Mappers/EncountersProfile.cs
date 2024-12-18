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

            CreateMap<EncounterDTO, Encounter>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new MapLocation(src.Location.Latitude, src.Location.Longitude)))
            .ReverseMap();
            CreateMap<TouristProfile, TouristProfileDTO>().ReverseMap();
            CreateMap<Encounter, EncounterDTO>().ReverseMap();

            CreateMap<Domain.EncounterStatus, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Domain.EncounterStatus>().ConvertUsing(src => Enum.Parse<Domain.EncounterStatus>(src));

            CreateMap<Domain.EncounterType, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Domain.EncounterType>().ConvertUsing(src => Enum.Parse<Domain.EncounterType>(src));

            CreateMap<MapLocationDTO, MapLocation>()
                .ConstructUsing(src => new MapLocation(src.Latitude, src.Longitude))
                .ReverseMap();

            CreateMap<Tours.API.Dtos.TouristPositionDto.MapLocationDto, MapLocation>()
                .ConstructUsing(src => new MapLocation(src.Latitude, src.Longitude))
                .ReverseMap();

        }
    }
}
