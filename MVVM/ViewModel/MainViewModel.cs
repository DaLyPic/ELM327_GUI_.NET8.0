using Microsoft.Extensions.DependencyInjection;

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

        public MainViewModel(DbcParserViewModel dbcParserVM, MainWindowViewModel mainVM,
                             PIDCommandWindowViewModel pidCommandVM, ATCommandWindowViewModel atCommandVM,
                             PortSettingsViewModel portSettingsVM, VINDecodeWindowViewModel vinDecodeVM)
        {
            DbcParserVM = dbcParserVM;
            MainVM = mainVM;
            PIDCommandVM = pidCommandVM;
            ATCommandVM = atCommandVM;
            PortSettingsVM = portSettingsVM;
            VINDecodeVM = vinDecodeVM;
        }

    }

    //public class MainViewModel
    //{
    //    public DbcParserViewModel DbcParserVM { get; }
    //    public MainWindowViewModel MainVM { get; }
    //    public PIDCommandWindowViewModel PIDCommandVM { get; }
    //    public ATCommandWindowViewModel ATCommandVM { get; }
    //    public PortSettingsViewModel PortSettingsVM { get; }
    //    public VINDecodeWindowViewModel VINDecodeVM { get; }

    //    public MainViewModel()
    //    {
    //        DbcParserVM = new DbcParserViewModel();
    //        MainVM = new MainWindowViewModel();
    //        PIDCommandVM = new PIDCommandWindowViewModel();
    //        ATCommandVM = new ATCommandWindowViewModel();
    //        PortSettingsVM = new PortSettingsViewModel();
    //        VINDecodeVM = new VINDecodeWindowViewModel();
    //    }
    //}
}