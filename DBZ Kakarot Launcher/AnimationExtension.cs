using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DBZ_Kakarot_Launcher
{
	public static class ProgressBarExtensions
	{
		private static TimeSpan duration = TimeSpan.FromSeconds(1.0);

		public static void SetPercent(this ProgressBar progressBar, double percentage)
		{
			DoubleAnimation animation = new DoubleAnimation(percentage, duration);
			progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
		}
	}
}
