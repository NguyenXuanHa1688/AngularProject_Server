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
    public class gamesController : ControllerBase
    {
        private readonly DataContext _context;

        public gamesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<game>>> Getgame()
        {
          if (_context.game == null)
          {
              return NotFound();
          }
            return await _context.game.ToListAsync();
        }

        // GET: api/games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<game>> Getgame(int id)
        {
          if (_context.game == null)
          {
              return NotFound();
          }
            var game = await _context.game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putgame(int id, game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gameExists(id))
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

        // POST: api/games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<game>> Postgame(game game)
        {
          if (_context.game == null)
          {
              return Problem("Entity set 'DataContext.game'  is null.");
          }
            var gamelist = _context.game;
            foreach(var g in gamelist)
            {
                if(g.Name == game.Name)
                {
                    return BadRequest("You already have this game in your gamelist");
                }
            }
            _context.game.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getgame", new { id = game.Id }, game);
        }

        // DELETE: api/games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletegame(int id)
        {
            if (_context.game == null)
            {
                return NotFound();
            }
            var game = await _context.game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.game.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getMyGamelist")]
        public async Task<IActionResult> getLogUser(string request)
        {
            var product = _context.game.Where(g => (g.UserAdd.Contains(request)));
            return Ok(product);
        }

        private bool gameExists(int id)
        {
            return (_context.game?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
