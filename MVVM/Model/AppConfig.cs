using System.IO.Ports;


namespace ELM327_GUI.MVVM.Model
{
    public static class AppConfig
    {
        public static bool UserPortSet { get; set; } = false;

        public static string PortName { get; set; } = "COM5";

        public static int BaudRate { get; set; } = 115200;

        public static int DataBits { get; set; } = 8;

        public static Parity Parity { get; set; } = Parity.None;

        public static StopBits StopBits { get; set; } = StopBits.One;

        public static Handshake Handshake { get; set; } = Handshake.None;

        public static int ReadTimeout { get; set; } = 3000;

        public static int WriteTimeout { get; set; } = 3000;

        public static string NewLine { get; set; } = "\r";

    }
}

