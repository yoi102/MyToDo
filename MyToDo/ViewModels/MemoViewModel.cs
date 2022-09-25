using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Linq;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {
        public MemoViewModel(IMemoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            this.service = service;
            dialogHost = containerProvider.Resolve<IDialogHostService>();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            CreateMemoList();
        }

        private ObservableCollection<MemoDto> _MemoDtos;
        private MemoDto _CurrentMemo;
        private bool _IsRightDrawerOpen;
        private string _Search;
        private readonly IMemoService service;
        private readonly IDialogHostService dialogHost;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return _MemoDtos; }
            set { _MemoDtos = value; RaisePropertyChanged(); }
        }

        public MemoDto CurrentMemo
        {
            get { return _CurrentMemo; }
            set { _CurrentMemo = value; RaisePropertyChanged(); }
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

        private async void CreateMemoList()
        {
            UpdateLoading(true);

            var memoResult = await service.GetAllAsync();

            if (memoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memoResult.Result)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }

        /// <summary>
        /// 执行命令
        /// </summary>

        public DelegateCommand<string> ExecuteCommand => new DelegateCommand<string>((o) =>
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
            if (string.IsNullOrWhiteSpace(CurrentMemo.Title) || string.IsNullOrWhiteSpace(CurrentMemo.Content))
            {
                return;
            }

            UpdateLoading(true);
            try
            {
                if (CurrentMemo.Id > 0)//更新
                {
                    var updateResult = await service.UpdateAsync(CurrentMemo);
                    if (updateResult.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentMemo.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentMemo.Title;
                            todo.Content = CurrentMemo.Content;
                        }
                        IsRightDrawerOpen = false;
                        aggregator.SendMessage("已更新备忘录！");
                    }
                }
                else//新增
                {
                    var addResult = await service.AddAsync(CurrentMemo);

                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                        aggregator.SendMessage("已添加备忘录！");
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

        private void Add()
        {
            IsRightDrawerOpen = true;
            CurrentMemo = new MemoDto();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        private async void Query()
        {
            UpdateLoading(true);

            var todoResult = await service.GetSearchAsync(new QueryParameter() { Search = Search });

            if (todoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in todoResult.Result)
                {
                    MemoDtos.Add(item);
                }
                aggregator.SendMessage("已完成搜索！");
            }

            UpdateLoading(false);
        }

        /// <summary>
        /// 选中对象
        /// </summary>
        public DelegateCommand<MemoDto> SelectedCommand => new DelegateCommand<MemoDto>(async (o) =>
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(o.Id);
                if (todoResult.Status)
                {
                    CurrentMemo = todoResult.Result;
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

        public DelegateCommand<MemoDto> DeleteCommand => new DelegateCommand<MemoDto>(async (obj) =>
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除备忘录：{obj.Title}？");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
            UpdateLoading(true);

            var deleteResult = await service.DeleteAsync(obj.Id);

            if (deleteResult.Status)
            {
                var model = MemoDtos.FirstOrDefault(t => t.Id == obj.Id);

                if (model != null)
                {
                    MemoDtos.Remove(model);

                    aggregator.SendMessage("已完成删除！");
                }
            }
            UpdateLoading(false);
        });
    }
}