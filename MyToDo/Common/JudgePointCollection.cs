using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicDataDisplay.Common;

namespace MyToDo.Common
{
    public class JudgePointCollection : RingArray<JudgePoint>
    {
        private const int TOTAL_POINTS = 6000;

        public JudgePointCollection()
            : base(TOTAL_POINTS) // here i set how much values to show
        {
        }
    }

    public class JudgePoint : BindableBase
    {

        public JudgePoint(double value, DateTime recordTime)
        {
            this.Value = value;
            this.RecordTime = recordTime;
        }



        /// <summary>
        /// record time
        /// </summary>
        private DateTime _RecordTime;
        public DateTime RecordTime
        {
            get
            {
                return _RecordTime;
            }
            set
            {
                if (_RecordTime != value)
                {
                    _RecordTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Judge value
        /// </summary>
        private double _Value;
        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    RaisePropertyChanged();
                }
            }
        }

       
    }
}
