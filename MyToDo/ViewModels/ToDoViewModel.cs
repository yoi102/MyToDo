global using Prism.Mvvm;
global using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {

        public ToDoViewModel(IToDoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            this.service = service;
            ToDoDtos = new ObservableCollection<ToDoDto>();
            dialogHost = containerProvider.Resolve<IDialogHostService>();


        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
            {
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
                Search = "";
                Query();
            }
            else
            {
                SelectedIndex = 0;
                GetDataAsync();

            }


        }

        private ObservableCollection<ToDoDto> _ToDoDtos;//仅从webapi中获取搜索的需要的，如需较少搜索时间，还可回去api中的全部数据，再当前数据中再筛选
        private ToDoDto _CurrentToDo;
        private string _Search;
        private readonly IToDoService service;
        private readonly IDialogHostService dialogHost;
        private bool _IsRightDrawerOpen;
        private DelegateCommand<string> _ExecuteCommand;
        private int _SelectedIndex;


        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 总Item
        /// </summary>
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return _ToDoDtos; }
            set { _ToDoDtos = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 当前选择的Item
        /// </summary>
        public ToDoDto CurrentToDo
        {
            get { return _CurrentToDo; }
            set { _CurrentToDo = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return _Search; }
            set { _Search = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 右侧展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { _IsRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);

            var todoResult = await service.GetAllAsync();

            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result)
                {
                    ToDoDtos.Add(item);

                }
            }


            UpdateLoading(false);



        }











        /// <summary>
        /// 执行命令
        /// </summary>

        public DelegateCommand<string> ExecuteCommand => _ExecuteCommand ??= new DelegateCommand<string>((o) =>
        {

            switch (o)
            {
                case "新增": Add(); break;
                case "查询": Query(); break;
                case "保存": Save(); break;
            }




        });



        private async void Save()
        {


            if (string.IsNullOrWhiteSpace(CurrentToDo.Title) || string.IsNullOrWhiteSpace(CurrentToDo.Content))
            {

                return;
            }


            UpdateLoading(true);

            try
            {

                if (CurrentToDo.Id > 0)//更新
                {
                    var updateResult = await service.UpdateAsync(CurrentToDo);
                    if (updateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentToDo.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentToDo.Title;
                            todo.Content = CurrentToDo.Content;
                            todo.Status = CurrentToDo.Status;

                        }
                        IsRightDrawerOpen = false;
                        aggregator.SendMessage("已更新待办事项！");

                    }
                }
                else//新增
                {
                    var addResult = await service.AddAsync(CurrentToDo);

                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                        aggregator.SendMessage("已添加待办事项！");

                    }
                }


            }
            catch
            {

            }
            finally
            {

                UpdateLoading(false);

            }


        }

        void Add()
        {
            IsRightDrawerOpen = true;
            CurrentToDo = new ToDoDto();


        }
        /// <summary>
        /// 搜索
        /// </summary>
        async void Query()
        {
            UpdateLoading(true);

            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await service.GetSearchAsync(new ToDoParameter() { Search = Search, Status = Status });

            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result)
                {
                    ToDoDtos.Add(item);

                }
                aggregator.SendMessage("已完成搜索！");

            }


            UpdateLoading(false);
        }



        /// <summary>
        /// 选中对象
        /// </summary>
        public DelegateCommand<ToDoDto> SelectedCommand => new DelegateCommand<ToDoDto>(async (o) =>
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(o.Id);
                if (todoResult.Status)
                {
                    CurrentToDo = todoResult.Result;
                    IsRightDrawerOpen = true;

                }

            }
            catch
            {

            }
            finally
            {
                UpdateLoading(false);

            }

        });


        public DelegateCommand<ToDoDto> DeleteCommand => new DelegateCommand<ToDoDto>(async (obj) =>
        {


            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项：{obj.Title}？");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

            UpdateLoading(true);


            var deleteResult = await service.DeleteAsync(obj.Id);

            if (deleteResult.Status)
            {
                var model = ToDoDtos.FirstOrDefault(t => t.Id == obj.Id);

                if (model != null)
                {
                    ToDoDtos.Remove(model);
                    aggregator.SendMessage("已删除！");
                }

            }
            UpdateLoading(false);

        });

        public DelegateCommand SelectionChangedCommand => new DelegateCommand(() =>
        {

            Query();

        });









    }
}
