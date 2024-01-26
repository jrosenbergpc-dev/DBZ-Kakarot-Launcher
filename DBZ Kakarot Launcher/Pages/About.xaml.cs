using DBZK_Core.Settings;
using System;
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
			if (DefaultConfig.CustomWallpaper != null)
			{
				if (DefaultConfig.CustomWallpaper != String.Empty)
				{
					BG_Wallpaper.Source = new BitmapImage(new Uri(DefaultConfig.CustomWallpaper));
				}
			}
		}
    }
}
