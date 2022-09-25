using MyToDo.Api.Context.UnitOfWork;

namespace MyToDo.Api.Context.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
