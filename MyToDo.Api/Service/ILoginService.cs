using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public interface ILoginService
    {

        Task<ApiResponse> LoginAsync(string account, string password);
        Task<ApiResponse> Resgiter(UserDto user);

    }
}
