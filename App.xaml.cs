using ELM327_GUI.MVVM.View;
using ELM327_GUI.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;
using System;
using System.Windows;
using static ELM327_GUI.MainWindow;

namespace ELM327_GUI
{
    public partial class App : Application
    {

        private IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            GlobalFontSettings.FontResolver = new CustomFontResolver();
            //OnStartupAsync(e);

            var services = new ServiceCollection();

            services.AddSingleton<DbcParserViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<PIDCommandWindowViewModel>();
            services.AddSingleton<ATCommandWindowViewModel>();
            services.AddSingleton<PortSettingsViewModel>();
            services.AddSingleton<VINDecodeWindowViewModel>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>();


            _serviceProvider = services.BuildServiceProvider();

            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();

            var splash = new SplashScreenWindow();
            splash.Show();

            Task.Delay(8000).ContinueWith(t =>
            {
                splash.Dispatcher.Invoke(() =>
                {
                    splash.Close();

                    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                });
            });

        }
        //private async void OnStartupAsync(StartupEventArgs e)
        //{
        //    SplashScreenWindow splash = new SplashScreenWindow();
        //    splash.Show();

        //    await Task.Delay(7000); // várakozás a splash animációra

        //    splash.Close();

        //    MainWindow mainWindow = new MainWindow();
        //    Application.Current.MainWindow = mainWindow;
        //    mainWindow.Show();
        //    await Task.Delay(5000);
        //    mainWindow.Close();
        //}
    }
}
