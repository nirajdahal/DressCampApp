using API.Exceptions;
using AutoMapper;
using Core.Dtos.User;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        public AccountsController(UserManager<User> userManager, IMapper mapper,  JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null)
                return BadRequest(new APIResponse(400, "Bad Request Has Been Made"));
            var emailExist = _userManager.FindByEmailAsync(userForRegistration.Email);
            if (emailExist !=null)
            {
                return new BadRequestObjectResult(new APIValidationErrorResponse { Errors = new[] { "Email address is in use" } });
            }
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(errors);
            }

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
                var checkPassword = await _userManager.CheckPasswordAsync(user, userForAuthentication.Password);
            
                if (user == null || !checkPassword)
                    return Unauthorized(new APIResponse(401, "Wrong Email or Password"));
                var signingCredentials = _jwtHandler.GetSigningCredentials();
                var claims = _jwtHandler.GetClaims(user);
                var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var response = new LoginResponse
                {
                    Token = token,
                    isAuthenticationSuccesfull = true
                };
                return Ok(response);
            }

         
        }
    }

