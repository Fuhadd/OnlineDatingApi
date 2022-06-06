using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Models;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Core.Services.Authentication;
using OnlineDatingApi.Core.Services.RefreshTokenServices;
using OnlineDatingApi.Core.Services.UserClaims;
using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;
using static OnlineDatingApi.Core.DTOs.AuthDTO;

namespace OnlineDatingApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IRefreshToken _refreshToken;
        private readonly IUnitofWork _unitofWork;
        private readonly IUserClaims _userClaims;

        public AccountController(UserManager<ApiUser> userManager,
                                IMapper mapper,
                                IAuthentication authentication,
                                IRefreshToken refreshToken,
                                IUnitofWork unitofWork,
                                IUserClaims userClaims)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authentication = authentication;
            _refreshToken = refreshToken;
            _unitofWork = unitofWork;
            _userClaims = userClaims;
        }


        [HttpPost]
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


            //var usertest = new LoginUserDTO()
            //{
            //    Email = userDTO.Email,
            //    Password = userDTO.Password,
            //};

            //await _authentication.AuthenticateAsync(usertest);
            var jwtToken = await _authentication.CreateToken(user);

            return Accepted(jwtToken);
            //return Accepted(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authentication.AuthenticateAsync(user))
            {
                return Unauthorized();

            }
            ApiUser _user = await _userManager.FindByEmailAsync(user.Email);
            var jwtToken = await _authentication.CreateToken(_user);

            return Accepted(jwtToken);
        }


        [HttpGet]
        [Route("email")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            UserDTO user1 = _mapper.Map<UserDTO>(user);
            Console.WriteLine(user.Age);
            return Ok(user1);

        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUser()
        {
            var user = _userManager.Users.ToList();
            var user1 = _mapper.Map<IList<UserDTO>>(user);

            return Ok(user1);

        }

        [HttpGet]
        [Route("all+details")]
        public async Task<IActionResult> GetAllUserWithAllImagesandIntersts()
        {
            List<CustomUserResult> primes = new List<CustomUserResult>();
            var users = _userManager.Users.ToList();
            var userResult = _mapper.Map<IList<UserDTO>>(users);
            foreach (var user in userResult)
            {

                var currentUser = user;
                var images = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == user.Id);
                var imagesResults = _mapper.Map<IList<CustomImagesUrlDTO>>(images);
                var interests = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == user.Id);
                var interestResults = _mapper.Map<IList<CustomInterestDTO>>(interests);
                var endResult = new CustomUserResult()
                {
                    ApiUser = user,
                    ImagesUrl = imagesResults,
                    Interests = interestResults,
                };
                primes.Add(endResult);
            }


            return Ok(primes);

        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
        {
            Console.WriteLine("going4");
            if (!ModelState.IsValid)
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>()
                    {
                        "Invalid Input Parameters. Please Try Again"
                    }
                });
            Console.WriteLine("going5");

            return Ok(await _refreshToken.VerifyAndGenerateRefreshTokenAsync(tokenRequest));

        }

        //[HttpGet]
        //[Route("all+details")]
        //public async Task<IActionResult> GetFilteredUsers()
        //{
        //    List<CustomUserResult> primes = new List<CustomUserResult>();
        //    var users = _userManager.Users.ToList();
        //    var userResult = _mapper.Map<IList<UserDTO>>(users);
        //    foreach (var user in userResult)
        //    {

        //        var currentUser = user;
        //        var images = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == user.Id);
        //        var imagesResults = _mapper.Map<IList<CustomImagesUrlDTO>>(images);

        //        var interests = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == user.Id);
        //        var interestResults = _mapper.Map<IList<CustomInterestDTO>>(interests);


        //        var endResult = new CustomUserResult()
        //        {
        //            ApiUser = user,
        //            ImagesUrl = imagesResults,
        //            Interests = interestResults,
        //        };
        //        primes.Add(endResult);
        //    }


        //    return Ok(primes);

        //}

        [HttpGet]
        [Route("all+details/id")]
        [Authorize]
        public async Task<IActionResult> GetAllUsersMinusCurrent()
        {
            string id = _userClaims.GetMyId();
            List<CustomUserResult> primes = new List<CustomUserResult>();
            var users = await _unitofWork.ApiUsers.GetAll(expression: o => o.Id != id);
            var userResult = _mapper.Map<IList<UserDTO>>(users);
            var currentUserResult = _userManager.FindByIdAsync(id);
            var currentUser = currentUserResult.Result;
            var currentUserInterests = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == id);

            foreach (var user in userResult)
            {
                
                int interestindex = 0;
                int currentInterestIndex = 0;
                float matches = 0;

                var thisUser = user;
                var images = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == user.Id);
                var imagesResults = _mapper.Map<IList<CustomImagesUrlDTO>>(images);

                var interests = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == user.Id);
                var interestResults = _mapper.Map<IList<CustomInterestDTO>>(interests);


                foreach (var interest in interestResults)
                {
                    foreach (var currentUserInterest in currentUserInterests)
                    {
                        if (matches > 5)
                            matches = 5;
                        if (currentUserInterest.Interest == interest.Interest)
                        {
                            matches++;
                        }
                        currentInterestIndex++;

                    }
                    interestindex++;


                }
                matches = (matches / 5f) * 10;

                Console.WriteLine(matches);
                Console.WriteLine(1 / 5);

                if (user.Gender != currentUser.Gender)
                    matches += 10;


                if (Enumerable.Range((currentUser.Age) - 5, (currentUser.Age) + 5).Contains(user.Age))
                    matches += 10;


                if (user.LookingFor == currentUser.LookingFor)
                    matches += 10;
                int totalMatches = (Convert.ToInt32(matches)) + 70;

                var endResult = new CustomUserResult()
                {
                    ApiUser = user,
                    ImagesUrl = imagesResults,
                    Interests = interestResults,
                    matches = totalMatches >= 100 ? 98 : totalMatches
                };
                primes.Add(endResult);
            }


            return Ok(primes);

        }

        //[HttpGet]
        //[Route("/{id}")]
        //public async Task<IActionResult> GetImages(string id)
        //{
        //    ApiUser user = await _userManager.FindByIdAsync(id);
        //    UserDTO user1 = _mapper.Map<UserDTO>(user);
        //    Console.WriteLine(user.Age);
        //    return Ok(user1);

        //}





    }
}
