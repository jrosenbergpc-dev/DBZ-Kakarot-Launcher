using System;
using System.Collections.Generic;

namespace DBZK_Core.Models
{
    public class Mod : IDisposable
    {
		private bool disposedValue;

		public string Name { get; set; }
        public List<string> FilePaths { get; set; } = new List<string>();
        public bool IsEnabled { get; set; }
		public bool IsDirectory { get; set; }
		public bool IsPakFile { get; set; }
		public string Version { get; set; }

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
		// ~Mod()
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
