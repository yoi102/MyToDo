using MyToDo.Shared.Dtos;
using MyToDo.Api.Context;
using MyToDo.Shared.Parameters;
using MyToDo.Api.Context.UnitOfWork;
using System.Reflection.Metadata;
using System.Collections.ObjectModel;
using AutoMapper;

namespace MyToDo.Api.Service
{
    /// <summary>
    /// 待办事项的实现
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                await work.GetRepository<ToDo>().InsertAsync(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                //var todos = await repository.GetPagedListAsync(predicate:
                //   x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Equals(parameter.Search),
                //   pageIndex: parameter.PageIndex,
                //   pageSize: parameter.PageSize,
                //   orderBy: source => source.OrderByDescending(t => t.CreateDate)
                //   );
                var todos = await repository.GetAllAsync();
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }



        //public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        //{
        //    try
        //    {
        //        var repository = work.GetRepository<ToDo>();
        //        var todos = await repository.GetPagedListAsync(predicate:
        //           x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))
        //           && (parameter.Status == null ? true : x.Status.Equals(parameter.Status)),
        //           pageIndex: parameter.PageIndex,
        //           pageSize: parameter.PageSize,
        //           orderBy: source => source.OrderByDescending(t => t.CreateDate));
        //        return new ApiResponse(true, todos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse(ex.Message);
        //    }
        //}



        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var dbToDo = mapper.Map<ToDo>(model);
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbToDo.Id));
                if (todo != null)
                {
                    todo.Title = dbToDo.Title;
                    todo.Content = dbToDo.Content;
                    todo.Status = dbToDo.Status;
                    todo.UpdateDate = DateTime.Now;
                    repository.Update(todo);

                }
                else
                {
                    await AddAsync(model);

                }



                if (await work.SaveChangesAsync() > 0)

                    return new ApiResponse(true, todo);
                return new ApiResponse("更新数据异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSearchAsync(ToDoParameter parameter)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetAllAsync(predicate:
                   x => (parameter.Search == null || string.IsNullOrWhiteSpace(parameter.Search) || x.Title.Contains(parameter.Search))
                   && (parameter.Status == null || x.Status.Equals(parameter.Status)),
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Summary()
        {


            try
            {
                //待办结果
                var todos = await work.GetRepository<ToDo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));
                //备忘录结果
                var memos = await work.GetRepository<Memo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count;//汇总待办事项数量
                summary.CompletedCount = todos.Where(t => t.Status == 1).Count();//统计完成数量
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");//统计完成率
                summary.MemoCount = memos.Count;//汇总备忘录数量

                summary.TodoList = new ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));



                return new ApiResponse(true, summary);
            }


            catch
            {
                return new ApiResponse(false, "");

            }





        }








        //分页查找。。。。。。。不用
        //public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        //{
        //    try
        //    {
        //        var repository = work.GetRepository<ToDo>();
        //        var todos = await repository.GetPagedListAsync(predicate:
        //           x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Equals(parameter.Search),
        //           pageIndex: parameter.PageIndex,
        //           pageSize: parameter.PageSize,
        //           orderBy: source => source.OrderByDescending(t => t.CreateDate)
        //           );

        //        return new ApiResponse(true, todos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse(ex.Message);
        //    }
        //}







    }
}
