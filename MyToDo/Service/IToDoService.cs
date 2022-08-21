using MyToDo.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDo.Shared;

namespace MyToDo.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {

        public Task<ApiResponse<List<ToDoDto>>> GetSearchAsync(ToDoParameter parameter);
        public Task<ApiResponse<SummaryDto>> SummaryAsync();


    }
}
