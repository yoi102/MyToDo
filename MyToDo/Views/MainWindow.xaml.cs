using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extensions;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MyToDo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDialogHostService dialogHostService;

        public MainWindow(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();

        

            //注册等待消息窗口
            aggregator.Register(arg =>
            {

                DialogHost.IsOpen = arg.IsOpen;
                if (DialogHost.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();


                }

            });
            //注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                string message = "error";
                if (arg.Message != null)
                    message = arg.Message;
                snackbar.MessageQueue.Enqueue(message);
            },"Main");

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {

                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    btnMax.Content = new PackIcon() { Kind = PackIconKind.WindowMaximize };

                }
                else
                {

                    this.WindowState = WindowState.Maximized;
                    btnMax.Content = new PackIcon() { Kind = PackIconKind.DockWindow };

                }

                //this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;



            };
            btnClose.Click += async (s, e) =>
            {

                var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统？");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                this.Close();



            };
            borderZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();

            };

            int i = 0;
            borderZone.MouseDown += (s, e) =>
            {

                i += 1;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                timer.Tick += (sender1, e1) => { timer.IsEnabled = false; i = 0; };
                timer.IsEnabled = true;
                if (i == 2)  //2击就是2下， 
                {
                    i = 0;
                    this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                }
            };
            menuBar.SelectionChanged += (s, e) =>
            {
                drawewHost.IsLeftDrawerOpen = false;
            };
            this.dialogHostService = dialogHostService;
        }
    }
}
