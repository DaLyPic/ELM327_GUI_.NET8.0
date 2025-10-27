using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ELM327_GUI.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;

namespace ELM327_GUI.MVVM.ViewModel
{
    public partial class PortSettingsViewModel : ObservableObject
    {
        public ObservableCollection<string> Ports { get; } = new ObservableCollection<string>();
        private bool userPortSet = false;

        //[ObservableProperty]
        //private bool userPortSet = false;

        public bool UserPortSet
        {
            get => userPortSet;
            set => SetProperty(ref userPortSet, value);
        }

        //[ObservableProperty]
        //private string portName = "COM1";

        private string portName = "COM1";
        public string PortName
        {
            get => portName;
            set => SetProperty(ref portName, value);
        }

        private int baudRate = 115200;
        public int BaudRate
        {
            get => baudRate;
            set => SetProperty(ref baudRate, value);
        }

        private int dataBits = 8;
        public int DataBits
        {
            get => dataBits;
            set => SetProperty(ref dataBits, value);
        }

        private Parity parity = Parity.None;
        public Parity Parity
        {
            get => parity;
            set => SetProperty(ref parity, value);
        }

        private StopBits stopBits = StopBits.One;
        public StopBits StopBits
        {
            get => stopBits;
            set => SetProperty(ref stopBits, value);
        }

        private Handshake handshake = Handshake.None;
        public Handshake Handshake
        {
            get => handshake;
            set => SetProperty(ref handshake, value);
        }

        private int readTimeout = 6000;
        public int ReadTimeout
        {
            get => readTimeout;
            set => SetProperty(ref readTimeout, value);
        }

        private int writeTimeout = 6000;
        public int WriteTimeout
        {
            get => writeTimeout;
            set => SetProperty(ref writeTimeout, value);
        }

        private string newLine = "\r";
        public string NewLine
        {
            get => newLine;
            set => SetProperty(ref newLine, value);
        }

        public IRelayCommand SaveCommand { get; }

        public event Action RequestClose;

        public PortSettingsViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                Ports.Add(port);
            }
        }

        public enum BaudRateEnum
        {
            Baud_300 = 300,
            Baud_600 = 600,
            Baud_1200 = 1200,
            Baud_2400 = 2400,
            Baud_4800 = 4800,
            Baud_9600 = 9600,
            Baud_14400 = 14400,
            Baud_19200 = 19200,
            Baud_28800 = 28800,
            Baud_31250 = 31250,
            Baud_38400 = 38400,
            Baud_57600 = 57600,
            Baud_115200 = 115200
        }

        public enum DataBitsEnum
        {
            DataBits_5 = 5,
            DataBits_6 = 6,
            DataBits_7 = 7,
            DataBits_8 = 8,
            DataBits_9 = 9
        }

        public enum NewLineEnum
        {
            NewLine_CR = '\r',
            NewLine_LF = '\n',
            NewLine_CRLF = '\r' + '\n',
            NewLine_None = '\0'
        }
        private void OnSave()
        {
            AppConfig.UserPortSet = true;
            AppConfig.PortName = PortName;
            AppConfig.BaudRate = BaudRate;
            AppConfig.DataBits = DataBits;
            AppConfig.Parity = Parity;
            AppConfig.StopBits = StopBits;
            AppConfig.Handshake = Handshake;
            AppConfig.ReadTimeout = ReadTimeout;
            AppConfig.WriteTimeout = WriteTimeout;
            AppConfig.NewLine = NewLine;

            MessageBox.Show("Beállítások mentve:\n" +
                            $"Port: {portName}\n" +
                            $"Baudrate: {baudRate}\n" +
                            $"DataBits: {dataBits}\n" +
                            $"Parity: {parity}\n" +
                            $"StopBits: {stopBits}\n" +
                            $"Handshake: {handshake}\n" +
                            $"ReadTimeout: {readTimeout}\n" +
                            $"WriteTimeout: {writeTimeout}\n" +
                            $"NewLine: {newLine}");

            RequestClose?.Invoke();
        }
    }
}