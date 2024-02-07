using DBZK_Core.Settings;
using DBZK_Core.Tools;
using DBZK_GUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Timers;
using System.Reflection;

namespace DBZ_Kakarot_Launcher.Pages
{
	/// <summary>
	/// Interaction logic for Main.xaml
	/// </summary>
	public partial class Main : Page
	{
		private ModHandler GetModHandler() => new ModHandler();
		private Timer m_Timer = new Timer();

		private int m_CurCount = 0;

		private int m_MaxTime = 10;

		public Main()
		{
			InitializeComponent();

			m_Timer.Interval = 1000;
			m_Timer.Elapsed += M_Timer_Elapsed;
		}

		public event EventHandler UserLaunchedGame;
		public event EventHandler UserInvokedSettings;

		private async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateModList();

			if (DefaultConfig.CustomWallpaper != null)
			{
				if (DefaultConfig.CustomWallpaper != String.Empty)
				{
					this.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/bg_wallpaper.png")));
					BG_Wallpaper.Source = new BitmapImage(new Uri(DefaultConfig.CustomWallpaper));
				}
				else
				{
					this.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/bg.png")));
					BG_Wallpaper.Source = null;
				}
			}

			//before we check for auto launch, lets check for nxmLink download/install first!
			//TODO
			if (App.nxmLink != string.Empty)
			{
				AlertDialog xDialog = new AlertDialog("Get New Mod?", "Automatically Download & Install Mod from nexusmods.com?");
				xDialog.ShowDialog();

				if (xDialog.Result == AlertDialogResult.YES)
				{
					string modfile = await DownloadHandler.GetModFromNexus(App.nxmLink, System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

					if (new FileHandler().FileExists(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + modfile) == true)
					{
						GetModHandler().InstallMod(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + modfile);

						UpdateModList();

						RemoveModBtn.IsEnabled = false;
					}
					else
					{
						AlertDialog xyDialog = new AlertDialog("Failed Download!", "Download Failed to Process, please try again!");
						xyDialog.ShowDialog();
					}
				}
			}

			AutoLaunchChkBx.IsChecked = DefaultConfig.AutoLaunchGameEnabled;

			if (DefaultConfig.AutoLaunchGameEnabled == true)
			{
				StartAutoLaunchTimer();
			}
		}

		private void LaunchGameBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			GetModHandler().LaunchGame();
			UserLaunchedGame?.Invoke(sender, e);
		}

		private void Page_Drop(object sender, DragEventArgs e)
		{
			string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

			fileNames.ToList().ForEach(file =>
			{
				GetModHandler().InstallMod(file);
			});

			UpdateModList();

			RemoveModBtn.IsEnabled = false;
		}

		private void OpenModFolderBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			ProcessStartInfo xInfo = new ProcessStartInfo()
			{
				UseShellExecute = true,
				FileName = DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder
			};

			Process.Start(xInfo);
		}

		private void ModBox_SelectedChanged(object sender, OptionsListItem e)
		{
			RemoveModBtn.IsEnabled = true;
		}

		private void RemoveModBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			AlertDialog xDialog = new AlertDialog("Remove Mod?", "This will remove the mod from the mod folder you will need to re-install the mod to add it back in, are you sure you wish to do this?");
			xDialog.ShowDialog();

			if (xDialog.Result == AlertDialogResult.YES)
			{
				GetModHandler().UninstallMod(ModBox.SelectedItem.ModPackage);

				UpdateModList();

				RemoveModBtn.IsEnabled = false;
				//remove mod
			}
		}

		private void AddNewModBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog xDialog = new OpenFileDialog();
			xDialog.Filter = "Mod Files|*.zip;*.rar;*.pak";
			xDialog.Title = "Select a Mod File";
			xDialog.ShowDialog();

			if (xDialog.FileName != string.Empty)
			{
				GetModHandler().InstallMod(xDialog.FileName);

				UpdateModList();
			}
		}

		private void UpdateModList()
		{
			ModBox.OptionsListItems.Clear();

			GetModHandler().GetMods().ForEach(m =>
			{
				OptionsListItem xItem = new OptionsListItem()
				{
					Text = m.Name,
					IsModEnabled = m.IsEnabled,
					ModPackage = m
				};

				ModBox.OptionsListItems.Add(xItem);
			});
		}

		private void ExplrModsBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			GetModHandler().LaunchModViewer();
		}

		private void SettingsBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			UserInvokedSettings?.Invoke(sender, e);
		}

		private void AutoLaunchChkBx_Click(object sender, RoutedEventArgs e)
		{
			if (AutoLaunchChkBx.IsChecked == true)
			{
				DefaultConfig.AutoLaunchGameEnabled = true;
				DefaultConfig.UpdateConfigFile();

				StartAutoLaunchTimer();
			}
			else
			{
				DefaultConfig.AutoLaunchGameEnabled = false;
				DefaultConfig.UpdateConfigFile();

				CancelAutoLaunchTimer();
			}
        }

		private void StartAutoLaunchTimer()
		{
			m_CurCount = 0;
			AutoLaunchProgBar.Maximum = m_MaxTime;
			AutoLaunchProgBar.Value = m_CurCount;

			AutoLaunchLbl.Content = "Auto-Launching... (" + (m_MaxTime - m_CurCount) + ") Seconds";

			AutoLaunchLbl.Visibility = Visibility.Visible;
			AutoLaunchProgBar.Visibility = Visibility.Visible;

			m_Timer.Start();
		}

		private void CancelAutoLaunchTimer()
		{
			if (m_CurCount < m_MaxTime)
			{
				m_Timer.Stop();

				m_CurCount = 0;
				AutoLaunchProgBar.Maximum = m_MaxTime;
				AutoLaunchProgBar.Value = m_CurCount;

				AutoLaunchProgBar.SetPercent(m_CurCount);

				AutoLaunchLbl.Visibility = Visibility.Collapsed;
				AutoLaunchProgBar.Visibility = Visibility.Collapsed;
			}
		}

		private void M_Timer_Elapsed(object? sender, ElapsedEventArgs e)
		{
			m_CurCount++;

			Dispatcher.Invoke(new Action(() => 
			{
				AutoLaunchProgBar.SetPercent(m_CurCount);
				AutoLaunchLbl.Content = "Auto-Launching... (" + (m_MaxTime - m_CurCount) + ") Seconds";
			}));

			if (m_CurCount >= m_MaxTime)
			{
				m_Timer.Stop();

				Dispatcher.Invoke(new Action(() =>
				{
					AutoLaunchProgBar.SetPercent(100);
					AutoLaunchLbl.Content = "Auto-Launching DBZ Kakarot™!";
				}));

				GetModHandler().LaunchGame();

				Dispatcher.Invoke(new Action(() =>
				{
					UserLaunchedGame?.Invoke(sender, e);
				}));
			}
			else
			{
				m_Timer.Start();
			}
		}
	}
}
