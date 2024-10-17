using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DBZK_Core.Games;
using DBZK_Core.Interfaces;
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
		private ModHandler GetModHandler() => new ModHandler();

		private List<IVideoGame> AvailableGames = new List<IVideoGame>();
		private int CurrentSelectionIndex = 0;

		public GameSelect()
		{
			InitializeComponent();
		}

		public event EventHandler LaunchWelcomeScreen;
		public event EventHandler PageFinished;

		private List<IVideoGame> GetSupportedGames()
		{
			return new List<IVideoGame>
			{
				new DBZKakarot(),
				new DBZSparkingZero(),
				// Add future games here
			};
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			ConfigureAvailableGames();
			ChangeSlideshowImage();
		}

		private void ConfigureAvailableGames()
		{
			if (DefaultConfig.DoesConfigExist())
			{
				DefaultConfig.ReadConfigFile();

				AvailableGames = DefaultConfig.Games;

				AddUnConfiguredGames();
			}
			else
			{
				AddAllSupportedGames();
			}
		}

		private void AddAllSupportedGames()
		{
			AvailableGames = GetSupportedGames();
		}

		private void AddUnConfiguredGames()
		{
			GetSupportedGames().ForEach(game =>
			{
				if (!AvailableGames.Any(existingGame => existingGame.GetType() == game.GetType()))
				{
					AvailableGames.Add(game);
				}
			});
		}

		private void ChangeSlideshowImage()
		{
			DefaultConfig.SelectedVideoGame = AvailableGames[CurrentSelectionIndex];

			Uri uriSource = new Uri(DefaultConfig.SelectedVideoGame.Logo, UriKind.Relative);
			GameImage.Source = new BitmapImage(uriSource);

			if (string.IsNullOrEmpty(DefaultConfig.SelectedVideoGame.InstallationPath))
			{
				LaunchLbl.Content = "SETUP GAME";
				ManageLbl.Foreground = new SolidColorBrush(Colors.Gray);
				ManageLbl.IsEnabled = false;
			}
			else
			{
				LaunchLbl.Content = "LAUNCH GAME";
                ManageLbl.Foreground = new SolidColorBrush(Colors.White);
				ManageLbl.IsEnabled = true;
            }
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

			CurrentSelectionIndex--;

			if (CurrentSelectionIndex < 0)
			{
				CurrentSelectionIndex = AvailableGames.Count - 1;
			}

			ChangeSlideshowImage();
		}

        private void RightArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
			RightArrow.Opacity = 0.5;
        }

        private void RightArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
			RightArrow.Opacity = 1.0;

			CurrentSelectionIndex++;

			if (CurrentSelectionIndex > AvailableGames.Count - 1)
			{
				CurrentSelectionIndex = 0;
			}

			ChangeSlideshowImage();
        }

		private void LaunchLbl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (string.IsNullOrEmpty(DefaultConfig.SelectedVideoGame.InstallationPath))
			{
				LaunchWelcomeScreen?.Invoke(null, new EventArgs());
			}
			else
			{
				GetModHandler().LaunchGame();
			}
		}
	}
}
