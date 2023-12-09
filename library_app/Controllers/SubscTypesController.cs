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
    public class SubscTypesController : ControllerBase
    {
        private IAppServices _services;
        private ILogger<SubscTypesController> _logger;
        public List<String> Errors = new();

        public SubscTypesController(IAppServices services, ILogger<SubscTypesController> logger)
        {
            _services = services;
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubscriptionTypeShowDTO>>> GetAll()
        {
            try
            {
                var results = await _services.SubscriptionTypeService.GetAll();
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
        public async Task<ActionResult<SubscriptionTypeShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.SubscriptionTypeService.GetById(id);
                return Ok(result);
            }
            catch(EntityNotFoundException e)
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
        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<SubscriptionTypeShowDTO>> Insert(SubscriptionTypeInsertDTO insertDTO)
        {
            if(!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        Errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(Errors);
            }
            try
            {

                var result = await _services.SubscriptionTypeService.Insert(insertDTO);
                return CreatedAtAction("GetOne", new { id = result.Id }, result);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<SubscriptionTypeShowDTO>> Update(int id,  SubscriptionTypeUpdateDTO updateDTO)
        {

            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(Errors);
            }
            if(id != updateDTO.Id)
            {
                return Unauthorized();
            }
            try
            {
                var modified = await _services.SubscriptionTypeService.Update(updateDTO);
                return Ok(modified);
            }
            catch(EntityNotFoundException e)
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
                await _services.SubscriptionTypeService.Delete(id);
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
                return Problem(e.Message);
            }
        }
    }
}
