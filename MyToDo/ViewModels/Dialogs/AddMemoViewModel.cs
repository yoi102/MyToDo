using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class AddMemoViewModel : BindableBase, IDialogHostAware
    {
        public AddMemoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private MemoDto _Model;

        public MemoDto Model
        {
            get { return _Model; }
            set { _Model = value; RaisePropertyChanged(); }
        }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Model"))
            {
                Model = parameters.GetValue<MemoDto>("Model");
            }
            else
            {
                Model = new MemoDto();
            }
        }

        private void Save()
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

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, new DialogParameters()));
        }
    }
}