using DBZK_Core.Tools;
using DBZK_Core.Interfaces;
using DBZK_Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBZK_Core.Settings
{
	public static class DefaultConfig
	{
		private static FileHandler GetFileHandler() => new FileHandler();
		private static List<IVideoGame> Games { get; set; } = new List<IVideoGame>();

		private static string ConfigFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\launcher.cfgset";

		public static string AcceptedProtocol { get; set; } = "nxm://";
		public static double ConfigVersion { get; set; } = 2.0;
		public static string CustomWallpaper { get; set; } = string.Empty;
		public static bool AutoLaunchGameEnabled { get; set; } = false;

		public static void SetupConfigFile()
		{
			if (DoesConfigExist() == false)
			{
				Games.ForEach(game =>
				{
                    List<string> configdata = new List<string>();

                    if (game.InstallationPath != string.Empty)
                    {
                        GetFileHandler().CreateFolder(game.InstallationPath + "\\" + game.ModFolder);

                        GetFileHandler().CreateFolder(game.InstallationPath + "\\" + game.DisableFolder);

                        GetFileHandler().CreateEmptyFile(ConfigFile);

						configdata.Add("[" + game.Name + "]");
                        configdata.Add("InstallPath=" + game.InstallationPath);
						configdata.Add("ModFolder=" + game.ModFolder);
						configdata.Add("DisableFolder=" + game.DisableFolder);
						configdata.Add("ModPatchRequired=" + game.ModPatchRequired.ToString());
                        configdata.Add("Version=" + game.Version);

                        GetFileHandler().WriteToFile(ConfigFile, configdata);
                    }
                    else
                    {
                        //throw new Exception("No Installation Path Set!");
                    }
                });
			}
			else
			{
				//
			}
		}

		public static bool DoesConfigExist()
		{
			return GetFileHandler().FileExists(ConfigFile);
		}

		public static void ReadConfigFile()
		{
			List<string> configdata = GetFileHandler().ReadAllContentsFromFile(ConfigFile);

			configdata.ForEach(line =>
			{
				IVideoGame CurrentGame = null;

				if (line.StartsWith("["))
				{
					if (line.Replace("[", "").Replace("]", "") == new DBZKakarot().Name)
					{
						CurrentGame = new DBZKakarot();
					}
					else if (line.Replace("[", "").Replace("]", "") == new DBZSparkingZero().Name)
                    {
                        CurrentGame = new DBZSparkingZero();
                    }
                }
				if (line.StartsWith("InstallPath="))
				{
					string[] s_split = line.Split('=');

					if (s_split.Count() > 1)
					{
						if (CurrentGame != null)
						{
							CurrentGame.InstallationPath = s_split[1];
						}
					}
				}
				else if (line.StartsWith("ModFolder="))
                {
                    string[] s_split = line.Split('=');

                    if (s_split.Count() > 1)
                    {
                        if (CurrentGame != null)
                        {
                            CurrentGame.ModFolder = s_split[1];
                        }
                    }
                }
                else if (line.StartsWith("DisableFolder="))
                {
                    string[] s_split = line.Split('=');

                    if (s_split.Count() > 1)
                    {
                        if (CurrentGame != null)
                        {
                            CurrentGame.DisableFolder = s_split[1];
                        }
                    }
                }
                else if (line.StartsWith("ModPatchRequired="))
                {
                    string[] s_split = line.Split('=');

                    if (s_split.Count() > 1)
                    {
                        if (CurrentGame != null)
                        {
                            CurrentGame.ModPatchRequired = bool.Parse(s_split[1]);
                        }
                    }
                }
                else if (line.StartsWith("Version="))
                {
                    string[] s_split = line.Split('=');

                    if (s_split.Count() > 1)
                    {
                        if (CurrentGame != null)
                        {
                            CurrentGame.Version = s_split[1];
							Games.Add(CurrentGame); //add it here after the last property gets filled
                        }
                    }
                }
                else if (line.StartsWith("CustomWallpaper="))
				{
					string[] s_split = line.Split('=');

					if (s_split.Count() > 1)
					{
						CustomWallpaper = s_split[1];
					}
				}
				else if (line.StartsWith("AutoLaunch="))
				{
					string[] s_split = line.Split('=');

					if (s_split.Count() > 1)
					{
						AutoLaunchGameEnabled = bool.Parse(s_split[1]);
					}
				}
			});
		}

		public static void UpdateConfigFile()
		{
			List<string> configdata = new List<string>();

			configdata.Add("InstallPath=" + InstallationPath);
			configdata.Add("CustomWallpaper=" + CustomWallpaper);
			configdata.Add("AutoLaunch=" + AutoLaunchGameEnabled.ToString());
			configdata.Add("Version=1.0");

			GetFileHandler().WriteToFile(ConfigFile, configdata);
		}
	}
}
