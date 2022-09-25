using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse<UserDto>> LoginAsync(UserDto dto);

        Task<ApiResponse> RegisterAsync(UserDto dto);
    }
}