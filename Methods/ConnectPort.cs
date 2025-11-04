using ELM327_GUI.MVVM.View;
using System;
using System.IO.Ports;
using System.Windows;
using ELM327_GUI.MVVM.Model;

namespace ELM327_GUI.Methods
{
    public static class ConnectPort
    {
        public static event Action<string> StatusUpdated;
        public static SerialPort? OpenPort(Window owner = null)
        {
            var selectorWindow = new PortSelectorWindow();
            if (owner != null)
            {
                selectorWindow.Owner = owner;
            }
            bool? result = selectorWindow.ShowDialog();

            if (result != true || selectorWindow.SelectedPort == null)
            {
                StatusUpdated?.Invoke("Nem választottál portot.");
                return null;
            }

            string selectedPort = selectorWindow.SelectedPort;

            try
            {
                string portName;
                int baudRate;
                int dataBits;
                Parity parity;
                StopBits stopBits;
                Handshake handshake;
                int readTimeout;
                int writeTimeout;
                string newLine;

                if (AppConfig.UserPortSet)
                {
                    portName = AppConfig.PortName;
                    baudRate = AppConfig.BaudRate;
                    dataBits = AppConfig.DataBits;
                    parity = AppConfig.Parity;
                    stopBits = AppConfig.StopBits;
                    handshake = AppConfig.Handshake;
                    readTimeout = AppConfig.ReadTimeout;
                    writeTimeout = AppConfig.WriteTimeout;
                    newLine = AppConfig.NewLine;
                }
                else
                {
                    portName = selectedPort;
                    baudRate = 115200;
                    dataBits = 8;
                    parity = Parity.None;
                    stopBits = StopBits.One;
                    handshake = Handshake.None;
                    readTimeout = 3000;
                    writeTimeout = 3000;
                    newLine = "\r";
                }

                var serialPort = new SerialPort(portName, baudRate)
                {
                    DataBits = dataBits,
                    Parity = parity,
                    StopBits = stopBits,
                    Handshake = handshake,
                    ReadTimeout = readTimeout,
                    WriteTimeout = writeTimeout,
                    NewLine = newLine
                };
                serialPort.Open();
                StatusUpdated?.Invoke($"A port {portName} megnyitva.");
                return serialPort;
            }
            catch (Exception ex)
            {
                StatusUpdated?.Invoke($"Hiba történt a port megnyitásakor: {ex.Message}");
                return null;
            }
        }
    }
}
