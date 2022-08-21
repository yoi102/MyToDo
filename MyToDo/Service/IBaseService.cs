using MyToDo.Api.Service;
using MyToDo.Shared;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiResponse = MyToDo.Shared.ApiResponse;

namespace MyToDo.Service
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id);

        Task<ApiResponse<List< TEntity>>> GetAllAsync();




    }
}
