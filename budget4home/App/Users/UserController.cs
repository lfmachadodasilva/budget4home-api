using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Users.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.App.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetUserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var models = await _userService.GetAllAsync();
            var response = _mapper.Map<ICollection<GetUserResponse>>(models);
            return Ok(response);
        }
    }
}