using MyToDo.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Service;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
    /// <summary>
    /// Login控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto param)
        {

            return await service.LoginAsync(param.Account, param.Password);
        }

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto param)
        {

            return await service.Resgiter(param);
        } 












    }
}
