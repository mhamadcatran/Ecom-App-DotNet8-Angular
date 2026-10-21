using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Ecom.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var Category = await work.CategoryRepositry.GetAllAsync();
                if (Category is null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(Category);    
                 

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyId(int id)
        {
            try
            {
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category is null)
                    return BadRequest(new ResponseAPI(400,$"not Found Category id  {id}"));
                return Ok(category);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.CategoryRepositry.AddAsync(category);
                return Ok(new ResponseAPI(200,"Item has been Added"));


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> update(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(updateCategoryDTO);
                await work.CategoryRepositry.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been Updated"));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }
        [HttpDelete("delete-Category/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                 await work.CategoryRepositry.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been Deleted"));

            }
            catch (Exception ex )
            {

                return BadRequest(ex.Message);

            }
        }

    }
}
