using MyToDo.Api.Context.UnitOfWork;

namespace MyToDo.Api.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
