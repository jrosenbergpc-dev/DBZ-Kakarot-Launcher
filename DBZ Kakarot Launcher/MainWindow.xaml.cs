using DBZ_Kakarot_Launcher.Pages;
using DBZK_Core.Settings;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBZ_Kakarot_Launcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Welcome m_WelcomePage = new Welcome();
		private Main m_MainPage = new Main();
		private Settings m_SettingsPage = new Settings();
		private About m_AboutPage = new About();

		public MainWindow()
		{
			InitializeComponent();

			m_WelcomePage.PageFinished += WelcomePage_PageFinished;
			m_MainPage.UserLaunchedGame += M_MainPage_UserLaunchedGame;
			m_MainPage.UserInvokedSettings += M_MainPage_UserInvokedSettings;
			m_SettingsPage.UserInvokedGoBack += M_SettingsPage_UserInvokedGoBack;
			m_SettingsPage.UserInvokedGoAbout += M_SettingsPage_UserInvokedGoAbout;
			m_AboutPage.UserInvokedGoBack += M_AboutPage_UserInvokedGoBack;
		}

		private void M_AboutPage_UserInvokedGoBack(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_SettingsPage);
		}

		private void M_SettingsPage_UserInvokedGoAbout(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_AboutPage);
		}

		private void M_SettingsPage_UserInvokedGoBack(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_MainPage);
		}

		private void M_MainPage_UserInvokedSettings(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_SettingsPage);
		}

		private void M_MainPage_UserLaunchedGame(object? sender, EventArgs e)
		{
			Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (DefaultConfig.DoesConfigExist())
			{
				DefaultConfig.ReadConfigFile();

				CoreFrame.Navigate(m_MainPage);
			}
			else
			{
				CoreFrame.Navigate(m_WelcomePage);
			}
		}

		private void CloseButton_OnClick(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void CoreFrame_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			
		}

		private void CoreFrame_Navigated(object sender, NavigationEventArgs e)
		{
			
		}

		private void WelcomePage_PageFinished(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_MainPage);
		}
	}
}
