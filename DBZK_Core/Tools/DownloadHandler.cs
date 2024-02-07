using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DBZK_Core.Tools
{
	public static class DownloadHandler
	{
		private const string ApiKey = "ZbsgScbun+WHwELDZf14FfAJx4FWlAZTZ5RfvBzdsE1XrQ==--5C2xag1a4RWjL48n--Wx64wuBjQkSwTBrpSb5PkQ==";
		private const string ApiUrlPrefix = "https://api.nexusmods.com/v1/games/dragonballzkakarot/mods/";

		public static async Task<string> GetModFromNexus(string url, string downloadDirectory)
		{
			string modfilename = String.Empty;
			string apiUrl = string.Empty;

			// Break down nxmLink here
			if (url.Contains("dragonballzkakarot"))
			{
				// This is a proper mod link for the right game!
				Uri uri = new Uri(url);

				string modId = GetModIdFromUrl(url);

				string fileNumber = uri.Segments[^1].TrimEnd('/');

				NameValueCollection queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

				string key = queryParameters["key"];
				string expires = queryParameters["expires"];

				apiUrl = $"{ApiUrlPrefix}{modId}/files/{fileNumber}/download_link.json?key={key}&expires={expires}";
			}

			//MessageBox.Show("Got URL: " + apiUrl);

			if (!string.IsNullOrEmpty(apiUrl))
			{
				string downloadUrl = await GetDownloadUrl(apiUrl);

				if (downloadUrl != null)
				{
					modfilename = await DownloadFile(downloadUrl, downloadDirectory);
					Console.WriteLine("File downloaded successfully.");
				}
				else
				{
					Console.WriteLine("Failed to get download URL.");
				}
			}

			//MessageBox.Show("Downloaded: " + modfilename);

			return modfilename;
		}

		internal static string GetModIdFromUrl(string url)
		{
			// Extract mod ID from the URL
			Uri uri = new Uri(url);
			string[] segments = uri.Segments;
			string modIdSegment = segments[Array.IndexOf(segments, "mods/") + 1]; // Get the segment next to "mods/"
			string modId = modIdSegment.TrimEnd('/'); // Remove trailing slash if present
			return modId;
		}

		internal static async Task<string> GetDownloadUrl(string apiUrl)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Add("apikey", ApiKey);

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					var jsonDocument = JsonDocument.Parse(responseBody);

					var uriElement = jsonDocument.RootElement[0].GetProperty("URI");

					return uriElement.ToString();
				}
				else
				{
					Console.WriteLine($"Failed to get download URL. Status code: {response.StatusCode}");
					return null;
				}
			}
		}

		internal static async Task<string> DownloadFile(string url, string downloadDirectory)
		{
			using (HttpClient client = new HttpClient())
			using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
			{
				if (response.IsSuccessStatusCode)
				{
					//MessageBox.Show("Download Success!");

					string filename = GetFilenameFromUrl(url);

					//MessageBox.Show("Saving download with filename: " + downloadDirectory + "/" + filename);

					using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
					{
						using (Stream streamToWriteTo = File.Open(Path.Combine(downloadDirectory, filename), FileMode.Create))
						{
							await streamToReadFrom.CopyToAsync(streamToWriteTo);
						}
					}

					Console.WriteLine($"File '{filename}' downloaded successfully.");
					return filename;
				}
				else
				{
					Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
					return string.Empty;
				}
			}
		}

		private static string GetFilenameFromUrl(string url)
		{
			// Extract filename from the URL
			Uri uri = new Uri(url);
			string[] segments = uri.Segments;
			string lastSegment = segments[segments.Length - 1]; // Get the last segment

			// Remove query string
			string filename = lastSegment.Split('?')[0];

			// If the filename contains spaces, ensure it's properly extracted
			if (filename.Contains(" "))
			{
				// Find the last occurrence of the slash character to locate the start of the filename
				int lastSlashIndex = url.LastIndexOf('/');
				filename = url.Substring(lastSlashIndex + 1);

				// Remove any query parameters if present
				int questionMarkIndex = filename.IndexOf('?');
				if (questionMarkIndex != -1)
				{
					filename = filename.Substring(0, questionMarkIndex);
				}
			}

			// Decode any URL encoding in the filename
			filename = Uri.UnescapeDataString(filename);

			return filename;
		}
	}
}
