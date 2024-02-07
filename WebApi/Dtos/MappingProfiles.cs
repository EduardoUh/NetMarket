using AutoMapper;
using Core.Entities;

namespace WebApi.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                // we can be more specific when mapping, we can indicate the specific property
                // to be mapped to a property of our dto object
                .ForMember(dto => dto.CategoryName, p => p.MapFrom(p => p.Category!.Name))
                .ForMember(dto => dto.BrandName, p => p.MapFrom(p => p.Brand!.Name));
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
