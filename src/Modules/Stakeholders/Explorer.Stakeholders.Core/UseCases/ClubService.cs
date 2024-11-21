using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases;

public class ClubService : BaseService<ClubDto, Club>, IClubService
{
    public IClubRepository _clubRepository { get; set; }
    public IMapper _mapper { get; set; }

    public ClubService(IClubRepository clubRepository, IMapper mapper): base(mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public PagedResult<ClubDto> GetPaged(int page, int pageSize)
    {
        PagedResult<Club> clubs = _clubRepository.GetPaged(page, pageSize);

        var clubDtos = _mapper.Map<List<ClubDto>>(clubs.Results);
        return new PagedResult<ClubDto>(clubDtos, clubs.TotalCount);
    }

    public ClubDto Create(ClubDto newClub)
    {
        return _mapper.Map<ClubDto>(_clubRepository.Create(_mapper.Map<Club>(newClub)));
    }

    public void Delete(int id)
    {
        _clubRepository.Delete(id);
    }

    public ClubDto Update(ClubDto updatedClub)
    {
        return _mapper.Map<ClubDto>(_clubRepository.Update(_mapper.Map<Club>(updatedClub)));
    }

    public ClubDto Get(int id)
    {
        return _mapper.Map<ClubDto>(_clubRepository.Get(id));
    }



}