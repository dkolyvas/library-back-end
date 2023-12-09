using library_app.DTO;
using library_app.Exceptions;
using library_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private IAppServices _services;
        private ILogger<CategoriesController> _logger;

        public CategoriesController(IAppServices services, ILogger<CategoriesController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryShowDTO>>> GetAll()
        {
            try
            {
                var results = await _services.CategoryService.GetAll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.CategoryService.GetById(id);
                return Ok(result);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
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
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }

        }
        [HttpPut("{id}")]
        [Authorize(Roles ="admin")]
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
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _services.CategoryService.DeleteById(id);
                return Ok();
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message) ;
            }
        }
        
    }
}
