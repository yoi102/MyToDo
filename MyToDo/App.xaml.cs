using DryIoc;
using Hardware.Info;
using MyToDo.Common;
using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.ViewModels.Dialogs;
using MyToDo.Views;
using MyToDo.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
            //return Container.Resolve<MainWindow>();
        }

        public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();

            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                Current.MainWindow.Show();
            });
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                    service.Configure();
                base.OnInitialized();
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowModel>();
            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<ImageListBoxView, ImageListBoxViewModel>();
            containerRegistry.RegisterForNavigation<PCInforView, PCInforViewModel>();
            containerRegistry.RegisterForNavigation<MemoryInforView, MemoryInforViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();

            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();//有三种注册方式
            containerRegistry.Register<ILoginService, LoginService>();//每一次解析都会创建一个实例，仅限当前实例，同一个实例相当于同一个请求
            containerRegistry.RegisterScoped<IHardwareInfo, HardwareInfo>();//在同一个Scope内只初始化一个实例，多个view中同一个范围？，
            containerRegistry.RegisterSingleton<ITestService, TestService>();//单例
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));

            containerRegistry.GetContainer().RegisterInstance(@"https://localhost:7211/", serviceKey: "webUrl");
        }

        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //    //初始化软件用。。。。。
        //    var service = App.Current.MainWindow.DataContext as IConfigureService;
        //    if (service != null)
        //        service.Configure();

        //}
    }
}