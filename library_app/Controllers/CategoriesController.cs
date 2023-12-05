using library_app.DTO;
using library_app.Exceptions;
using library_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private IAppServices _services;

        public CategoriesController(IAppServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryShowDTO>>> GetAll()
        {
            try
            {
                var results = await _services.CategoryService.GetAll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.CategoryService.GetById(id);
                return Ok(result);
            }
            catch(EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryShowDTO>> Insert(CategoryInsertDTO insertDTO)
        {
            if (!ModelState.IsValid)
            {
                string? errorMessage = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(errorMessage);              
                
            }
            try
            {
                var result = await _services.CategoryService.Insert(insertDTO.categoryName);
                return CreatedAtAction("GetOne", new { id = result.Id }, result);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryUpdateDTO>> Update(int id, CategoryUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                string? errorMessage = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(errorMessage);

            }
            if(id != updateDTO.Id)
            {
                return Unauthorized();
            }
            try
            {
                var modified = await _services.CategoryService.Update(updateDTO);
                return Ok(modified);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _services.CategoryService.DeleteById(id);
                return Ok();
            }
            catch(EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return Problem(e.Message) ;
            }
        }
        
    }
}
