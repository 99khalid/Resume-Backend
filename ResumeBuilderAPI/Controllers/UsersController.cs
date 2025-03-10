using Application.Features.UserFeatures.Command;
using Infrastructure.Interfaces;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResumeBuilderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;

        public UsersController(AppDBContext context, IJwtService jwtService, IMediator mediator)
        {
            _context = context;
            _jwtService = jwtService;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticateUser(UserLoggingDto request)
        {
            

            var user = _context.Users.SingleOrDefault(u => u.UserName == request.UserName);
            if (user == null ||user.PasswordHash== BCrypt.Net.BCrypt.HashPassword(request.Password))
            {
                return Unauthorized("Invalid username or password");
            }
            var token = _jwtService.GenerateToken(user);
            return Ok(new LoginResponse
            {
                Token = token,
                UserName = user.UserName,
                UserRole = user.UserRole.ToString()
            });                      
        }

       
    }
}
