using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Services.ProductAPI
{
    public static class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
