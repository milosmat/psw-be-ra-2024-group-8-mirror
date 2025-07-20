using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Stakeholders.API.Public.Administration;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class AdministratorService : /*CrudService<AccountInformationDto, User>*/ IAdministratorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRespository;
        private IMapper _mapper {  get; set; }
        public AdministratorService(IUserRepository userRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _personRespository = personRepository;
            _mapper = mapper;
        }

        public Result<PagedResult<AccountInformationDto>> GetPaged(int page, int pageSize)
        {
            try
            {
                var pagedResult = _userRepository.GetPaged(page, pageSize);
                var userAccounts = _mapper.Map<List<AccountInformationDto>>(pagedResult.Results); 

                var userIds = pagedResult.Results.Select(u => u.Id).ToList();

                var people = _personRespository.GetByUserIds(userIds);

                var peopleDict = people.ToDictionary(p => p.UserId, p => p.Email);

                foreach(var account in userAccounts)
                {
                    if(peopleDict.TryGetValue(account.Id, out var email))
                    {
                        account.Email = email;
                    }
                }
                
                return Result.Ok(new PagedResult<AccountInformationDto>(userAccounts, pagedResult.TotalCount));     
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to load the user accounts. Error: {ex.Message}");
            }
        }
        public Result UpdateUserStatus(long userId, bool isActive)
        {
            try
            {
                _userRepository.UpdateUserStatus(userId, isActive);
                return Result.Ok();
            }
            catch(KeyNotFoundException ex)
            {
                return Result.Fail($"Failed to update user status. Error: {ex.Message}");
            }
        }

    }
}
