namespace MyToDo.Shared.Dtos
{
    public class RegisterUserDto : BaseDto
    {

        private string? _UserName = "";
        private string? _Account = "";
        private string? _Password = "";
        private string? _NewPassword = "";

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }

        public string Account
        {
            get { return _Account; }
            set { _Account = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }
        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; OnPropertyChanged(); }
        }



    }


}
