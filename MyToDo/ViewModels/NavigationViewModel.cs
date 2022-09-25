using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void UpdateLoading(bool IsOpen)
        {
            aggregator.UpdataLoading(new Common.Events.UpdateModel()
            {
                IsOpen = IsOpen
            });
        }
    }
}