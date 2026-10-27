using AutoMapper;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound() 
        {
            var category = await work.CategoryRepositry.GetByIdAsync(100);
            if (category == null) return NotFound();
            return Ok(category);

        }
        [HttpGet("server-Error")]
        public async Task<ActionResult> GetServerError()
        {
            var Category = await work.CategoryRepositry.GetByIdAsync(100);
            Category.Name = "";
            
            return Ok(Category);
        }
        [HttpGet("bad-request/{Id}")]
        public async Task<ActionResult> GetBadRequest(int Id)
        {
            return Ok();
            

        }
        [HttpGet("bad-request/")]
        public async Task<ActionResult> GetBadRequest()
        {
            return BadRequest();


        }


    }
}
