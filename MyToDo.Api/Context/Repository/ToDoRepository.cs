using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;

namespace MyToDo.Api.Context.Repository
{


    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }





}
