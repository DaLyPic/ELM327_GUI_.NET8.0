using ELM327_GUI.MVVM.View;
using PdfSharp.Fonts;
using System.Windows;
using static ELM327_GUI.MainWindow;

namespace ELM327_GUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            GlobalFontSettings.FontResolver = new CustomFontResolver();
            OnStartupAsync(e);
        }
        private async void OnStartupAsync(StartupEventArgs e)
        {
            SplashScreenWindow splash = new SplashScreenWindow();
            splash.Show();

            await Task.Delay(9000); // várakozás a splash animációra

            splash.Close();

            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        //{
        //    base.OnStartup(e);
        //    GlobalFontSettings.FontResolver = new CustomFontResolver();

        //    SplashScreenWindow splash = new SplashScreenWindow();
        //    splash.Show();

        //    MainWindow mainWindow = new MainWindow();
        //    Application.Current.MainWindow = mainWindow;
        //    mainWindow.Show();
        //}
    }
}
