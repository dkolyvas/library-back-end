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
    public class BooksController : ControllerBase
    {
        private IAppServices _services;
        public string? errors ;
        private ILogger<BooksController> _logger;

        public BooksController(IAppServices services, ILogger<BooksController> logger)
        {
            _services = services;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookShowDTO>>> GetAll([FromQuery] string? title, [FromQuery] string? author, 
            [FromQuery] string? isbn, [FromQuery] int? category)
        {
            BookSearchDTO searchCriteria = new()
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                Category_Id = category
            };

            try
            {
                var results = await _services.BookService.FindBooks(searchCriteria);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookShowDTO>> GetOne(int id)
        {
            try
            {
                var book = await _services.BookService.GetBook(id);
                return Ok(book);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                errors += e.Message + " | ";
                return NotFound(errors);
            }    
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookShowDTO>> InsertBook(BookInsertDTO insertDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors += error.ErrorMessage + " | ";
                    }
                }
                return BadRequest(errors);
            }
            try
            {
                var book = await _services.BookService.InsertBook(insertDTO);
                return CreatedAtAction("GetOne", new {id = book.Id}, book);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookShowDTO>> UpdateBook(int id, BookUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors += error.ErrorMessage + " | ";
                    }
                }
                return BadRequest(errors);
            }
            if(!(id == updateDTO.Id))
            {
                return Unauthorized();
            }
            try
            {
                var book = await _services.BookService.UpdateBook(updateDTO);
                return Ok(book);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _services.BookService.DeleteBook(id);
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
