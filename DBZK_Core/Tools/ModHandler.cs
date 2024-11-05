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
using DBZK_Core.Games;

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
		//public void InstallMod(Mod modfile)
		//{
		//	GetFileHandler().CopyFile(modfile.FilePath, DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder);
		//	string orig_filename = Path.GetFileName(modfile.FilePath);
		//	modfile.FilePath = DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder + "\\" + orig_filename;
		//}

		/// <summary>
		/// Install Mod Package to Mod Directory from File
		/// </summary>
		/// <param name="filename"></param>
		public void InstallMod(string filename)
		{
			if (Path.GetExtension(filename) == ".pak")
			{
				GetFileHandler().CopyFile(filename, DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder);
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
						ZipFile.ExtractToDirectory(filename, DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder, true);
					}
					catch (Exception)
					{
					}
				}
			}
			else if (Path.GetExtension(filename) == ".rar")
			{
				bool bContainsPak = false;
				bool bContainsModFolder = false;

				using (ArchiveFile rarfile = new ArchiveFile(filename))
				{
					rarfile.Entries.ToList().ForEach(entry =>
					{
						if (entry.FileName.EndsWith(".pak"))
						{
							bContainsPak = true;
						}
						if (entry.FileName.Contains("~mods"))
						{
							bContainsModFolder = true;
						}
					});
				}

				if (bContainsPak)
				{
					if (bContainsModFolder)
					{
						try
						{
							using (ArchiveFile rarfile = new ArchiveFile(filename))
							{
								rarfile.Extract(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder.Replace("~mods", ""), true);
							}
						}
						catch (Exception)
						{
						}
					}
					else
					{
						try
						{
							using (ArchiveFile rarfile = new ArchiveFile(filename))
							{
								rarfile.Extract(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder, true);
							}
						}
						catch (Exception)
						{
						}
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
			modfile.FilePaths.ForEach(file =>
			{
				GetFileHandler().DeleteFile(file);
			});
		}

		/// <summary>
		/// Enable a Mod for the Game
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void EnableMod(Mod modfile)
		{
			modfile.IsEnabled = true;

			if (modfile.IsDirectory)
			{
				modfile.FilePaths.ForEach(file =>
				{
                    //GetFileHandler().MoveDirectory(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder + "\\");
                });
			}
			else if (modfile.IsPakFile)
			{
				modfile.FilePaths.ForEach(file =>
				{
					GetFileHandler().CopyFile(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder + "\\" + Path.GetFileName(file), DefaultConfig.SelectedVideoGame.InstallationPath + "\\" + DefaultConfig.SelectedVideoGame.ModFolder);
					GetFileHandler().DeleteFile(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder + "\\" + Path.GetFileName(file));
				});
			}
		}

		/// <summary>
		/// Disable a Mod for the Game
		/// </summary>
		/// <param name="modfile">Mod Package File</param>
		public void DisableMod(Mod modfile)
		{
			modfile.IsEnabled = false;
			if (modfile.IsDirectory)
			{

			}
			else if (modfile.IsPakFile)
			{
				modfile.FilePaths.ForEach(file =>
				{
					GetFileHandler().CopyFile(file, DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder);
					GetFileHandler().DeleteFile(file);
				});
			}
		}

		public List<Mod> GetMods()
		{
			List<Mod> mods = new List<Mod>();

			if (Directory.Exists(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder))
			{
				Directory.GetFileSystemEntries(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder).ToList().ForEach(file =>
				{
					if(Path.GetExtension(file) == "")
					{
						//must be a Directory!
						Mod newMod = new Mod()
						{
							IsEnabled = true,
							IsDirectory = true,
							Name = Path.GetFileNameWithoutExtension(file),
							Version = "1.0"
						};

						newMod.FilePaths.Add(file);

						mods.Add(newMod);
					}

					if (Path.GetExtension(file) == ".pak")
					{
						string PreviousPakFileName = Path.GetFileNameWithoutExtension(file);

						Mod newMod = new Mod()
						{
							IsEnabled = true,
							IsPakFile = true,
							Name = Path.GetFileNameWithoutExtension(file),
							Version = "1.0"
						};

						newMod.FilePaths.Add(file);

						//search for matching ucas and utoc files!
						List<string> search = Directory.GetFiles(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder).ToList();

						search.Where(xfile => Path.GetFileNameWithoutExtension(xfile) == PreviousPakFileName).ToList().ForEach(extrafile =>
						{
							if (Path.GetExtension(extrafile) == ".ucas" || Path.GetExtension(extrafile) == ".utoc")
							{
								newMod.FilePaths.Add(extrafile);
							}
						});

						mods.Add(newMod);
					}
				});
			}

			if (Directory.Exists(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder))
			{
				Directory.GetFileSystemEntries(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder).ToList().ForEach(file =>
				{
					if (Path.GetExtension(file) == "")
					{
						//must be a Directory!
						Mod newMod = new Mod()
						{
							IsEnabled = true,
							IsDirectory = false,
							Name = Path.GetFileNameWithoutExtension(file),
							Version = "1.0"
						};

						newMod.FilePaths.Add(file);

						mods.Add(newMod);
					}

					if (Path.GetExtension(file) == ".pak")
					{
						string PreviousPakFileName = Path.GetFileNameWithoutExtension(file);

						Mod newMod = new Mod()
						{
							IsEnabled = false,
							IsPakFile = true,
							Name = Path.GetFileNameWithoutExtension(file),
							Version = "1.0"
						};

						newMod.FilePaths.Add(file);

						//search for matching ucas and utoc files!
						List<string> search = Directory.GetFiles(DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.DisableFolder).ToList();

						search.Where(xfile => Path.GetFileNameWithoutExtension(xfile) == PreviousPakFileName).ToList().ForEach(extrafile =>
						{
							if (Path.GetExtension(extrafile) == ".ucas" || Path.GetExtension(extrafile) == ".utoc")
							{
								newMod.FilePaths.Add(extrafile);
							}
						});

						mods.Add(newMod);
					}
				});
			}

			mods = mods.OrderBy(mod => mod.Name).ToList();

			return mods;
		}

		public void LaunchGame()
		{
			if (DefaultConfig.SelectedVideoGame is DBZKakarot)
			{
				ProcessStartInfo xInfo = new ProcessStartInfo()
				{
					UseShellExecute = true,
					FileName = "steam://rungameid/851850"
				};

				Process.Start(xInfo);
			}
			else if (DefaultConfig.SelectedVideoGame is DBZSparkingZero)
			{
				ProcessStartInfo xInfo = new ProcessStartInfo()
				{
					UseShellExecute = true,
					FileName = "steam://rungameid/1790600"
				};

				Process.Start(xInfo);
			}
		}

		public void LaunchModViewer()
		{
			string arg = "-path=" + "\"" + DefaultConfig.SelectedVideoGame.InstallationPath + DefaultConfig.SelectedVideoGame.ModFolder + "\"";

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
