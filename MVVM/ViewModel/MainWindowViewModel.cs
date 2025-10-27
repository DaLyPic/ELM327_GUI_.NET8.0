using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ELM327_GUI.Methods;
using ELM327_GUI.MVVM.View;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;

namespace ELM327_GUI.MVVM.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private SerialPort serialPort;
        //private List<string> response = new List<string>();

        [ObservableProperty]
        private string connectionStatus;

        [ObservableProperty]
        private ObservableCollection<string> responses = new();


        public ICommand AutodataCommand { get; }
        public ICommand ATcommandCommand { get; }
        public ICommand PIDcommandCommand { get; }
        //public ICommand ParsingCommand { get; }
        public ICommand UdsCommand { get; }
        public ICommand HowtouseCommand { get; }
        public ICommand PortsettingCommand { get; }
        public ICommand PDFreportgenCommand { get; }
        public ICommand CommandsfromfileCommand { get; }
        public ICommand ExitCommand { get; }

        public MainWindowViewModel()
        {
            AutodataCommand = new RelayCommand<object>(AutodataExecute);
            ATcommandCommand = new RelayCommand<object>(ATcommandExecute);
            PIDcommandCommand = new RelayCommand<object>(PIDcommandExecute);
            //ParsingCommand = new RelayCommand<object>(ParsingExecute);
            UdsCommand = new RelayCommand<object>(UdsExecute);
            HowtouseCommand = new RelayCommand<object>(HowtouseExecute);
            PortsettingCommand = new RelayCommand<object>(PortsettingExecute);
            PDFreportgenCommand = new RelayCommand<object>(PDFreportgenExecute);
            CommandsfromfileCommand = new RelayCommand<object>(CommandsfromfileExecute);
            ExitCommand = new RelayCommand<object>(ExitExecute);
            ConnectPort.StatusUpdated += status => ConnectionStatus = status;
        }

        #region Autó adatai
        private void AutodataExecute(object obj)
        {
            var dialog = new InputDialog("Enter data:", "Autó adatai", "");
            dialog.ShowDialog();
        }
        #endregion
        #region AT parancsok
        private void ATcommandExecute(object obj)
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();

            serialPort = ConnectPort.OpenPort(Application.Current.MainWindow);
            if (serialPort == null)
                return;

            try
            {
                var commandWindow = new ATCommandWindow(HandleATCommand)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Application.Current.MainWindow
                };
                commandWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                ConnectionStatus=$"Hiba történt a port megnyitásakor: {ex.Message}";
            }
        }

            
        private void HandleATCommand(string command)
        {
            ConnectionStatus = $"Parancs: {command}";
            SendATCommand(command);
        }
        private void SendATCommand(string command)
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                ConnectionStatus = "A port nincs megnyitva!";
                return;
            }

            try
            {
                serialPort.WriteLine(command);
                string resp = serialPort.ReadLine();
                ConnectionStatus = $"Válasz: {resp}";
                Responses.Add(resp);

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "response.txt");
                File.AppendAllText(filePath, $"Parancs: {command}{Environment.NewLine}Válasz: {Environment.NewLine}{resp}{Environment.NewLine}");
            }
            catch (TimeoutException)
            {
                ConnectionStatus = "Nincs válasz az ELM327 eszköztől (időtúllépés).";
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Hiba az AT parancs küldésekor: {ex.Message}";
            }
        }
        #endregion
        #region PID parancsok
        private void PIDcommandExecute(object obj)
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();

            serialPort = ConnectPort.OpenPort(Application.Current.MainWindow);
            if (serialPort == null)
                return;
            try
            {
            var commandWindow = new PIDCommandWindow(HandlePIDCommand)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            commandWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Hiba történt a port megnyitásakor: {ex.Message}";
            }

           
        }
        private void HandlePIDCommand(string command)
        {
            ConnectionStatus = $"Parancs: {command}";
            SendATCommand(command);
        }
        #endregion
     
        #region UDS kompatibilitás
        private void UdsExecute(object obj)
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();

            serialPort = ConnectPort.OpenPort(Application.Current.MainWindow);
            if (serialPort == null)
                return;

            ConnectionStatus = "UDS kompatibilitás ellenőrzés";
            try
            {
                serialPort.WriteLine("AT Z");
                System.Threading.Thread.Sleep(1000);

                serialPort.WriteLine("AT SP 6");    // Protokoll beállítás ISO 15765-4 (CAN 11 bit) - mely UDS támogatására is alkalmas
                System.Threading.Thread.Sleep(1000);

                serialPort.WriteLine("10 01");      // UDS StartDiagnosticSession - Default session indítása


                System.Threading.Thread.Sleep(500);

                string response = serialPort.ReadExisting();
                string hexString = response.Replace("-", " ");

                // Ellenőrzés, hogy kaptunk-e pozitív választ (pl. 50 01)
                if (response.Contains("50 01"))
                {
                    ConnectionStatus = "Az autó támogatja az UDS protokollt és elfogadta a Start Session parancsot.";
                    
                }
                else
                {
                    ConnectionStatus = $"Válasz érkezett, de nem az elvárt UDS Start Session: {response}";
                    ConnectionStatus = $"Válasz: {hexString}";
                }
             
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Hiba történt a port megnyitásakor: {ex.Message}";
            }
        }
        #endregion
        #region Használati útmutató
        private void HowtouseExecute(object obj)
        {
            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "HowToUse.pdf");
            if (File.Exists(pdfPath))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                };
                Process.Start(psi);

            }
            else
            {
                MessageBox.Show("HowToUse.pdf not found in Files folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion region
        #region Port beállítás
        private void PortsettingExecute(object obj)
        {
            var window = new PortSettingsWindow();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //var wm = new PortSettingsViewModel();
            //window.DataContext = wm;
            //wm.RequestClose += () => window.Close();
            window.ShowDialog();

        }
        #endregion
        #region PDF riport generálás
        private void PDFreportgenExecute(object obj)
        {
            string responseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "response.txt");
            if (!File.Exists(responseFile))
            {
                MessageBox.Show("response.txt nem található.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[] lines = File.ReadAllLines(responseFile);

            PdfDocument document = new PdfDocument();
            document.Info.Title = "ELM327 AT Command Responses";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12);

            double margin = 40;
            double lineHeight = 20;
            double yPoint = margin;
            double usableHeight = page.Height.Point - margin;

            foreach (string line in lines)
            {
                gfx.DrawString(line, font, XBrushes.Black, new XRect(margin, yPoint, page.Width.Point - 2 * margin, usableHeight), XStringFormats.TopLeft);
                yPoint += lineHeight;
                if (yPoint > page.Height.Point - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin;
                }
            }

            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "response_report.pdf");
            document.Save(pdfPath);
            MessageBox.Show($"PDF riport elkészült: {pdfPath}", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        #region Parancsok fájlból

        private string SelectCommandsFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Szöveg fájlok (*.txt)|*.txt|Minden fájl (*.*)|*.*";
            openFileDialog.Title = "Parancsfájl megnyitása";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        private void CommandsfromfileExecute(object obj)
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();

            serialPort = ConnectPort.OpenPort(Application.Current.MainWindow);
            if (serialPort == null)
                return;

            string commandsFilePath = SelectCommandsFile();
            if (string.IsNullOrEmpty(commandsFilePath))
            {
                ConnectionStatus = "Nincs parancsfájl kiválasztva.";
                return;
            }

            try
            {
                string[] commands = File.ReadAllLines("ELM327_ATCommands.txt");
                

                if (serialPort == null || !serialPort.IsOpen)
                {
                    ConnectionStatus = "A soros port nincs megnyitva.";
                    return;
                }

                using (StreamWriter responseWriter = new StreamWriter("Response.txt", append: false))

                    foreach (string command in commands)
                {
                    if (string.IsNullOrWhiteSpace(command))
                        continue;

                    serialPort.WriteLine(command);

                    System.Threading.Thread.Sleep(200);

                    string response = serialPort.ReadExisting();

                    responseWriter.WriteLine($"Időbélyegző: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    responseWriter.WriteLine($"=== Parancs: {command} ===");
                    responseWriter.WriteLine(response);
                    responseWriter.WriteLine();

                    ConnectionStatus = $"Parancs: {command}\nVálasz: {response}";
                }

                ConnectionStatus = "Parancsok elküldve, válaszok mentve a Response.txt fájlba.";
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Hiba történt: {ex.Message}";
            }
        }
        #endregion
        #region Kilépés
        
        private void ExitExecute(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            Application.Current.Shutdown());
        }
        #endregion
    }
}
