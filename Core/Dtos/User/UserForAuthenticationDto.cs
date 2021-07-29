using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.User
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }

        public bool isAuthenticationSuccesfull { get; set; }
    }

}
