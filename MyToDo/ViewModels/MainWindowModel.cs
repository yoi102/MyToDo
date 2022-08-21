using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Views;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyToDo.ViewModels
{

    public class MainWindowModel : BindableBase, IConfigureService
    {
        public MainWindowModel(IRegionManager regionManager,IContainerProvider container)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            this.container = container;


        }

        private ObservableCollection<MenuBar> _MenuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _MenuBars; }
            set { _MenuBars = value; RaisePropertyChanged(); }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; RaisePropertyChanged(); }
        }







        private readonly IRegionManager regionManager;
        private readonly IContainerProvider container;
        private IRegionNavigationJournal journal;

        private DelegateCommand<MenuBar> _NavigateCommand;
        public DelegateCommand<MenuBar> NavigateCommand => _NavigateCommand ??= new DelegateCommand<MenuBar>((o) =>
        {
            if (o == null || string.IsNullOrWhiteSpace(o.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(o.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;

            });

        });


        private DelegateCommand _GoBackCommand;

        public DelegateCommand GoBackCommand => _GoBackCommand ??= new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoBack)
                journal.GoBack();

        });
        private DelegateCommand _GoForwardCommand;

        public DelegateCommand GoForwardCommand => _GoForwardCommand ??= new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoForward)
                journal.GoForward();


        });    
        public DelegateCommand LoginOutCommand =>  new DelegateCommand(() =>
        {
            App.LoginOut(container);
        });


        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            UserName = AppSession.UserName;

            MenuBars.Add(new MenuBar() { IconKind = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { IconKind = "NotebookOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { IconKind = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { IconKind = "Cog", Title = "设置", NameSpace = "SettingsView" });
            MenuBars.Add(new MenuBar() { IconKind = "Image", Title = "图", NameSpace = "ImageListBoxView" });





            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back =>
            {
                journal = back.Context.NavigationService.Journal;

            });

        }
    }
}
