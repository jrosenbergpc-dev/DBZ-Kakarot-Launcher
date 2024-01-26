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
			OnClick?.Invoke(sender, e);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			BrushConverter bc = new BrushConverter();
			this.Background = (Brush)bc.ConvertFromString("#FFFEE63B");
			this.OptionLabel.Foreground = (Brush)bc.ConvertFromString("#FF8E794A");

			TopHighlight.Visibility = System.Windows.Visibility.Visible;
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			BrushConverter bc = new BrushConverter();
			this.Background = (Brush)bc.ConvertFromString("#FFB3550F");
			this.OptionLabel.Foreground = (Brush)bc.ConvertFromString("#FFDCDCDC");

			TopHighlight.Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
