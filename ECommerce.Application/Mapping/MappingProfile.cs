using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Product, ProductDto>()
        //    .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
        //    .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
        //    .ForCtorParam("Amount", o => o.MapFrom(s => s.Price.Amount))
        //    .ForCtorParam("Currency", o => o.MapFrom(s => s.Price.Currency));

        CreateMap<Order, OrderDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("Status", o => o.MapFrom(s => s.Status))
            .ForCtorParam("CreatedAt", o => o.MapFrom(s => s.CreatedAt))
            .ForCtorParam("TotalAmount", o => o.MapFrom(s => s.Items.Sum(i => i.UnitPrice.Amount * i.Quantity)))
            .ForCtorParam("Currency", o => o.MapFrom(s => s.Items.First().UnitPrice.Currency));
    }
}
