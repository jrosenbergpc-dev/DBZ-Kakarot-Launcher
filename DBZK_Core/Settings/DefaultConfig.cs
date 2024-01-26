using DBZK_Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace DBZK_Core.Settings
{
	public static class DefaultConfig
	{
		private static FileHandler GetFileHandler() => new FileHandler();

		private static string ConfigFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\launcher.cfgset";

		public static string InstallationPath { get; set; } = string.Empty;
		public static string ModFolder { get; set; } = "\\AT\\Content\\Paks\\~mods";
		public static string DisableFolder { get; set; } = "\\AT\\Content\\Paks\\~disabled";
		public static string AcceptedProtocol { get; set; } = "nxm://";
		public static double Version { get; set; } = 1.0;
		public static string CustomWallpaper { get; set; } = string.Empty;
		public static bool AutoLaunchGameEnabled { get; set; } = false;

		public static void SetupConfigFile()
		{
			if (DoesConfigExist() == false)
			{
				if (InstallationPath != string.Empty)
				{
					GetFileHandler().CreateFolder(InstallationPath + "\\" + ModFolder);

					GetFileHandler().CreateFolder(InstallationPath + "\\" + DisableFolder);

					GetFileHandler().CreateEmptyFile(ConfigFile);

					List<string> configdata = new List<string>();

					configdata.Add("InstallPath=" + InstallationPath);
					configdata.Add("Version=1.0");

					GetFileHandler().WriteToFile(ConfigFile, configdata);
				}
				else
				{
					//throw new Exception("No Installation Path Set!");
				}
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
				if (line.StartsWith("InstallPath="))
				{
					string[] s_split = line.Split('=');

					if (s_split.Count() > 1)
					{
						InstallationPath = s_split[1];
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
