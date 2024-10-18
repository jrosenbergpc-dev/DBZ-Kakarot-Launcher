using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace DBZK_GUI
{
	/// <summary>
	/// Interaction logic for Button.xaml
	/// </summary>
	public partial class Button : UserControl
	{
		private string m_Text = string.Empty;

		public Button()
		{
			InitializeComponent();
		}

		public event MouseButtonEventHandler OnClick;

		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;

				OptionLabel.Content = m_Text;
			}
		}

		private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			ButtonBG.Background = null;
			OnClick?.Invoke(sender, e);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			BrushConverter bc = new BrushConverter();
			ButtonBG.Background = (Brush)bc.ConvertFromString("#CCFFEA00");
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			BrushConverter bc = new BrushConverter();
			ButtonBG.Background = null;
		}
	}
}
