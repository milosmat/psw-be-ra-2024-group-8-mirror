using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class AppRatingProfile : Profile
    {
        public AppRatingProfile() { 
            CreateMap<AppRatingDto, AppRating>().ReverseMap();
        }
    }
}
