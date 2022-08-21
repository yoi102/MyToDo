namespace MyToDo.Api.Context
{
    public class User : BaseEntity
    {


        public string? UserName { get; set; } = "";
        public string? Account { get; set; } = "";
        public string? Password { get; set; } = "";
    }
}
