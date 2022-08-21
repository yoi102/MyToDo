using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class TaskBar : BindableBase
    {
        private string _Icon;
        private string _Title;
        private string _Content;
        private string _Color;
        private string _Target;

        public string Icon
        {
            get { return _Icon; }
            set { _Icon = value; RaisePropertyChanged(); }
        }



        public string Title
        {
            get { return _Title; }
            set { _Title = value; RaisePropertyChanged(); }
        }

        public string Content
        {
            get { return _Content; }
            set { _Content = value; RaisePropertyChanged(); }
        }

        public string Color
        {
            get { return _Color; }
            set { _Color = value; RaisePropertyChanged(); }
        }

        public string Target
        {
            get { return _Target; }
            set { _Target = value; RaisePropertyChanged(); }
        }







    }
}
