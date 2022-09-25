using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly HttpRestClient client;

        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<List<ToDoDto>>> GetSearchAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/ToDo/GetSearch?status={parameter.Status} " +
                $"&search={parameter.Search}";
            return await client.ExecuteAsync<List<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/ToDo/Summary";
            return await client.ExecuteAsync<SummaryDto>(request);
        }
    }
}