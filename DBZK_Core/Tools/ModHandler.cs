using DBZK_Core.Models;
using DBZK_Core.Settings;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.IO.Compression;
using SevenZipExtractor;

namespace DBZK_Core.Tools
{
	public class ModHandler : IDisposable
	{
		private FileHandler GetFileHandler() => new FileHandler();
		private bool disposedValue;

		/// <summary>
		/// Install Mod Package to Mod Directory
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void InstallMod(Mod modfile)
		{
			GetFileHandler().CopyFile(modfile.FilePath, DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder);
			string orig_filename = Path.GetFileName(modfile.FilePath);
			modfile.FilePath = DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder + "\\" + orig_filename;
		}

		/// <summary>
		/// Install Mod Package to Mod Directory from File
		/// </summary>
		/// <param name="filename"></param>
		public void InstallMod(string filename)
		{
			if (Path.GetExtension(filename) == ".pak")
			{
				GetFileHandler().CopyFile(filename, DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder);
			}
			else if (Path.GetExtension(filename) == ".zip")
			{
				bool bContainsPak = false;

				ZipFile.OpenRead(filename).Entries.ToList().ForEach(entry =>
				{
					if (entry.FullName.EndsWith(".pak"))
					{
						bContainsPak = true;
					}
				});

				if (bContainsPak)
				{
					try
					{
						ZipFile.ExtractToDirectory(filename, DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder, true);
					}
					catch (Exception)
					{
					}
				}
			}
			else if (Path.GetExtension(filename) == ".rar")
			{
				bool bContainsPak = false;

				using (ArchiveFile rarfile = new ArchiveFile(filename))
				{
					rarfile.Entries.ToList().ForEach(entry =>
					{
						if (entry.FileName.EndsWith(".pak"))
						{
							bContainsPak = true;
						}
					});
				}

				if (bContainsPak)
				{
					try
					{
						using (ArchiveFile rarfile = new ArchiveFile(filename))
						{
							rarfile.Extract(DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder, true);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			else
			{
				//not allowed!
			}
		}

		/// <summary>
		/// Uninstalls Mod Package from Mod Directory
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void UninstallMod(Mod modfile)
		{
			GetFileHandler().DeleteFile(modfile.FilePath);
		}

		/// <summary>
		/// Enable a Mod for the Game
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void EnableMod(Mod modfile)
		{
			modfile.IsEnabled = true;
			GetFileHandler().CopyFile(DefaultConfig.InstallationPath + "\\" + DefaultConfig.DisableFolder + "\\" + Path.GetFileName(modfile.FilePath), DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder);
			GetFileHandler().DeleteFile(DefaultConfig.InstallationPath + "\\" + DefaultConfig.DisableFolder + "\\" + Path.GetFileName(modfile.FilePath));
		}

		/// <summary>
		/// Disable a Mod for the Game
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void DisableMod(Mod modfile)
		{
			modfile.IsEnabled = false;
			GetFileHandler().CopyFile(modfile.FilePath, DefaultConfig.InstallationPath + "\\" + DefaultConfig.DisableFolder);
			GetFileHandler().DeleteFile(modfile.FilePath);
		}

		/// <summary>
		/// Create Mod Package from File Path
		/// </summary>
		/// <param name="file">Full Path to file</param>
		/// <returns></returns>
		public Mod CreateMod(string file)
		{
			Mod? newMod = null;

			if (Path.GetExtension(file) == "pak")
			{
				newMod = new Mod()
				{
					FilePath = file,
					IsEnabled = true,
					Name = Path.GetFileName(file),
					Version = "1.0"
				};
			}

			return newMod;
		}

		public List<Mod> GetMods()
		{
			List<Mod> mods = new List<Mod>();

			if (Directory.Exists(DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder))
			{
				Directory.GetFiles(DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder).ToList().ForEach(file =>
				{
					Mod newMod = new Mod()
					{
						FilePath = file,
						IsEnabled = true,
						Name = Path.GetFileNameWithoutExtension(file),
						Version = "1.0"
					};

					mods.Add(newMod);
				});
			}

			if (Directory.Exists(DefaultConfig.InstallationPath + "\\" + DefaultConfig.DisableFolder))
			{
				Directory.GetFiles(DefaultConfig.InstallationPath + "\\" + DefaultConfig.DisableFolder).ToList().ForEach(file =>
				{
					Mod newMod = new Mod()
					{
						FilePath = file,
						IsEnabled = false,
						Name = Path.GetFileNameWithoutExtension(file),
						Version = "1.0"
					};

					mods.Add(newMod);
				});
			}

			mods = mods.OrderBy(mod => mod.Name).ToList();

			return mods;
		}

		public void LaunchGame()
		{
			ProcessStartInfo xInfo = new ProcessStartInfo()
			{
				UseShellExecute = true,
				FileName = "steam://rungameid/851850"
			};

			Process.Start(xInfo);
		}

		public void LaunchModViewer()
		{
			string arg = "-path=" + "\"" + DefaultConfig.InstallationPath + "\\" + DefaultConfig.ModFolder + "\"";

			ProcessStartInfo xInfo = new ProcessStartInfo()
			{	
				FileName = "umodel.exe",
				Arguments = arg
			};

			Process.Start(xInfo);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~ModHandler()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
