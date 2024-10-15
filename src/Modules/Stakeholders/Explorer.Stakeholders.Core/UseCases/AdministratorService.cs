using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class AdministratorService : CrudService<AccountInformationDto, User>, IAdministratorService
    {
        public AdministratorService(ICrudRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
