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
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private IAppServices _services;
        private ILogger<SubscriptionsController> _logger;
        public List<string> Errors = new();

        public SubscriptionsController(IAppServices services, ILogger<SubscriptionsController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.SubscriptionService.GetSubscription(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
        [HttpGet("Membercurrent/{memberId}")]
        public async Task<ActionResult<SubscriptionShowDTO>> GetMemberCurrent(int memberId)
        {
            try
            {
                var result = await _services.SubscriptionService.GetMemberCurrentSubscription(memberId);
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
        [HttpGet("Member/{memberId}")]
        public async Task<ActionResult<IEnumerable<SubscriptionShowDTO>>> GetMemberHistory(int memberId)
        {
            try
            {
                var results = await _services.SubscriptionService.GetMemberSubscriptions(memberId);
                return Ok(results);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<SubscriptionShowDTO>> Create(SubscriptionInsertDTO insertDTO)
        {
            try
            {
                var newSubscription = await _services.SubscriptionService.CreateSubscription(insertDTO);
                return CreatedAtAction("GetOne", new { id = newSubscription.Id }, newSubscription);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubscriptionShowDTO>> Stop(int id)
        {
            try
            {
                var modified = await _services.SubscriptionService.StopSubscription(id);
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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _services.SubscriptionService.DeleteSubscription(id);
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
