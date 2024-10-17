using DBZK_Core.Settings;
using DBZK_Core.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace DBZ_Kakarot_Launcher.Pages
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page
	{
		public Settings()
		{
			InitializeComponent();
		}

		public event EventHandler UserInvokedGoBack;
		public event EventHandler UserInvokedGoAbout;

		private void SetBackgroundBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog xDialog = new OpenFileDialog();
			xDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
			xDialog.Title = "Select an Image File";
			xDialog.ShowDialog();

			if (xDialog.FileName != string.Empty)
			{
				BGImage_Display.Source = new BitmapImage(new Uri(xDialog.FileName));

				DefaultConfig.CustomWallpaper = xDialog.FileName;
				DefaultConfig.UpdateConfigFile();
				DefaultConfig.ReadConfigFile();

				this.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/bg_wallpaper.png")));
				BG_Wallpaper.Source = BGImage_Display.Source;
			}
		}

		private void AutoLocateBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			InstallPath_TB.Text = new FileHandler().LocateGameDirectory();
			DefaultConfig.SelectedVideoGame.InstallationPath = InstallPath_TB.Text;
			DefaultConfig.UpdateConfigFile();
			DefaultConfig.ReadConfigFile();
		}

		private void BrowseBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.ShowDialog();
				if (dialog.SelectedPath != null) 
				{ 
					if (dialog.SelectedPath != string.Empty)
					{
						InstallPath_TB.Text = dialog.SelectedPath;
						DefaultConfig.SelectedVideoGame.InstallationPath = InstallPath_TB.Text;
						DefaultConfig.UpdateConfigFile();
						DefaultConfig.ReadConfigFile();
					}
				}
			}
		}

		private void GoBackBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			UserInvokedGoBack?.Invoke(sender, e);
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			InstallPath_TB.Text = DefaultConfig.SelectedVideoGame.InstallationPath;

			if (DefaultConfig.CustomWallpaper != null)
			{
				if (DefaultConfig.CustomWallpaper != String.Empty)
				{
					BG_Wallpaper.Source = new BitmapImage(new Uri(DefaultConfig.CustomWallpaper));
					BGImage_Display.Source = new BitmapImage(new Uri(DefaultConfig.CustomWallpaper));
				}
			}
		}

		private void ClearBackgroundBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			DefaultConfig.CustomWallpaper = string.Empty;
			DefaultConfig.UpdateConfigFile();
			DefaultConfig.ReadConfigFile();

			this.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/bg.png")));
			BG_Wallpaper.Source = null;
			BGImage_Display.Source = null;
		}

		private void AboutBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			UserInvokedGoAbout?.Invoke(sender, e);

		}
    }
}
