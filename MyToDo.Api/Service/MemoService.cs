using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    /// <summary>
    /// 备忘录事项的实现
    /// </summary>
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
           
        }


        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = mapper.Map<Memo>(model);
                await work.GetRepository<Memo>().InsertAsync(memo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, memo);
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
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(memo);

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
                var repository = work.GetRepository<Memo>();
                //var todos = await repository.GetPagedListAsync(predicate:
                //   x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                //   pageIndex: parameter.PageIndex,
                //   pageSize: parameter.PageSize,
                //   orderBy: source => source.OrderByDescending(t => t.CreateDate));


                var todos = await repository.GetAllAsync();

                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSearchAsync(QueryParameter parameter)
        {

            try
            {
                var repository = work.GetRepository<Memo>();
                var todos = await repository.GetAllAsync(predicate:
                   x => string.IsNullOrWhiteSpace(parameter.Search) || x.Title.Contains(parameter.Search) || x.Content.Contains(parameter.Search),
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));

                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }


        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                return new ApiResponse(true, memo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var dbMemo = mapper.Map<Memo>(model);
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbMemo.Id));
                memo.Title = dbMemo.Title;
                memo.Content = dbMemo.Content;
                memo.UpdateDate = DateTime.Now;
                repository.Update(memo);


                if (await work.SaveChangesAsync() > 0)

                    return new ApiResponse(true, memo);
                return new ApiResponse("更新数据异常");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
