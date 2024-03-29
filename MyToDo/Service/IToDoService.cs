﻿using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        public Task<ApiResponse<List<ToDoDto>>> GetSearchAsync(ToDoParameter parameter);

        public Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}