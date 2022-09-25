using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class MemoService : BaseService<MemoDto>, IMemoService
    {
        private readonly HttpRestClient client;

        public MemoService(HttpRestClient client) : base(client, "Memo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<List<MemoDto>>> GetSearchAsync(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/Memo/GetSearch?Search={parameter.Search}";
            return await client.ExecuteAsync<List<MemoDto>>(request);
        }
    }
}