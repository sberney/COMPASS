﻿using COMPASS.Models;
using COMPASS.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace COMPASS.Views
{
  /// <summary>
  /// Interaction logic for HomeLayout.xaml
  /// </summary>
  public partial class HomeLayout : UserControl
  {
    public HomeLayout()
    {
      InitializeComponent();
    }

    public void HandleDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Codex toOpen = ((ListBoxItem)sender).DataContext as Codex;
      _ = CodexViewModel.OpenCodex(toOpen);
    }
  }
}
