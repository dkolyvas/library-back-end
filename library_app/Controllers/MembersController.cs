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
    public class MembersController : ControllerBase
    {
        private IAppServices _services;
        public string? Errors;
        private ILogger<MembersController> _logger; 

        public MembersController(IAppServices appServices, ILogger<MembersController> logger)
        {
            this._services = appServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberShowDTO>>> GetAll([FromQuery] string? name)
        {
            List<MemberShowDTO> results = new();
            try
            {
                if (name == null)
                {
                    results = await _services.MemberService.GetMembers();

                }
                else
                {
                    results = await _services.MemberService.GetMembersByName(name);
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberShowDTO>> GetOne(int id)
        {
            try
            {
                var result = await _services.MemberService.GetMemberById(id);
                return Ok(result);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<MemberShowDTO>> GetOneByEmail(string email)
        {
            try
            {
                var result = await _services.MemberService.GetMemberByEmail(email);
                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MemberShowDTO>> Insert(MemberInsertDTO insertDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var error  in value.Errors)
                    {
                        Errors += error.ErrorMessage + " | ";
                    }
                }
                return BadRequest(Errors);
            }
            try
            {
                var newMember = await _services.MemberService.InsertMember(insertDTO);
                return CreatedAtAction("GetOne", new { id = newMember.Id }, newMember);
            }
            catch(EmailExistsException e)
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
        public async Task<ActionResult<MemberShowDTO>> Update(int id, MemberUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        Errors += error.ErrorMessage;
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
                var modifiedMember = await _services.MemberService.UpdateMember(updateDTO);
                return Ok(modifiedMember);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(EmailExistsException e)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await _services.MemberService.DeleteMember(id);
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
