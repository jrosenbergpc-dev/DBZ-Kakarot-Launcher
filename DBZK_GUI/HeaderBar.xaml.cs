using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace DBZK_GUI
{
	/// <summary>
	/// Interaction logic for HeaderBar.xaml
	/// </summary>
	public partial class HeaderBar : UserControl
	{
		private string m_Text = string.Empty;

		public HeaderBar()
		{
			InitializeComponent();
		}

		[Category("Common")]
		[Description("Sets the Text Displayed in the Header Banner")]
		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;

				HeaderLabel.Content = m_Text;
			}
		}
	}
}
