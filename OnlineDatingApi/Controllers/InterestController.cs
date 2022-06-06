using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Core.Services.UserClaims;
using OnlineDatingApi.Data;

namespace OnlineDatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class InterestController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IUserClaims _userClaims;

        public InterestController(IUnitofWork unitofWork, IMapper mapper, IUserClaims userClaims)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _userClaims = userClaims;
        }

        [HttpGet]
        public async Task<IActionResult> GetInterest()
        {
            var interest = await _unitofWork.UserInterests.GetAll();
            var results = _mapper.Map<IList<InterestDTO>>(interest);
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInterest([FromBody] CreateInterestDTO imagesDTO)
        {
            
            imagesDTO.ApiUserId = _userClaims.GetMyId();
            var interest = _mapper.Map<UserInterests>(imagesDTO);
            interest.Id = Guid.NewGuid();
            await _unitofWork.UserInterests.Create(interest);
            await _unitofWork.Save();

            return Ok(interest);
        }

        [HttpGet]
        [Route("user+id")]
        public async Task<IActionResult> GetAllInterestByASingleUser()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == id, includes: new List<string> { "ApiUser" });
            var results = _mapper.Map<IList<InterestDTO>>(users);
            return Ok(results);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetInterestById()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == id);
            var results = _mapper.Map<IList<CreateInterestDTO>>(users);
            return Ok(results);
        }

        [HttpGet]
        [Route("list/id")]
        public async Task<IActionResult> GetInterestByIdAsList()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == id);
            List<string> interests = new List<string>();
            foreach (var user in users)
            {
                interests.Add(user.Interest!);
            }
            
           
            return Ok(interests);
        }

        //[HttpGet]
        //[Route("ordered/{id}")]
        //public async Task<IActionResult> GetOrderedImagesById(string id)
        //{
        //    var users = await _unitofWork.UserInterests.GetAll(expression: o => o.ApiUserId == id, orderBy: s => s.OrderByDescending(o => o.CreatedAt), limit: 4);
        //    var results = _mapper.Map<IList<CreateInterestDTO>>(users);
        //    return Ok(results);
        //}


    }
}
