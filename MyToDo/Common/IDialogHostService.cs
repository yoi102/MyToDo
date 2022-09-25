using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public interface IDialogHostService : IDialogService
    {
        public Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root");
    }
}