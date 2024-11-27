using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Author;

public class BundleService : CrudService<BundleDTO, Bundle>, IBundleService
{
    private readonly ICrudRepository<BundleTour> _bundleTourRepository;
    private readonly IBundleRepository _bundleRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;

    public BundleService(ICrudRepository<Bundle> repository, IMapper mapper,
        ICrudRepository<BundleTour> bundleTourRepository,
        IBundleRepository bundleRepository, ITourRepository tourRepository) : base(repository, mapper)
    {
        _mapper = mapper;
        _bundleTourRepository = bundleTourRepository;
        _bundleRepository = bundleRepository;
        _tourRepository = tourRepository;
    }

    public Result AddTour(int bundleId, BundleTourDTO bundleTourDto)
    {
        var bundleTour = _mapper.Map<BundleTour>(bundleTourDto);
        var bundle = CrudRepository.Get(bundleId);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        var result = bundle.AddTour(bundleTour);
        if (result.IsSuccess)
            CrudRepository.Update(bundle);

        return result;
    }

    public Result RemoveTour(int bundleId, BundleTourDTO bundleTourDto)
    {
        var bundleTour = _mapper.Map<BundleTour>(bundleTourDto);
        var bundle = CrudRepository.Get(bundleId);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        var result = bundle.RemoveTour(bundleTour);
        if (result.IsSuccess)
            CrudRepository.Update(bundle);

        return result;
    }

    public Result<PagedResult<BundleTourDTO>> GetPagedTours(int bundleId, int page, int pageSize)
    {
        var bundle = CrudRepository.Get(bundleId, b => b.Tours);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        var tours = bundle.Tours.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var tourDtos = tours.Select(t => _mapper.Map<BundleTourDTO>(t)).ToList();
        var pagedResult = new PagedResult<BundleTourDTO>(tourDtos, bundle.Tours.Count);

        return Result.Ok(pagedResult);
    }

    public new Result<PagedResult<BundleDTO>> GetPaged(int page, int pageSize)
    {
        var result = _bundleRepository.GetPaged(page, pageSize);
        return MapToDto(result);
    }

    public new Result<BundleDTO> Update(BundleDTO entity)
    {
        try
        {
            var result = _bundleRepository.Update(MapToDomain(entity));
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result SetStatus(int bundleId, int status)
    {
        var bundle = CrudRepository.Get(bundleId);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        if (!Enum.IsDefined(typeof(BundleStatus), status))
            return Result.Fail("Invalid status value.");

        bundle.UpdateStatus((BundleStatus)status);

        CrudRepository.Update(bundle);

        return Result.Ok();
    }

    public Result ArchiveBundle(int bundleId)
    {
        var bundle = CrudRepository.Get(bundleId);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        bundle.Archive(); 
        CrudRepository.Update(bundle);

        return Result.Ok();
    }

    public Result<List<BundleDTO>> GetPublishedBundles()
    {
        var bundles = _bundleRepository.GetPublishedBundles();
        var bundleDtos = _mapper.Map<List<BundleDTO>>(bundles);
        return Result.Ok(bundleDtos);
    }

    public Result<BundleDTO> Get(int id)
    {
        var bundle = CrudRepository.Get(id);
        if (bundle == null)
            return Result.Fail("Bundle not found.");

        return MapToDto(bundle);
    }

    public override Result<BundleDTO> Create(BundleDTO dto)
    {
        var entity = MapToDomain(dto);
        var createdEntity = CrudRepository.Create(entity);
        return MapToDto(createdEntity);
    }


    public Result CheckToursStatus(long bundleId)
    {
        var bundle = CrudRepository.Get(bundleId, b => b.Tours); // Učitaj bundle sa turama
        if (bundle == null)
        {
            return Result.Fail("Bundle not found.");
        }

        foreach (var bundleTour in bundle.Tours)
        {
            var tour = _tourRepository.Get(bundleTour.TourId); // Pronađi turu po ID-u
            if (tour == null)
            {
                return Result.Fail($"Tour with ID {bundleTour.TourId} not found.");
            }

            if (tour.Status != TourStatus.PUBLISHED)
            {
                return Result.Fail($"Tour with ID {tour.Id} is not published.");
            }
        }

        bundle.UpdateStatus(BundleStatus.PUBLISHED);

        CrudRepository.Update(bundle);

        return Result.Ok();
    }





}
