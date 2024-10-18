using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DBZK_Core.Settings;
using DBZK_Core.Tools;
using Microsoft.Win32;

namespace DBZ_Kakarot_Launcher.Pages
{
	/// <summary>
	/// Interaction logic for Welcome.xaml
	/// </summary>
	public partial class Welcome : Page
	{
		public Welcome()
		{
			InitializeComponent();
		}

		public event EventHandler GameSelectClicked;
		public event EventHandler ModPatchInstalled;
		public event EventHandler ModPatchFailedInstall;
		public event EventHandler PageFinished;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			InstallPath_TB.Text = new FileHandler().LocateGameDirectory();
			WelcomeText.Text = "Let's get started by locating your " + DefaultConfig.SelectedVideoGame.Name + " Game Installation!";

            Uri uriSource = new Uri(DefaultConfig.SelectedVideoGame.Logo, UriKind.Relative);
            GameLogo.Source = new BitmapImage(uriSource);
        }

		private void AutoLocateBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			InstallPath_TB.Text = new FileHandler().LocateGameDirectory();
		}

		private async void NextBtn_OnClick(object sender, MouseButtonEventArgs e)
		{
			DefaultConfig.SelectedVideoGame.InstallationPath = InstallPath_TB.Text;
			DefaultConfig.UpdateConfigFile();

			if (InstallModLink_ChkBx.IsChecked == true)
			{
				//Start Process to install reg file!
				try
				{
					System.IO.File.WriteAllText(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ModLinkDeploy.reg", Properties.Resources.ModLinkDeploy);

					List<string> xLineToAdd = new List<string>();
					string newLine = "@=\"\\\"" + System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", @"\\") + @"\\DBZ Kakarot Launcher.exe\" + "\" \\\"%1\\\"\"";
					xLineToAdd.Add(newLine);

					System.IO.File.AppendAllLines(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ModLinkDeploy.reg", xLineToAdd);

					ProcessStartInfo xInfo = new ProcessStartInfo()
					{
						UseShellExecute = true,
						Verb = "runas",
						FileName = "regedit.exe",
						Arguments = "/s \"" + System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ModLinkDeploy.reg\""
					};

					Process.Start(xInfo).WaitForExit();
				}
				catch (Exception)
				{

				}
			}

			if (DefaultConfig.SelectedVideoGame.ModPatchRequired)
			{
				if (DefaultConfig.SelectedVideoGame.InstallPatch())
				{
                    ModPatchInstalled?.Invoke(this, new EventArgs());
                }
				else
				{
                    ModPatchFailedInstall?.Invoke(this, new EventArgs());

                }
			}

			PageFinished?.Invoke(sender, new EventArgs());
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
			GameSelectClicked?.Invoke(null, new EventArgs());
		}
	}
}
