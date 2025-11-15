using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ELM327_GUI.MVVM.ViewModel
{
    public partial class ATCommandWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string commandText;

        [RelayCommand]
        private void Send()
        {
            string command = CommandText?.Trim();
            if (string.IsNullOrEmpty(command))
                return;

            if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
            {
                RequestClose?.Invoke();
                return;
            }

            CommandEntered?.Invoke(command);
            CommandText = string.Empty;
        }


        public event Action<string> CommandEntered;
        public event Action RequestClose;
    }
}

//private string _commandText;

//public string CommandText
//{
//    get => _commandText;
//    set => SetProperty(ref _commandText, value);
//}

//public ICommand SendCommand { get; }

//public ATCommandWindowViewModel()
//{
//    SendCommand = new RelayCommand(ExecuteSend);
//}

//private void ExecuteSend()
//{
//    string command = CommandText?.Trim();
//    if (string.IsNullOrEmpty(command))
//        return;

//    if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
//    {
//        RequestClose?.Invoke();
//        return;
//    }

//    CommandEntered?.Invoke(command);
//    CommandText = string.Empty;
//}

//MessageBox.Show("AT parancs ablak bezárva az 'exit' parancs miatt.", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);