using MyToDo.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Service;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
    /// <summary>
    /// 待办事项控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService service;

        public ToDoController(IToDoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id)
        {

            return await service.GetSingleAsync(id);
        }

        [HttpGet]
        public async Task<ApiResponse> GetAll()
        {

            return await service.GetAllAsync();
        }
        [HttpGet]
        public async Task<ApiResponse> Summary()
        {

            return await service.Summary();
        }
        [HttpGet]
        public async Task<ApiResponse> GetSearch([FromQuery] ToDoParameter param)
        {

            return await service.GetSearchAsync(param);
        }


        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto model)
        {

            return await service.AddAsync(model);
        }
        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ToDoDto model)
        {

            return await service.UpdateAsync(model);
        }
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id)
        {

            return await service.DeleteAsync(id);
        }











    }
}
