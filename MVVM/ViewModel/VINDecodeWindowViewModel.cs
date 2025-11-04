using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ELM327_GUI.MVVM.View;
using System.Diagnostics;
using System.Windows;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;

namespace ELM327_GUI.MVVM.ViewModel
{
    public class VINDecodeWindowViewModel : ObservableObject
    {
        private string _vinInput = "WVWZZZ1JZXW000001";
        public string VINInput
        {
            get => _vinInput;
            set => SetProperty(ref _vinInput, value);
        }

        private string _prompt = "Add meg a VIN számot (17 karakter)!";
        public string Prompt
        {
            get => _prompt;
            set => SetProperty(ref _prompt, value);
        }

        private string _title = "VIN dekódolás";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IRelayCommand VINDecodeCommand { get; }
        public IRelayCommand ShowInfoCommand { get; }

        public VINDecodeWindowViewModel()
        {
            VINDecodeCommand = new RelayCommand(OnVINDecode);
            ShowInfoCommand = new RelayCommand(OnShowInfo);
        }

        private void OnVINDecode()
        {
            string connectionString = @"Server=.\SQLEXPRESS;Database=vPICList_Lite1;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.spVINDecode", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@v", VINInput);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable vinTable = new DataTable();
                            adapter.Fill(vinTable);

                            if (vinTable.Rows.Count > 0)
                            {
                                //string result = vinTable.Rows[0][0].ToString();
                                VINResultWindow resultWindow = new VINResultWindow(vinTable);
                                resultWindow.ShowDialog();
                                //MessageBox.Show($"VIN dekódolás eredménye: {result}", "Eredmény");
                            }
                            else
                            {
                                MessageBox.Show("Nincs adat az adott VIN-re.", "Eredmény");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adatbázis művelet során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Application.Current.Windows.OfType<VINDecodeWindow>().FirstOrDefault()?.Close();
                }
            }
        }

        private void OnShowInfo()
        {
            try
            {
                var pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "VIN_number_explained.pdf");
                //var pdfPath = @"E:\Git\source\repo\ELM327_GUI\Files\VIN_number_explained.pdf";
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nem sikerült megnyitni az ismertetőt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}