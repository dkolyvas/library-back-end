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
    public class BorrowingsController : ControllerBase
    {
        private IAppServices _services;
        private ILogger<BorrowingsController> _logger;

        public BorrowingsController(IAppServices services, ILogger<BorrowingsController> logger)
        {
            _services = services;
            _logger = logger;
        }
        [HttpGet("Member/{memberId}")]
        public async Task<ActionResult<IEnumerable<BorrowingShowDTO>>> GetAllForMember(int memberId)
        {
            try
            {
                var results = await _services.BorrowingService.BorrowingHistoryForMember(memberId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult<IEnumerable<BorrowingShowDTO>>> GetAllForBook(int bookId)
        {
            try
            {
                var results = await _services.BorrowingService.BorrowingHistoryForBook(bookId)   ;
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowingShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.BorrowingService.GetBorrowing(id);
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
        [HttpGet("Expired")]
        public async Task<ActionResult<IEnumerable<BorrowingShowDTO>>> GetExpired()
        {
            try
            {
                var results = await _services.BorrowingService.FindDueBooks();
                return Ok(results);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }
        [HttpGet("CanBorrow/{id}")]
        public async Task<ActionResult<bool>> MemberCanBorrow(int id)
        {
            try
            {
                bool canBorrow =  await _services.BorrowingService.MemberCanBorrow(id);
                return Ok(canBorrow);

            }
            catch (BorrowingNotAllowedException e)
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
        public async Task<ActionResult<BorrowingShowDTO>> Borrow(BorrowingInsertDTO insertDTO)
        {
            try
            {
                var result = await _services.BorrowingService.Borrow(insertDTO);
                return CreatedAtAction("GetOne", new { id = result.Id }, result);
            }
            catch(BorrowingNotAllowedException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<BorrowingShowDTO>> Renew(int id)
        {
            try
            {
                var result = await _services.BorrowingService.RenewBorrowing(id);
                return CreatedAtAction("GetOne", new { id = result.Id }, result);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(BorrowingNotAllowedException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BorrowingShowDTO>> ReturnBook(int id)
        {
            try
            {
                var result = await _services.BorrowingService.ReturnBook(id);
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
                return Problem(e.Message) ;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                await _services.BorrowingService.DeleteBorrowing(id);
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

        [HttpGet("Borrowedbooks/{id}")]
        public async Task<ActionResult<int>> GetBorrowedBooksForCurrentSubscription(int id)
        {
            try
            {
               return await _services.BorrowingService.GetBorrowedBooksForCurrentSubscription(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
            

        }
    }
}
