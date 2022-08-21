using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 汇总
    /// </summary>
    public class SummaryDto : BaseDto
    {

        private int _Sum;
        private int _CompletedCount;
        private int _MemoCount;
        private string? _CompletedRatio;
        private ObservableCollection<ToDoDto> _TodoList = new ObservableCollection<ToDoDto>();
        private ObservableCollection<MemoDto> _MemoList = new ObservableCollection<MemoDto>();
        public int Sum
        {
            get { return _Sum; }
            set { _Sum = value; OnPropertyChanged(); }
        }



        public int CompletedCount
        {
            get { return _CompletedCount; }
            set { _CompletedCount = value; OnPropertyChanged(); }
        }



        public int MemoCount
        {
            get { return _MemoCount; }
            set { _MemoCount = value; OnPropertyChanged(); }
        }


        public string? CompletedRatio
        {
            //get { return (_CompletedCount/ _Sum).ToString("0%"); }
            get { return _CompletedRatio; }
            set { _CompletedRatio = value; OnPropertyChanged(); }
        }



        public ObservableCollection<ToDoDto> TodoList
        {
            get { return _TodoList; }
            set { _TodoList = value; OnPropertyChanged(); }
        }



        public ObservableCollection<MemoDto> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value; OnPropertyChanged(); }
        }














    }
}
