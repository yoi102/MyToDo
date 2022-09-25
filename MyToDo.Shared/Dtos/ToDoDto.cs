namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 待办事项数据实体
    /// </summary>
    public class ToDoDto : BaseDto
    {





        private string _Title;
        private string _Content;
        private int _Status;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get { return _Content; }
            set { _Content = value; OnPropertyChanged(); }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }




    }
}
