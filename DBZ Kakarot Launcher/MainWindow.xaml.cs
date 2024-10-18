using DBZ_Kakarot_Launcher.Pages;
using DBZK_Core.Games;
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
using System.Windows.Media.Animation;
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
		private GameSelect m_GameSelect = new GameSelect();

		public MainWindow()
		{
			InitializeComponent();

			m_WelcomePage.GameSelectClicked += M_WelcomePage_GameSelectClicked;
            m_WelcomePage.ModPatchInstalled += M_WelcomePage_ModPatchInstalled;
            m_WelcomePage.ModPatchFailedInstall += M_WelcomePage_ModPatchFailedInstall;
			m_WelcomePage.PageFinished += WelcomePage_PageFinished;

			m_MainPage.UserLaunchedGame += M_MainPage_UserLaunchedGame;
			m_MainPage.GameSelectClicked += M_MainPage_GameSelectClicked;
			m_MainPage.UserInvokedSettings += M_MainPage_UserInvokedSettings;

			m_SettingsPage.UserInvokedGoBack += M_SettingsPage_UserInvokedGoBack;
			m_SettingsPage.UserInvokedGoAbout += M_SettingsPage_UserInvokedGoAbout;

			m_AboutPage.UserInvokedGoBack += M_AboutPage_UserInvokedGoBack;

			m_GameSelect.LaunchWelcomeScreen += M_GameSelect_LaunchWelcomeScreen;
			m_GameSelect.LaunchManageScreen += M_GameSelect_LaunchManageScreen;
            m_GameSelect.PageFinished += M_GameSelect_PageFinished;
		}

        private void M_WelcomePage_ModPatchFailedInstall(object? sender, EventArgs e)
        {
            MessageBox.Show("Mod Patch Failed to Install, Mods may not work correctly. If problem continues, contact jrosenbergpc@gmail.com, [Error Code #MPMW57INST]");
        }

        private void M_GameSelect_PageFinished(object? sender, EventArgs e)
        {
            Close();
        }

        private void M_WelcomePage_ModPatchInstalled(object? sender, EventArgs e)
        {
			MessageBox.Show("Mod Patch Installed Successfully!");
        }

        private void M_MainPage_GameSelectClicked(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_GameSelect);
		}

		private void M_WelcomePage_GameSelectClicked(object? sender, EventArgs e)
		{
			CoreFrame.Navigate(m_GameSelect);
		}

		private void M_GameSelect_LaunchManageScreen(object? sender, EventArgs e)
		{
			if (DefaultConfig.SelectedVideoGame != null)
			{
				CoreFrame.Navigate(m_MainPage);
			}
		}

		private void M_GameSelect_LaunchWelcomeScreen(object? sender, EventArgs e)
		{
			if (DefaultConfig.SelectedVideoGame != null)
			{
				CoreFrame.Navigate(m_WelcomePage);
			}
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

		private void Window_Initialized(object sender, EventArgs e)
		{
			CoreFrame.Navigate(m_GameSelect);
		}

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
			FadeInCloseLabel();
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
			FadeOutCloseLbl();
        }

		private void FadeInCloseLabel()
		{
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(2)),
                AutoReverse = false
            };

            CloseLbl.BeginAnimation(Label.OpacityProperty, animation);
        }

		private void FadeOutCloseLbl()
		{
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = false
            };

            CloseLbl.BeginAnimation(Label.OpacityProperty, animation);
        }
    }
}
