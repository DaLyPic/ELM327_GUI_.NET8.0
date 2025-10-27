using CommunityToolkit.Mvvm.Input;
using ELM327_GUI.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ELM327_GUI.MVVM.Model;


namespace ELM327_GUI.MVVM.ViewModel
{
    public class DbcParserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<DbcMessage> messages;
        public ObservableCollection<DbcMessage> Messages
        {
            get => messages;
            set { messages = value; OnPropertyChanged(); }
        }

        private DbcMessage selectedMessage;
        public DbcMessage SelectedMessage
        {
            get => selectedMessage;
            set { selectedMessage = value; OnPropertyChanged(); }
        }

        public ICommand ParsingCommand { get; }

        public DbcParserViewModel()
        {
            ParsingCommand = new RelayCommand<object>(ParsingExecute);
        }

        private string SelectDbcFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "DBC fájlok (*.dbc)|*.dbc|Minden fájl (*.*)|*.*";
            openFileDialog.Title = "DBC fájl megnyitása";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        private void ParsingExecute(object obj)
        {
            try
            {
                //string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "11-bit-OBD2-v4.0.dbc");
                string filePath = SelectDbcFile();
                if (string.IsNullOrEmpty(filePath))
                {
                    // A felhasználó nem választott fájlt vagy megszakította
                    MessageBox.Show($"Nem választottál DBC fájlt.", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var parsed = DbcParser.Parse(filePath);
                Messages = new ObservableCollection<DbcMessage>(parsed);

                var sb = new StringBuilder();
                foreach (var msg in parsed)
                {
                    sb.AppendLine($"[ID {msg.Id}] {msg.Name} ({msg.Dlc} byte) - {msg.Transmitter}");
                    foreach (var sig in msg.Signals)
                    {
                        sb.AppendLine($" - Signal: {sig.Name}, Multiplexer: {sig.Multiplexer}, StartBit: {sig.StartBit}, Length: {sig.Length}, Factor: {sig.Factor}, Offset: {sig.Offset}, MIN: {sig.Min}, MAX: {sig.Max}, Unit: {sig.Unit}, Sender: {sig.Sender}");
                    }
                }

                string tempFile = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllText(tempFile, sb.ToString());

                var viewer = new FileViewerWindow(tempFile);
                viewer.Owner = Application.Current.MainWindow;
                viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a parsing során:\n{ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
