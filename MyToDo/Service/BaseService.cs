using MyToDo.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient client;
        private readonly string serviceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            this.client = client;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Add";
            request.Parameter = entity;

            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Delete;
            request.Route = $"api/{serviceName}/Delete?id={id}";

            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<List<TEntity>>> GetAllAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            //request.Route = $"api/{serviceName}/GetAll?pageIndex={parameter.PageIndex}" +
            //    $"&pageSize={parameter.PageSize}" +
            //    $"&search={parameter.Search}";
            request.Route = $"api/{serviceName}/GetAll";

            return await client.ExecuteAsync<List<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/Get?id={id}";

            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Update";
            request.Parameter = entity;

            return await client.ExecuteAsync<TEntity>(request);
        }
    }
}