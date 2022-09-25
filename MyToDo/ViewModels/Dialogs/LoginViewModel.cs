using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;

namespace MyToDo.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService service, IEventAggregator aggregator)
        {
            this.service = service;
            this.aggregator = aggregator;
        }

        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            Register();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        private string _Account;
        private string _Password;
        private readonly ILoginService service;
        private readonly IEventAggregator aggregator;
        private int _SelectedIndex;
        private RegisterUserDto _UserDto = new RegisterUserDto();

        public string Account
        {
            get { return _Account; }
            set { _Account = value; RaisePropertyChanged(); }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; RaisePropertyChanged(); }
        }

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); }
        }

        public RegisterUserDto UserDto
        {
            get { return _UserDto; }
            set { _UserDto = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExecuteCommand => new DelegateCommand<string>((arg) =>
        {
            switch (arg)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "GoRegister": GoRegister(); break;//
                case "Register": Register(); break;
                case "Return": Return(); break;
                case "DirectLogin": DirectLogin(); break;
            }
        });

        private void DirectLogin()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Account) ||
                          string.IsNullOrWhiteSpace(UserDto.UserName) ||
                          string.IsNullOrWhiteSpace(UserDto.Password) ||
                          string.IsNullOrWhiteSpace(UserDto.NewPassword))
            {
                aggregator.SendMessage("请输入完整的注册信息！", "Login");
                return;
            }
            if (UserDto.Password != UserDto.NewPassword)
            {
                aggregator.SendMessage("密码不一致,请重新输入！", "Login");
                return;
            }

            var registerResult = await service.RegisterAsync(new UserDto()
            {
                Account = UserDto.Account,
                UserName = UserDto.UserName,
                Password = UserDto.Password,
            });

            if (registerResult != null && registerResult.Status)
            {
                aggregator.SendMessage("注册成功", "Login");
                //注册成功,返回登录页页面
                SelectedIndex = 0;
            }
            else
                aggregator.SendMessage(registerResult.Message, "Login");
        }

        private void Return()
        {
            SelectedIndex = 0;
        }

        private void GoRegister()
        {
            SelectedIndex = 1;
        }

        private void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Account) ||
              string.IsNullOrWhiteSpace(Password))
            {
                aggregator.SendMessage("请输入账号或密码！", "Login");

                return;
            }
            var loginResult = await service.LoginAsync(new UserDto()
            {
                Account = _Account,
                Password = _Password
            });

            if (loginResult != null && loginResult.Status)
            {
                AppSession.UserName = loginResult.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                //登录失败提示...
                aggregator.SendMessage(loginResult.Message, "Login");
            }
        }
    }
}