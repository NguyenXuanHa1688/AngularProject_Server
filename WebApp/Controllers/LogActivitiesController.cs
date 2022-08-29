using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class LogActivitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public LogActivitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/LogActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogActivities>>> GetLogActivities()
        {
          if (_context.LogActivities == null)
          {
              return NotFound();
          }
            return await _context.LogActivities.ToListAsync();
        }

        // GET: api/LogActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogActivities>> GetLogActivities(int id)
        {
          if (_context.LogActivities == null)
          {
              return NotFound();
          }
            var logActivities = await _context.LogActivities.FindAsync(id);

            if (logActivities == null)
            {
                return NotFound();
            }

            return logActivities;
        }

        // PUT: api/LogActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogActivities(int id, LogActivities logActivities)
        {
            if (id != logActivities.Id)
            {
                return BadRequest();
            }

            _context.Entry(logActivities).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogActivitiesExists(id))
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

        // POST: api/LogActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogActivities>> PostLogActivities(LogActivities logActivities)
        {
          if (_context.LogActivities == null)
          {
              return Problem("Entity set 'DataContext.LogActivities'  is null.");
          }
            _context.LogActivities.Add(logActivities);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogActivities", new { id = logActivities.Id }, logActivities);
        }

        // DELETE: api/LogActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogActivities(int id)
        {
            if (_context.LogActivities == null)
            {
                return NotFound();
            }
            var logActivities = await _context.LogActivities.FindAsync(id);
            if (logActivities == null)
            {
                return NotFound();
            }

            _context.LogActivities.Remove(logActivities);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogActivitiesExists(int id)
        {
            return (_context.LogActivities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
