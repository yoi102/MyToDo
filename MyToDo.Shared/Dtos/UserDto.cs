namespace MyToDo.Shared.Dtos
{
	public class UserDto : BaseDto
	{

		private string? _UserName = "";
		private string? _Account = "";
		private string? _Password = "";

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




	}


}
