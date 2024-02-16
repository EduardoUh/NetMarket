using AutoMapper;
using Core.Entities;
using Core.Entities.Orders;

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
            CreateMap<Core.Entities.Address, AddressDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<AddressDto, Core.Entities.Orders.Address>();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(orderDto => orderDto.ShippingType, order => order.MapFrom(sp => sp.ShippingType.Name))
                .ForMember(orderDto => orderDto.ShippingTypePrice, order => order.MapFrom(sp => sp.ShippingType.Price));
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(orderItemResponse => orderItemResponse.ProductId, orderItem => orderItem.MapFrom(oi => oi.OrderedItem.ItemId))
                .ForMember(orderItemResponse => orderItemResponse.ProductName, orderItem => orderItem.MapFrom(oi => oi.OrderedItem.ItemName))
                .ForMember(orderItemResponse => orderItemResponse.ProductImage, orderItem => orderItem.MapFrom(oi => oi.OrderedItem.ImageUrl));
        }
    }
}
