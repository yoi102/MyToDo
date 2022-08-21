using MyToDo.Shared.Dtos;
using MyToDo.Api.Context;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task<ApiResponse> GetSearchAsync(ToDoParameter model);
        Task<ApiResponse> Summary();













    }
}
