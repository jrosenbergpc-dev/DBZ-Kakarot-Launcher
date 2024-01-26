using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBZK_GUI
{
	/// <summary>
	/// Interaction logic for CloseButton.xaml
	/// </summary>
	public partial class CloseButton : UserControl
	{
		public CloseButton()
		{
			InitializeComponent();
		}

		public event MouseButtonEventHandler OnClick;

		private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			OnClick?.Invoke(this, e);
		}
	}
}
