using Hardware.Info;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Prism.Commands;
using SkiaSharp;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace MyToDo.ViewModels
{
    public class MemoryInforViewModel : BindableBase
    {
        public MemoryInforViewModel(IHardwareInfo hardwareInfo)
        {

            _observableValues = new ObservableCollection<ObservableValue>();

            MemorySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                Values = _observableValues,
                Fill = new SolidColorPaint(new SKColor(0, 255, 255, 90)),
                Name = "UsedMemory",
                LineSmoothness = 0.2

                }
            };

            Timer timer = new Timer(GetMemoRyData);
            timer.Change(0, 1000);
            this.hardwareInfo = hardwareInfo;
        }




        private readonly ObservableCollection<ObservableValue> _observableValues;
        private readonly IHardwareInfo hardwareInfo;
        private ObservableCollection<ISeries> _MemorySeries;

        public ObservableCollection<ISeries> MemorySeries
        {
            get { return _MemorySeries; }
            set { _MemorySeries = value; RaisePropertyChanged(); }
        }
        private string _Text;

        public string Text
        {
            get { return _Text; }
            set { _Text = value; RaisePropertyChanged(); }
        }

        


        void GetMemoRyData(object o)
        {
            //WMI 
            ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            var osClassInstances = osClass.GetInstances();
            foreach (var item in osClassInstances)
            {
                var FreeMemory = (double)(ulong)item.Properties["FreePhysicalMemory"].Value;
                var totalMemory = (double)(UInt64)item.Properties["TotalVisibleMemorySize"].Value;

                double value = (totalMemory - FreeMemory) / 1024 / 1024;
                value = Math.Round(value * 100) / 100;

                _observableValues.Add(new ObservableValue(value));

                if (_observableValues.Count > 20)
                {
                    _observableValues.RemoveAt(0);
                }
            }
            osClass.Dispose();
            osClassInstances.Dispose();
            //第三方api
            hardwareInfo.RefreshMemoryStatus();
            Text = ((double)(hardwareInfo.MemoryStatus.TotalPhysical - hardwareInfo.MemoryStatus.AvailablePhysical) / 1024 / 1024 / 1024).ToString();
            
        }



        public DelegateCommand TestCommand => new DelegateCommand(() =>
        {









        });










    }
}
