using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(5));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);

            await Task.Delay(3000);

            
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(5));
            fadeOut.Completed += (s, a) => this.Close();
            this.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}
