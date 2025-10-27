using ELM327_GUI.MVVM.ViewModel;
using System.Windows;

namespace ELM327_GUI.MVVM.View
{
    public partial class PortSettingsWindow : Window
    {
        public PortSettingsWindow(Window owner = null)
        {
            InitializeComponent();

            var wm = new PortSettingsViewModel();
            this.DataContext = wm;
            wm.RequestClose += () => this.Close();
        }
    }
}