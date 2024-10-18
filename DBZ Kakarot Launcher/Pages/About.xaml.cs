using DBZK_Core.Settings;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DBZ_Kakarot_Launcher.Pages
{
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : Page
	{
		public About()
		{
			InitializeComponent();
		}

		public event EventHandler UserInvokedGoBack;

		private void GoBackBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			UserInvokedGoBack?.Invoke(sender, e);
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
            Uri uriSource = new Uri(DefaultConfig.SelectedVideoGame.Logo, UriKind.Relative);
            GameLogo.Source = new BitmapImage(uriSource);

            if (DefaultConfig.CustomWallpaper != null)
			{
				if (DefaultConfig.CustomWallpaper != String.Empty)
				{
					BG_Wallpaper.Source = new BitmapImage(new Uri(DefaultConfig.CustomWallpaper));
				}
			}
		}

		private void Button_OnClick(object sender, MouseButtonEventArgs e)
		{
			ProcessStartInfo xInfo = new ProcessStartInfo()
			{
				UseShellExecute = true,
				FileName = "https://github.com/jrosenbergpc-dev/"
			};

			Process.Start(xInfo);
		}

		private void IGButton_OnClick(object sender, MouseButtonEventArgs e)
		{
			ProcessStartInfo xInfo = new ProcessStartInfo()
			{
				UseShellExecute = true,
				FileName = "https://www.instagram.com/jrosenbergpcdev/"
			};

			Process.Start(xInfo);
		}
	}
}
