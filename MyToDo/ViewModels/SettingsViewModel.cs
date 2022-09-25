using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBar();
            this.regionManager = regionManager;
        }

        private ObservableCollection<MenuBar> _MenuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _MenuBars; }
            set { _MenuBars = value; RaisePropertyChanged(); }
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { IconKind = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { IconKind = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { IconKind = "InformationOutline", Title = "更多", NameSpace = "AboutView" });
        }

        private readonly IRegionManager regionManager;

        private DelegateCommand<MenuBar> _NavigateCommand;

        public DelegateCommand<MenuBar> NavigateCommand => _NavigateCommand ??= new DelegateCommand<MenuBar>((o) =>
        {
            if (o == null || string.IsNullOrWhiteSpace(o.NameSpace))
                return;
            regionManager.Regions[PrismManager.SettingsRegionName].RequestNavigate(o.NameSpace);
        });
    }
}