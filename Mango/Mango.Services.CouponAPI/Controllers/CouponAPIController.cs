using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public CouponApiController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var coupons = _appDbContext.Coupons.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon? coupon = _appDbContext.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon != null)
                {
                    _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                    _responseDto.IsSuccess = true;
                }
                else
                {
                    _responseDto.Message = $"Coupon Id : {id} not found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon? coupon = _appDbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if (coupon != null)
                {
                    _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                    _responseDto.IsSuccess = true;
                }
                else
                {
                    _responseDto.Message = $"Coupon code : {code} not found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon? coupon = _mapper.Map<Coupon>(couponDto);
                _appDbContext.Coupons.Add(coupon);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon? coupon = _mapper.Map<Coupon>(couponDto);
                _appDbContext.Coupons.Update(coupon);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon? coupon = _appDbContext.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon != null)
                {
                    _appDbContext.Remove(coupon);
                    _appDbContext.SaveChanges();
                    _responseDto.IsSuccess = true;
                }
                else
                {
                    _responseDto.Message = $"Coupon Id : {id} not found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
