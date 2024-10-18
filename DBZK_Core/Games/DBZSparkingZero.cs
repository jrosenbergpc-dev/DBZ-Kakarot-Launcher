using DBZK_Core.Interfaces;
using DBZK_Core.Settings;
using DBZK_Core.Tools;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DBZK_Core.Games
{
    public class DBZSparkingZero : IVideoGame
    {
        public string Name { get; set; } = "Dragonball Z: Sparking Zero!";
		public string Logo { get; set; } = "/Images/dragon_ball_sparking__zero_logo.png";
		public string InstallationPath { get; set; }
		public string ModFolder { get; set; } = "\\SparkingZERO\\Content\\Paks\\~mods";
		public string DisableFolder { get; set; } = "\\SparkingZERO\\Content\\Paks\\~disable";
		public bool ModPatchRequired { get; set; } = true;
        public string Version { get; set; } = "5.1.1.0";

        public void InstallPatch()
        {
            FileHandler fileHandler = new FileHandler();

            if (fileHandler.DirectoryExists(InstallationPath + "\\SparkingZERO\\Binaries\\Win64"))
            {
				var assembly = Assembly.GetExecutingAssembly();
				string[] resourceNames = assembly.GetManifestResourceNames();

				Console.WriteLine("List of embedded resources:");
				foreach (string resourceName in resourceNames)
				{
					Console.WriteLine(resourceName);
				}

				WriteEmbeddedResourceToDisk("DBSparkingZeroModPatch.7z", DefaultConfig.RunningDirectory + "\\modpatch.7z");

				using (ArchiveFile rarfile = new ArchiveFile(DefaultConfig.RunningDirectory + "\\modpatch.7z"))
				{
					rarfile.Extract(InstallationPath + "\\SparkingZERO\\Binaries\\Win64", true);
				}
			}
        }

		private void WriteEmbeddedResourceToDisk(string resourceName, string outputFilePath)
		{
			// Get the current assembly
			Assembly assembly = Assembly.GetExecutingAssembly();

			// Format the resource name as it's stored in the assembly (Namespace + File)
			string resourceFullName = $"DBZK_Core.ModPatches.resources.{resourceName}";

			// Get the resource stream
			using (Stream resourceStream = assembly.GetManifestResourceStream(resourceFullName))
			{
				if (resourceStream == null)
				{
					throw new Exception($"Resource '{resourceFullName}' not found.");
				}

				// Write the resource stream to a file
				using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
				{
					resourceStream.CopyTo(fileStream);
				}
			}
		}
	}
}
