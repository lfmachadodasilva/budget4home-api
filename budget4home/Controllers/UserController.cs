using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Models.Dtos;
using budget4home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
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
        public async Task<IActionResult> Get()
        {
            var models = await _userService.GetAllAsync();
            var dtos = _mapper.Map<ICollection<UserDto>>(models);
            return Ok(dtos);
        }
    }
}