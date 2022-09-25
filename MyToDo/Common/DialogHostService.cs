using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.Common
{
    /// <summary>
    /// 对话主机服务（自定义）
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();

            //从容器当中取出弹出窗口的实例 View
            var content = containerExtension.Resolve<object>(name);
            //验证实例的有效性
            if (content is not FrameworkElement dialogContent)
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            //View和ViewModel绑定上下文
            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }

            if (dialogContent.DataContext is not IDialogHostAware viewModel)
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            viewModel.DialogHostName = dialogHostName;

            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpend(parameters);
                }

                eventArgs.Session.UpdateContent(content);
            };

            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }
    }
}