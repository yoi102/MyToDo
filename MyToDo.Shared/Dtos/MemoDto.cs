namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 备忘录
    /// </summary>
    public class MemoDto : BaseDto
    {




        private string _Title;
        private string _Content;

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





    }
}
