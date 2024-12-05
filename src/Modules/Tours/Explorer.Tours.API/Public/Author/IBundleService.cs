using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Author;


public interface IBundleService
{
    Result AddTour(int bundleId, BundleTourDTO bundleTourDto);
    Result<PagedResult<BundleTourDTO>> GetPagedTours(int bundleId, int page, int pageSize);
    Result<PagedResult<BundleDTO>> GetPaged(int page, int pageSize);
    Result<BundleDTO> Update(BundleDTO entity);
    Result SetStatus(int bundleId, int status);
    Result ArchiveBundle(int bundleId);
    Result<List<BundleDTO>> GetPublishedBundles();
    Result<List<BundleDTO>> GetAllBundles();
    Result<BundleDTO> Get(int id);
    BundleDTO Create(BundleDTO newBundle);
    Result CheckToursStatus(long bundleId);
    BundleDTO GetById(int id);
    Result DeleteTour(int id);
    Result Delete(int id);

    Result<List<BundleTourDTO>> GetAllTours(int bundleId);



}

