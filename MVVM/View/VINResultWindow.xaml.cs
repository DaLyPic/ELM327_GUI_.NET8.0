using System.Data;
using System.Windows;

namespace ELM327_GUI.MVVM.View
{
    public partial class VINResultWindow : Window
    {
        public VINResultWindow(DataTable vinDataTable)
        {
            InitializeComponent();
            VinDataGrid.ItemsSource = vinDataTable.DefaultView;
        }
    }
}
