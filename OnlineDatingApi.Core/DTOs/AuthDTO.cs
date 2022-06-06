using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.DTOs
{
    public class AuthDTO
    {

        public class LoginUserDTO
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;

        }
    }
}
