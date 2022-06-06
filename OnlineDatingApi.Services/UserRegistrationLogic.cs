

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineDatingApi.Core.Services.Authentication;
using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;
using System.Web.Mvc;
using static OnlineDatingApi.Core.DTOs.AuthDTO;

namespace OnlineDatingApi.Services
{
    public class UserRegistrationLogic
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;

        public UserRegistrationLogic(UserManager<ApiUser> userManager, IMapper mapper, IAuthentication authentication)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authentication = authentication;
        }

        public async Task<IActionResult> Register([FromBody] CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = _mapper.Map<ApiUser>(userDTO);

            user.UserName = user.Id;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = result.Errors.Select(error => error.Description).ToList()
                });

            }


            var usertest = new LoginUserDTO()
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
            };

            await _authentication.AuthenticateAsync(usertest);

            return Accepted(new RegistrationResponse()
            {
                Token = await _authentication.CreateToken(),
                Success = true,

            }
               );
            //return Accepted(user);
        }
    }
}
