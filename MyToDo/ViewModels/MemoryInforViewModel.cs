using Hardware.Info;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Prism.Commands;
using SkiaSharp;
using System;
using System.Management;
using System.Threading;

namespace MyToDo.ViewModels
{
    public class MemoryInforViewModel : BindableBase
    {
        public MemoryInforViewModel(IHardwareInfo hardwareInfo)
        {
            _physicalUsedMemoryValues = new ObservableCollection<ObservableValue>();
            _virtualUsedMemoryValues = new ObservableCollection<ObservableValue>();

            PhysicalUsedMemorySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                Values = _physicalUsedMemoryValues,
                Fill = new SolidColorPaint(new SKColor(0, 255, 255, 90)),
                Name = "PhysicalUsedMemory",
                LineSmoothness = 0.2
                }
            };
            VirtualUsedMemorySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                Values = _virtualUsedMemoryValues,
                Fill = new SolidColorPaint(new SKColor(0, 255, 255, 90)),
                Name = "VirtualUsedMemory",
                LineSmoothness = 0.2
                }
            };

            Timer timer = new Timer(GetMemoRyData);
            timer.Change(0, 1000);
            this.hardwareInfo = hardwareInfo;
        }

        private readonly ObservableCollection<ObservableValue> _physicalUsedMemoryValues;
        private readonly ObservableCollection<ObservableValue> _virtualUsedMemoryValues;
        private readonly IHardwareInfo hardwareInfo;
        private ObservableCollection<ISeries> _PhysicalUsedMemorySeries;
        private ObservableCollection<ISeries> _VirtualUsedMemorySeries;

        public ObservableCollection<ISeries> PhysicalUsedMemorySeries
        {
            get { return _PhysicalUsedMemorySeries; }
            set { _PhysicalUsedMemorySeries = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<ISeries> VirtualUsedMemorySeries
        {
            get { return _VirtualUsedMemorySeries; }
            set { _VirtualUsedMemorySeries = value; RaisePropertyChanged(); }
        }

        //private string _Text;

        //public string Text
        //{
        //    get { return _Text; }
        //    set { _Text = value; RaisePropertyChanged(); }
        //}

        private void GetMemoRyData(object o)
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

                _physicalUsedMemoryValues.Add(new ObservableValue(value));

                if (_physicalUsedMemoryValues.Count > 20)
                {
                    _physicalUsedMemoryValues.RemoveAt(0);
                }
            }
            osClass.Dispose();
            osClassInstances.Dispose();
            //第三方api
            hardwareInfo.RefreshMemoryStatus();
            //Text = ((double)(hardwareInfo.MemoryStatus.TotalPhysical - hardwareInfo.MemoryStatus.AvailablePhysical) / 1024 / 1024 / 1024).ToString();

            _virtualUsedMemoryValues.Add(new ObservableValue(Math.Round(((double)(hardwareInfo.MemoryStatus.TotalVirtual - hardwareInfo.MemoryStatus.AvailableVirtual) / 1024 / 1024 / 1024) * 100) / 100));

            if (_virtualUsedMemoryValues.Count > 20)
            {
                _virtualUsedMemoryValues.RemoveAt(0);
            }
        }

        public DelegateCommand TestCommand => new DelegateCommand(() =>
        {
        });
    }
}