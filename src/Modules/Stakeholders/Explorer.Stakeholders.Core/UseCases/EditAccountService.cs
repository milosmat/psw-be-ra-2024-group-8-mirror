using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using AutoMapper;

namespace Explorer.Stakeholders.Core.UseCases;

public class EditAccountService : CrudService<AccountDto,Account>, IEditAccountService
{
    public EditAccountService(ICrudRepository<Account> crudRepository, IMapper mapper) : base(crudRepository, mapper) { }
}




