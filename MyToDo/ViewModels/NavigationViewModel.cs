using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
