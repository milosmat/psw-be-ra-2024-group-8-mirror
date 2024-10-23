using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration;

public class TourPreferencesService : CrudService<TourPreferencesDto, TourPreferences>, ITourPreferenceService
{
    public TourPreferencesService(ICrudRepository<TourPreferences> crudRepository, IMapper mapper) : base(crudRepository, mapper) {}
}
