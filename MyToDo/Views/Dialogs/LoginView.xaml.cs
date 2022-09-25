using MyToDo.Extensions;
using Prism.Events;
using System.Windows.Controls;

namespace MyToDo.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.RegisterMessage(arg =>
            {
                string message = "error";
                if (arg.Message != null)
                    message = arg.Message;
                LoginSnackBar.MessageQueue.Enqueue(message);
            }, "Login");
        }
    }
}