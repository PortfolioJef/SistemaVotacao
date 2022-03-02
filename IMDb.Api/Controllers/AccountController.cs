using IMDb.Api;
using IMDbApi.Context;
using IMDbApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IMDbApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly dbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string keyPass;


        public AccountController(IConfiguration configuration, dbContext context)
        {
            _configuration = configuration;
            _context = context;
            keyPass = _configuration["CtsAcs:scKey"];
        }
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserDto userDto)
        {             
            var loginDb = await _context.users
                               .Where(L => L.Name == userDto.Login.Trim() && L.Password == Util.Encrypt(userDto.Password.Trim(), keyPass))
                               .FirstOrDefaultAsync();


            if (loginDb == null)
                return NotFound("login inválido.");

            if (loginDb != null)
            {
                return BuildToken(loginDb.Id, loginDb.Name, loginDb.isAdmin);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login inválido.");
                return BadRequest(ModelState);
            }
        }

        [NonAction]
        private UserToken BuildToken(int Id, string Name, bool isAdmin)
        {
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.UniqueName, Id.ToString()),
                new Claim("idUser",Id.ToString()),
                new Claim("Name",Name),
                new Claim("IsAdmin","IsAdmin"),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyPass));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddDays(7);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,               
               signingCredentials: creds);
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }


    }
}