using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class CouponService : BaseService<CouponDTO, Coupon>, ICouponService
    {
        public ICouponRepository _couponRepository { get; set; }
        public IMapper _mapper { get; set; }

        public CouponService(ICouponRepository couponRepository, IMapper mapper) : base(mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public PagedResult<CouponDTO> GetPaged(int page, int pageSize)
        {
            PagedResult<Coupon> coupons = _couponRepository.GetPaged(page, pageSize);

            var couponDtos = _mapper.Map<List<CouponDTO>>(coupons.Results);
            return new PagedResult<CouponDTO>(couponDtos, coupons.TotalCount);
        }


        public CouponDTO Create(CouponDTO newCoupon)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Create(_mapper.Map<Coupon>(newCoupon)));
        }


        public void Delete(int id)
        {
            _couponRepository.Delete(id);
        }

        public CouponDTO Update(CouponDTO updateCoupon)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Update(_mapper.Map<Coupon>(updateCoupon)));
        }

        public CouponDTO Get(int id)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Get(id));
        }
    }
}
