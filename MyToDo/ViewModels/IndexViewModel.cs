using ImTools;
using MyToDo.Api.Context;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {


        public IndexViewModel(IDialogHostService dialog, IContainerProvider provider) : base(provider)
        {



            DispatcherTimer timer = new DispatcherTimer();
            //设置每隔一秒触发
            //TimeSpan--时间间隔
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (o, e) =>
            {

                NowTime = DateTime.Now.ToString("yyyy 年 MM 月 dd 日 dddd   HH:mm:ss");
            };
            timer.Start();
            this.dialog = dialog;
            this.toDoService = provider.Resolve<IToDoService>();
            this.memoServic = provider.Resolve<IMemoService>();
            this.regionManager = provider.Resolve<IRegionManager>();




        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);


            CreateTaskBars();
            GetSummary();
            GetData();

        }


        #region Properties

        private SummaryDto _Summary = new SummaryDto();
        private ObservableCollection<TaskBar> _TaskBars;
        private string _NowTime;
        private readonly IDialogHostService dialog;
        private readonly IToDoService toDoService;
        private readonly IMemoService memoServic;
        private readonly IRegionManager regionManager;
        private int _SelectedToDoStatus;

        public SummaryDto Summary
        {
            get { return _Summary; }
            set { _Summary = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedToDoStatus
        {
            get { return _SelectedToDoStatus; }
            set { _SelectedToDoStatus = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return _TaskBars; }
            set { _TaskBars = value; RaisePropertyChanged(); }
        }





        public string NowTime
        {


            get { return _NowTime; }
            set
            {
                _NowTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand ToDoSelectionChangedCommand => new DelegateCommand(() =>
        {

            GetToDoAsync();

        });



        /// <summary>
        /// ToDo选中对象
        /// </summary>
        public DelegateCommand<ToDoDto> ToDoEditCommand => new DelegateCommand<ToDoDto>((o) =>
        {
            AddToDo(o);

        });

        public DelegateCommand<ToDoDto> ToDoCompltedCommand => new DelegateCommand<ToDoDto>(async (o) =>
        {
            try
            {
                UpdateLoading(true);


                var updateResult = await toDoService.UpdateAsync(o);

                if (updateResult.Status)
                {
                    var todo = Summary.TodoList.FirstOrDefault(t => t.Id.Equals(o.Id));

                    if (todo != null)
                    {
                        var status = SelectedToDoStatus - 1;

                        if (status != -1)
                        {

                            if (todo.Status != status)
                            {
                                Summary.TodoList.Remove(todo);

                            }


                        }
                        aggregator.SendMessage("已修改状态！");

                    }
                    GetSummary();
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
        /// <summary>
        /// Memo选中对象
        /// </summary>
        public DelegateCommand<MemoDto> MemoEditCommand => new DelegateCommand<MemoDto>((o) =>
        {

            AddMemo(o);

        });

        public DelegateCommand<TaskBar> NavigateCommand => new DelegateCommand<TaskBar>((o) =>
        {

            if (string.IsNullOrWhiteSpace(o.Target)) return;

            NavigationParameters param = new NavigationParameters();

            if (o.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(o.Target, param);
        });






        /// <summary>
        /// 执行命令
        /// </summary>

        public DelegateCommand<string> ExecuteCommand => new DelegateCommand<string>((o) =>
        {

            switch (o)
            {
                case "新增待办": AddToDo(null); break;
                case "新增备忘录": AddMemo(null); break;
            }


        });


        async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Model", model);

            var dialogResult = await dialog.ShowDialog("AddMemoView", param);//导航到App注册的会话
            if (dialogResult.Result == ButtonResult.OK)
            {

                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Model");
                    if (memo.Id > 0)//更新
                    {

                        var UpdateResult = await memoServic.UpdateAsync(memo);
                        if (UpdateResult.Status)
                        {
                            var currentMemo = Summary.MemoList.FirstOrDefault(t => t.Id == memo.Id);

                            if (currentMemo != null)
                            {
                                currentMemo.Title = memo.Title;
                                currentMemo.Content = memo.Content;

                            }

                        }
                        aggregator.SendMessage("已更新备忘录！");

                    }
                    else//添加
                    {
                        var addResult = await memoServic.AddAsync(memo);
                        if (addResult.Status)
                        {

                            Summary.MemoList.Add(addResult.Result);
                            aggregator.SendMessage("已添加备忘录！");

                        }
                    }
                    GetSummary();
                }

                finally
                {
                    UpdateLoading(false);
                }


            }


        }

        async void AddToDo(ToDoDto model)
        {

            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Model", model);

            var dialogResult = await dialog.ShowDialog("AddToDoView", param);//导航到App注册的会话
            if (dialogResult != null && dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = dialogResult.Parameters.GetValue<ToDoDto>("Model");
                    if (todo.Id > 0)//更新
                    {

                        var updateResult = await toDoService.UpdateAsync(todo);
                        if (updateResult.Status)
                        {
                            var currentTodo = Summary.TodoList.FirstOrDefault(t => t.Id == todo.Id);
                            if (currentTodo != null)
                            {

                                var status = SelectedToDoStatus - 1;

                                if (status != -1)
                                {
                                    if (currentTodo.Status == status)
                                    {
                                        Summary.TodoList.Remove(currentTodo);
                                    }
                                }
                                else
                                {
                                    currentTodo.Title = todo.Title;
                                    currentTodo.Content = todo.Content;
                                    currentTodo.Status = todo.Status;
                                    currentTodo.Id = todo.Id;
                                }



                            }
                            aggregator.SendMessage("更新加待办事项！");

                        }


                    }
                    else//添加
                    {
                        var addResult = await toDoService.AddAsync(todo);
                        if (addResult.Status)
                        {
                            var status = SelectedToDoStatus - 1;

                            if (status != -1)
                            {
                                if (todo.Status == status)
                                {
                                    Summary.TodoList.Add(addResult.Result);

                                }
                            }
                            else
                            {
                                Summary.TodoList.Add(addResult.Result);

                            }
                            aggregator.SendMessage("已添加待办事项！");

                        }
                    }
                    GetSummary();

                }
                finally
                {
                    UpdateLoading(false);
                }

            }



        }

        #endregion

        #region Methods
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView" });
        }

        async void GetSummary()
        {
            var summaryResut = await toDoService.SummaryAsync();
            if (summaryResut.Status)
            {
                Summary.Sum = summaryResut.Result.Sum;
                Summary.MemoCount = summaryResut.Result.MemoCount;
                Summary.CompletedRatio = summaryResut.Result.CompletedRatio;
                Summary.CompletedCount = summaryResut.Result.CompletedCount;

                Refresh();
            }
        }


        void Refresh()
        {
            TaskBars[0].Content = Summary.Sum.ToString();
            TaskBars[1].Content = Summary.CompletedCount.ToString();
            TaskBars[2].Content = Summary.CompletedRatio;
            TaskBars[3].Content = Summary.MemoCount.ToString();

        }




        /// <summary>
        /// 获取数据
        /// </summary>
        void GetData()
        {

            GetToDoAsync();
            GetMemoAsync();


        }

        async void GetToDoAsync()
        {

            UpdateLoading(true);

            var todoResult = await toDoService.GetAllAsync();

            if (todoResult.Status)
            {
                Summary.TodoList.Clear();

                foreach (var item in todoResult.Result)
                {

                    var status = SelectedToDoStatus - 1;
                    if (status != -1)
                    {
                        if (item.Status == status)
                            Summary.TodoList.Add(item);
                    }
                    else
                    {
                        Summary.TodoList.Add(item);
                    }


                }
            }
            GetSummary();
            UpdateLoading(false);

        }

        async void GetMemoAsync()
        {
            UpdateLoading(true);

            var memoResult = await memoServic.GetAllAsync();


            if (memoResult.Status)
            {
                Summary.MemoList.Clear();
                foreach (var item in memoResult.Result)
                {
                    Summary.MemoList.Add(item);

                }

            }
            GetSummary();
            UpdateLoading(false);




        }


        #endregion



    }




}
