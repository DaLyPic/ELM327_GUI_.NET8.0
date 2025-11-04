using System.Windows;
using System.Windows.Media.Animation;

namespace ELM327_GUI.MVVM.View
{
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
            this.Opacity = 0;
            Loaded += SplashScreenWindow_Loaded;
        }

        private async void SplashScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(3));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);

            await Task.Delay(2000);

            
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(3));
            fadeOut.Completed += (s, a) => this.Close();
            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}
