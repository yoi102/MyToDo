﻿using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IMemoService : IBaseService<MemoDto>
    {
        public Task<ApiResponse<List<MemoDto>>> GetSearchAsync(QueryParameter parameter);
    }
}