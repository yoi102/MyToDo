using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
                password = password.GetMD5();
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(
                      predicate: x => x.Account.Equals(account) && x.Password.Equals(password));

                if (model == null)
                {
                    return new ApiResponse("账号或密码错误,请重试！");
                }
                return new ApiResponse(true, new UserDto()
                {
                    Account = model.Account,
                    UserName = model.UserName,
                    Id = model.Id
                });


            }
            catch
            {
                return new ApiResponse(false, "登录失败");
            }



        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {


            try
            {
                var model = mapper.Map<User>(user);
                var repository = work.GetRepository<User>();

                var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));

                if (userModel != null)
                    return new ApiResponse($"当前账号：{model.Account} 已经存在，请重新注册");

                model.CreateDate = DateTime.Now;
                model.Password = model.Password.GetMD5();
                await repository.InsertAsync(model);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);


                return new ApiResponse("注册失败，请重试！");


            }
            catch
            {
                return new ApiResponse("注册失败");



            }

        }
    }
}
