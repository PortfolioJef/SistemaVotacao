using IMDbApi.Context;
using IMDbApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDb.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly dbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string keyPass;

        public AdministratorsController(IConfiguration configuration, dbContext context)
        {
            _configuration = configuration;
            _context = context;
            keyPass = _configuration["CtsAcs:scKey"];
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Cadastro")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.isAdmin = false;
            user.Password = Util.Encrypt(user.Password, keyPass);

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        [Route("Edição")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            user.isAdmin = true;
            user.Password = Util.Encrypt(user.Password, keyPass);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbe)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw dbe;
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Somente Deleção Lógica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        [Route("DesativarUsuario")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Entry(user).State = EntityState.Modified;

            user.isDeleted = true;
            await _context.SaveChangesAsync();
            return user;
        }

        private bool UserExists(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ObterUsuariosNaoAdminAtivos")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersNoAdmin()
        {
            return await _context.users.Where(u => u.isAdmin == false && u.isDeleted == false).ToListAsync();
        }

    }
}
