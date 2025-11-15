using ELM327_GUI.MVVM.ViewModel;
using System.Windows;


namespace ELM327_GUI.MVVM.View
{
    public partial class ATCommandWindow : Window
    {
        //public ATCommandWindow(ATCommandWindowViewModel vm, Action<string> commandHandler)
        private Action<string> _commandHandler;
        public ATCommandWindow(ATCommandWindowViewModel vm) 
        {
            InitializeComponent();

            //var vm = new ATCommandWindowViewModel();
            //this.DataContext = vm;
            DataContext = vm;
            //vm.CommandEntered += commandHandler;
            vm.RequestClose += () => this.Close();
            this.Loaded += (s, e) => ATCommandTextBox.Focus();
        }
        public void SetCommandHandler(Action<string> commandHandler)
        {
            _commandHandler = commandHandler;
            if (DataContext is ATCommandWindowViewModel vm)
            {
                vm.CommandEntered += _commandHandler;
            }
        }
    }
}


