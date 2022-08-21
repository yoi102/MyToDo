using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.Dialogs
{
    public class MsgViewModel : BindableBase, IDialogHostAware
    {


        public MsgViewModel()
        {

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);

        }





        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }




        private string _Title;
        private string _Content;


  


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







        public void OnDialogOpend(IDialogParameters parameters)
        {


            if (parameters.ContainsKey("Title"))
                Title = parameters.GetValue<string>("Title");
            if (parameters.ContainsKey("Content"))
                Content = parameters.GetValue<string>("Content");


        }
        /// <summary>
        /// 确定
        /// </summary>
        void Save()
        {

       

            if (DialogHost.IsDialogOpen(DialogHostName))
                {
                    DialogParameters param = new DialogParameters();
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));

                }
        }
        /// <summary>
        /// 取消
        /// </summary>
        void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, new DialogParameters()));

        }
    }
}
