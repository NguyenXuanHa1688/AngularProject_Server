using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDtoController : ControllerBase
    {
        private readonly DataContext _context;

        public UserDtoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/UserDto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserDto()
        {
          if (_context.UserDto == null)
          {
              return NotFound();
          }
            return await _context.UserDto.ToListAsync();
        }

        // GET: api/UserDto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserDto(int id)
        {
          if (_context.UserDto == null)
          {
              return NotFound();
          }
            var userDto = await _context.UserDto.FindAsync(id);

            if (userDto == null)
            {
                return NotFound();
            }

            return userDto;
        }

        // PUT: api/UserDto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDto(int id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDtoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserDto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUserDto(UserDto userDto)
        {
          if (_context.UserDto == null)
          {
              return Problem("Entity set 'DataContext.UserDto'  is null.");
          }
            _context.UserDto.Add(userDto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDto", new { id = userDto.Id }, userDto);
        }

        // DELETE: api/UserDto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDto(int id)
        {
            if (_context.UserDto == null && _context.User == null)
            {
                return NotFound();
            }
            var userDto = await _context.UserDto.FindAsync(id);
            var user = await _context.User.FindAsync(id);
            if (userDto == null && user == null)
            {
                return NotFound();
            }

            _context.UserDto.Remove(userDto);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("finduser")]
        public async Task<IActionResult> getLogUser(string request)
        {
            var user = _context.UserDto.Where(l => (l.UserName.Contains(request)));
            return Ok(user);
        }

        private bool UserDtoExists(int id)
        {
            return (_context.UserDto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
