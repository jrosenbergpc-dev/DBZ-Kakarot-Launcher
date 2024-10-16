using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DBZK_Core.Settings;
using DBZK_Core.Tools;
using Microsoft.Win32;

namespace DBZ_Kakarot_Launcher.Pages
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class GameSelect : Page
	{
		public GameSelect()
		{
			InitializeComponent();
		}

		public event EventHandler PageFinished;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			
		}

		private void LaunchLbl_MouseEnter(object sender, MouseEventArgs e)
		{
			LaunchButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#CCFFEA00"));
		}

		private void LaunchLbl_MouseLeave(object sender, MouseEventArgs e)
		{
			LaunchButton.Background = null;
		}

		private void ManageLbl_MouseEnter(object sender, MouseEventArgs e)
		{
			ManageButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#CCFFEA00"));
		}

		private void ManageLbl_MouseLeave(object sender, MouseEventArgs e)
		{
			ManageButton.Background = null;
		}

        private void LeftArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
			LeftArrow.Opacity = 0.5;
        }

        private void LeftArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
			LeftArrow.Opacity = 1.0;
        }

        private void RightArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
			RightArrow.Opacity = 0.5;
        }

        private void RightArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
			RightArrow.Opacity = 1.0;
        }
    }
}
