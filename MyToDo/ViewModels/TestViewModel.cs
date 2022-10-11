using System;
using System.Windows.Threading;
using DynamicDataDisplay.DataSources;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MyToDo.Common;
using MyToDo.Service;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Commands;
//using Microsoft.WindowsAPICodePack.Dialogs;
namespace MyToDo.ViewModels
{
    public class TestViewModel : BindableBase
    {

        public TestViewModel()
        {



            voltagePointCollection = new JudgePointCollection();

            updateCollectionTimer = new DispatcherTimer();
            updateCollectionTimer.Interval = TimeSpan.FromMilliseconds(200);
            updateCollectionTimer.Tick += new EventHandler(updateCollectionTimer_Tick);
            updateCollectionTimer.Start();



            Data1 = new EnumerableDataSource<JudgePoint>(voltagePointCollection);

            Data1.SetXMapping(x => x.Value);
            Data1.SetYMapping(y => y.Value);



            OxyModel = CreatePlotModel("Custom PlotController", "Supports left/right keys only");
            Controller = new CustomPlotController();


        }


        private PlotModel _OxyModel;

        public PlotModel OxyModel
        {
            get { return _OxyModel; }
            set { _OxyModel = value; RaisePropertyChanged(); }
        }
        public IPlotController Controller { get; private set; }



        private static PlotModel CreatePlotModel(string title, string subtitle)
        {
            // Create the plot model
            var tmp = new PlotModel { Title = title, Subtitle = subtitle };

            tmp.Axes.Add(new TimeSpanAxis() { Position = AxisPosition.Bottom });
            tmp.Axes.Add(new LinearAxis() { Position = AxisPosition.Left });

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);

            // Axes are created automatically if they are not defined
            return tmp;
        }










        DispatcherTimer updateCollectionTimer;

        private int i = 0;
        void updateCollectionTimer_Tick(object sender, EventArgs e)
        {
            i++;
            voltagePointCollection.Add(new JudgePoint(Math.Sin(i * 0.1), DateTime.Now));
        }


        public JudgePointCollection voltagePointCollection;













    

        private EnumerableDataSource<JudgePoint> _Data1;
        public EnumerableDataSource<JudgePoint> Data1
        {
            get { return _Data1; }
            set { _Data1 = value; RaisePropertyChanged(); }
        }






        private DelegateCommand _TestCommand;

        public DelegateCommand TestCommand =>
            _TestCommand ?? (_TestCommand = new DelegateCommand(ExecuteTestCommand));

        private void ExecuteTestCommand()
        {

        }

        private DelegateCommand _Test2Command;

        public DelegateCommand Test2Command =>
            _Test2Command ?? (_Test2Command = new DelegateCommand(ExecuteTest2Command));

        private void ExecuteTest2Command()
        {


            
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                var s = "";

            }
        }
    }
    public class CustomPlotController : PlotController
    {
        public CustomPlotController()
        {
            //this.UnbindAll();
            this.BindKeyDown(OxyKey.Left, PlotCommands.PanRight);
            this.BindKeyDown(OxyKey.Right, PlotCommands.PanLeft);
        }
    }
}