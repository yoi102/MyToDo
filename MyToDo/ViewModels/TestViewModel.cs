using MyToDo.Service;
using Prism.Commands;

namespace MyToDo.ViewModels
{
    public class TestViewModel : BindableBase
    {
        private readonly ITestService testService;

        public TestViewModel(ITestService testService)
        {
            this.testService = testService;
        }

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; RaisePropertyChanged(); }
        }

        private DelegateCommand _TestCommand;

        public DelegateCommand TestCommand =>
            _TestCommand ?? (_TestCommand = new DelegateCommand(ExecuteTestCommand));

        private void ExecuteTestCommand()
        {
            testService.MyProperty += 1;

            MyProperty = testService.MyProperty;
        }

        private DelegateCommand _Test2Command;

        public DelegateCommand Test2Command =>
            _Test2Command ?? (_Test2Command = new DelegateCommand(ExecuteTest2Command));

        private void ExecuteTest2Command()
        {
            testService.MyProperty += 1;

            MyProperty = testService.MyProperty;
        }
    }
}