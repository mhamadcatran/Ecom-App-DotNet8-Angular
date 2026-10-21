

using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepositry(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;

            var product =mapper.Map<Product>(productDTO);
            await context.products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);

            var photo=ImagePath.Select(path=> new Photo
            {
                ImageName=path,
                ProductId=product.Id,

            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO is null)
            {
                return false;
            }
            var FindProduct = await context.products
                .Include(x=>x.Category)
                .Include(x=>x.Photos)
                .FirstOrDefaultAsync(x=>x.Id == productDTO.Id);
            if (FindProduct is null)
            {
                return false;
            }
            mapper.Map(productDTO,FindProduct);

            var FindPhoto = await context.Photos.Where(m=>m.ProductId==productDTO.Id).ToListAsync();

            foreach (var item in FindPhoto)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);

            }
            context.Photos.RemoveRange(FindPhoto);


            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = productDTO.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
