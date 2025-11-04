namespace ELM327_GUI.MVVM.ViewModel
{
    public class MainViewModel
    {
        public DbcParserViewModel DbcParserVM { get; }
        public MainWindowViewModel MainVM { get; }
        public PIDCommandWindowViewModel PIDCommandVM { get; }
        public ATCommandWindowViewModel ATCommandVM { get; }
        public PortSettingsViewModel PortSettingsVM { get; }
        public VINDecodeWindowViewModel VINDecodeVM { get; }

        public MainViewModel()
        {
            DbcParserVM = new DbcParserViewModel();
            MainVM = new MainWindowViewModel();
            PIDCommandVM = new PIDCommandWindowViewModel();
            ATCommandVM = new ATCommandWindowViewModel();
            PortSettingsVM = new PortSettingsViewModel();
            VINDecodeVM = new VINDecodeWindowViewModel();
        }
    }
}