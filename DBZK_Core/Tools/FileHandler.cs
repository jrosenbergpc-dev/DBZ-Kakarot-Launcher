using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DBZK_Core.Models;
using DBZK_Core.Settings;
using DBZK_Core.Games;

namespace DBZK_Core.Tools
{
	/// <summary>
	/// Handles File Interactions for Mod Packages 
	/// </summary>
	public class FileHandler : IDisposable
	{
		private bool disposedValue;

		/// <summary>
		/// Copies a file from it's location to another folder location and retains it's filename along the way.
		/// </summary>
		/// <param name="file">Full Path to File to Copy</param>
		/// <param name="destination">Destination Folder Path to copy the file to.</param>
		public void CopyFile(string file, string destination)
		{
			try
			{
				if (File.Exists(file))
				{
					if (Directory.Exists(destination))
					{
						File.Copy(file, destination + "\\" + Path.GetFileName(file));
					}
					else
					{
						CreateFolder(destination);

						File.Copy(file, destination + "\\" + Path.GetFileName(file));
					}
				}
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// Attempts to Delete a file.
		/// </summary>
		/// <param name="file">Full Path to File to Delete</param>
		public void DeleteFile(string file) 
		{
			try
			{
				if(File.Exists(file))
				{
					File.Delete(file);
				}
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// Creates a Directory based onpath supplied.
		/// </summary>
		/// <param name="path">Full Path to File</param>
		public void CreateFolder(string path)
		{
			try
			{
				if (Directory.Exists(path) == false)
				{
					Directory.CreateDirectory(path);
				}
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// Creates a blank empty text based file in directory specified
		/// </summary>
		/// <param name="path">Full path to File including filename</param>
		public void CreateEmptyFile(string path)
		{
			if (File.Exists(path) == false)
			{
				File.WriteAllText(path, string.Empty);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		public void WriteToFile(string path, string contents)
		{
			if (File.Exists(path))
			{
				File.WriteAllText(path, contents);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		public void WriteToFile(string path, List<string> contents)
		{
			string content = String.Join(Environment.NewLine, contents.ToArray());

			File.WriteAllText(path, content);
		}

		public string ReadAllContentFromFile(string path)
		{
			string content = string.Empty;

			if (File.Exists(path))
			{
				content= File.ReadAllText(path);
			}

			return content;
		}

		public List<string> ReadAllContentsFromFile(string path)
		{
			List<string> contents = new List<string>();

			if (File.Exists(path))
			{
				contents = File.ReadAllLines(path).ToList();
			}

			return contents;
		}

		/// <summary>
		/// Helps locate the Dragonball Z Kakarot™ Game Installation Directory!
		/// </summary>
		/// <returns></returns>
		public string LocateGameDirectory()
		{
			string gamedir = string.Empty;

			Directory.GetLogicalDrives().ToList().ForEach(drive =>
			{
				if (DefaultConfig.SelectedVideoGame is DBZKakarot)
				{
					string gamepath = "Program Files (x86)\\Steam\\steamapps\\common\\DRAGON BALL Z KAKAROT";
					string alt_gamepath = "SteamLibrary\\steamapps\\common\\DRAGON BALL Z KAKAROT";

					if (Directory.Exists(drive + gamepath))
					{
						gamedir = drive + gamepath;
					}
					else if (Directory.Exists(drive + alt_gamepath))
					{
						gamedir = drive + alt_gamepath;
					}
				}
				else if (DefaultConfig.SelectedVideoGame is DBZSparkingZero)
				{
					string gamepath = "Program Files (x86)\\Steam\\steamapps\\common\\DRAGON BALL Sparking! ZERO";
					string alt_gamepath = "SteamLibrary\\steamapps\\common\\DRAGON BALL Sparking! ZERO";

					if (Directory.Exists(drive + gamepath))
					{
						gamedir = drive + gamepath;
					}
					else if (Directory.Exists(drive + alt_gamepath))
					{
						gamedir = drive + alt_gamepath;
					}
				}
			});

			return gamedir;
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
		// ~FileHandler()
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
