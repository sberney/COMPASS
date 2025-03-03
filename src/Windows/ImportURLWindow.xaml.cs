﻿using COMPASS.ViewModels;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace COMPASS.Windows
{
    /// <summary>
    /// Interaction logic for ImportURLWindow.xaml
    /// </summary>
    public partial class ImportURLWindow : Window
    {
        public ImportURLWindow(ImportViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
