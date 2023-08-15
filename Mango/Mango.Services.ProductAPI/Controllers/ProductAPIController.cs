using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public ProductApiController(AppDbContext appDbContext, IMapper mapper)
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
                var products = _appDbContext.Products.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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
                Product? product = _appDbContext.Products.FirstOrDefault(x => x.ProductId == id);
                if (product != null)
                {
                    _responseDto.Result = _mapper.Map<ProductDto>(product);
                    _responseDto.IsSuccess = true;
                }
                else
                {
                    _responseDto.Message = $"product Id : {id} not found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product? product = _mapper.Map<Product>(productDto);
                _appDbContext.Products.Add(product);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product? product = _mapper.Map<Product>(productDto);
                _appDbContext.Products.Update(product);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
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
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product? product = _appDbContext.Products.FirstOrDefault(x => x.ProductId == id);
                if (product != null)
                {
                    _appDbContext.Remove(product);
                    _appDbContext.SaveChanges();
                    _responseDto.IsSuccess = true;
                }
                else
                {
                    _responseDto.Message = $"product Id : {id} not found";
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
