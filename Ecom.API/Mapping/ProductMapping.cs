using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping
{
    public class ProductMapping:Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(destinationMember: x=>x.CategoryName,
                memberOptions: op=>op
                .MapFrom(mapExpression: src=>src.Category.Name))
                .ReverseMap();
            CreateMap<Photo,PhotoDTO>().ReverseMap();
            CreateMap<AddProductDTO,Product>()
                .ForMember(m=>m.Photos,OP=>OP.Ignore())
                .ReverseMap();
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(m => m.Photos, op => op.Ignore())
                .ReverseMap();


        }
    }
}
