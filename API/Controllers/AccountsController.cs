using API.Exceptions;
using AutoMapper;
using Core.Dtos.User;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IMailService _mailService;
        public AccountsController(UserManager<User> userManager, IMapper mapper,  JwtHandler jwtHandler, IMailService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _mailService = mailService;
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null)
                return BadRequest(new APIResponse(400, "Bad Request Has Been Made"));


            var emailExist = await _userManager.FindByEmailAsync(userForRegistration.Email);
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
    {
        {"token", token },
        {"email", user.Email }
    };
            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
            var message = new MailRequest { ToEmail = user.Email, Subject = "Email Confirmation Token", Body = callback };
            await _mailService.SendEmailAsync(message);
            await _userManager.AddToRoleAsync(user, "Customer");
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            if(user == null)
            {
                return Unauthorized(new APIResponse(401, "Wrong Email or Password"));
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)){
                return Unauthorized(new APIResponse(401, "Email Hasnot Been Confirmed"));
            }
                var checkPassword = await _userManager.CheckPasswordAsync(user, userForAuthentication.Password);
            
                if (!checkPassword)
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $"Your account is locked out. To reset the password click this link: {userForAuthentication.clientURI}";
                    var message = new MailRequest { ToEmail = user.Email, Subject = "Account Loclout", Body = content };
                    await _mailService.SendEmailAsync(message);

                    return Unauthorized(new APIResponse(401, "Your email has been locked out. Check you email"));
                }
                return Unauthorized(new APIResponse(401, "Wrong Email or Password"));
            }
                    
                var signingCredentials = _jwtHandler.GetSigningCredentials();
                var claims =await _jwtHandler.GetClaims(user);
                var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var response = new LoginResponse
                {
                    Token = token,
                    isAuthenticationSuccesfull = true

                };
                return Ok(response);
            }

        [HttpPost("ForgotPassword")]
      
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
          
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest(new APIResponse(400, "The email doesnot exist"));
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
    {
        {"token", token },
        {"email", forgotPasswordDto.Email }
    };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var message = new MailRequest { ToEmail =user.Email, Subject="Reset Password Token", Body=callback};
            await _mailService.SendEmailAsync(message);
            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return new BadRequestObjectResult(new APIValidationErrorResponse {Errors=errors });
            }
            return Ok();
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");
            return Ok();
        }
    }
    }

