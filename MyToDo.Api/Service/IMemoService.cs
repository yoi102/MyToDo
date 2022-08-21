

using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public interface IMemoService : IBaseService<MemoDto>
    {

        Task<ApiResponse> GetSearchAsync(QueryParameter model);



    }
}
