using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DBZ_Kakarot_Launcher
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static string nxmLink = string.Empty;

		protected override void OnStartup(StartupEventArgs e)
		{
			if (e.Args.Length > 0)
			{
				e.Args.ToList().ForEach(arg =>
				{
					if (arg.StartsWith("nxm://"))
					{
						//we got ourselves an mod package to download!
						nxmLink = arg;
					}
					else
					{
						Environment.Exit(0);
					}
				});
			}
		}
	}
}
