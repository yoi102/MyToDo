using MyToDo.ViewModels;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using MyToDo.Views;
using Prism.DryIoc;
using MyToDo.Service;
using DryIoc;
using MyToDo.Common;
using MyToDo.Views.Dialogs;
using MyToDo.ViewModels.Dialogs;

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

            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<ILoginService, LoginService>();
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
