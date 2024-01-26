using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace DBZK_Core.Tools
{
	public static class DownloadHandler
	{
		public static async Task DownloadFile(string url, string pathToSave)
		{
			var httpClient = new HttpClient();
			var responseStream = await httpClient.GetStreamAsync(url);
			using var fileStream = new FileStream(pathToSave, FileMode.Create);
			responseStream.CopyTo(fileStream);
		}
	}
}
