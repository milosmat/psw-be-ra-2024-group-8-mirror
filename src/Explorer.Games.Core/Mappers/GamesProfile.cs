using AutoMapper;
using Explorer.Games.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Explorer.Games.API.Dtos;

namespace Explorer.Games.Core.Mappers
{
    public class GamesProfile : Profile
    {
        public GamesProfile()
        {
            // Mapiranje između Game i GameDTO
            CreateMap<GameDTO, Game>()
                .ForMember(dest => dest.Scores, opt => opt.MapFrom(src => src.Scores))
                .ReverseMap();

            // Mapiranje između GameScore i GameScoreDTO
            CreateMap<GameScoreDTO, GameScore>()
                .ReverseMap();


            // Ako imate specifične DTO objekte kao što je GameDTO, možete mapirati na odgovarajući tip u modelu
            CreateMap<GameDTO, Game>()
                .ForMember(dest => dest.Scores, opt => opt.MapFrom(src => src.Scores))
                .ReverseMap();
        }
    }
}
