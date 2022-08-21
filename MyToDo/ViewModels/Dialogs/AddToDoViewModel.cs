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
    public class AddToDoViewModel :BindableBase, IDialogHostAware
    {


        public AddToDoViewModel()
        {

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);

        }






        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private ToDoDto _Model;
        public ToDoDto Model
        {
            get { return _Model; }
            set { _Model = value; RaisePropertyChanged(); }
        }




   




        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Model"))
            {
                Model = parameters.GetValue<ToDoDto>("Model");

            }
            else
            {
                Model = new ToDoDto();

            }

        }







        void Save()
        {
            if (string.IsNullOrWhiteSpace(Model.Content) || string.IsNullOrWhiteSpace(Model.Title))
                return;
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Model", Model);

                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));

            }
        }
        void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, new DialogParameters()));

        }
    }
}
