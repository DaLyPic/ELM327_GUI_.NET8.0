using ELM327_GUI.MVVM.ViewModel;
using System.Windows;


namespace ELM327_GUI.MVVM.View
{
    public partial class PIDCommandWindow : Window
    {
        public event Action<string> PIDCommandEntered;
        private Action<string> _commandHandler;
        public PIDCommandWindow(PIDCommandWindowViewModel vm)
        {
            InitializeComponent();
            //var vm = new PIDCommandWindowViewModel();
            //this.DataContext = vm;
            DataContext = vm;
            //vm.CommandEntered += commandHandler;
            vm.RequestClose += () => this.Close();

            this.Loaded += (s, e) => PIDCommandTextBox.Focus();
        }
        public void SetCommandHandler(Action<string> commandHandler)
        {
            _commandHandler = commandHandler;
            if (DataContext is PIDCommandWindowViewModel vm)
            {
                vm.CommandEntered += _commandHandler;
            }
        }
    }
}
