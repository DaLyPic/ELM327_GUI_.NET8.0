using ELM327_GUI.MVVM.ViewModel;
using System;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ELM327_GUI.MVVM.View
{
    public partial class ATCommandWindow : Window
    {
        public ATCommandWindow(Action<string> commandHandler)
        {
            InitializeComponent();

            var vm = new ATCommandWindowViewModel();
            this.DataContext = vm;
            vm.CommandEntered += commandHandler;
            vm.RequestClose += () => this.Close();
            this.Loaded += (s, e) => ATCommandTextBox.Focus();
        }
    }
}


