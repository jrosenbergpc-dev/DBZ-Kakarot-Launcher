using System;
using System.Windows;
using System.Windows.Input;

namespace DBZ_Kakarot_Launcher
{
	/// <summary>
	/// Interaction logic for AlertDialog.xaml
	/// </summary>
	public partial class AlertDialog : Window
	{
		private string m_Header = string.Empty;
		private string m_Message = string.Empty;
		private AlertDialogResult m_Result = AlertDialogResult.NONE;

		public AlertDialog()
		{
			InitializeComponent();
		}

		public AlertDialog(string header, string message)
		{
			InitializeComponent();
			Header = header;
			Message = message;
		}

		public string Header
		{
			get
			{ 
				return m_Header; 
			}
			set
			{
				m_Header = value;

				Header_Lbl.Text = m_Header;
			}
		}

		public string Message
		{
			get
			{
				return m_Message;
			}
			set
			{
				m_Message = value;

				Message_Text_Block.Text = m_Message;
			}
		}

		public AlertDialogResult Result
		{
			get
			{
				return m_Result;
			}
		}

		private void CloseButton_OnClick(object sender, MouseButtonEventArgs e)
		{
			m_Result = AlertDialogResult.NO;
			Close();
        }

		private void Yes_Btn_OnClick(object sender, MouseButtonEventArgs e)
		{
			m_Result = AlertDialogResult.YES;
			Close();
		}

		private void No_Btn_OnClick(object sender, MouseButtonEventArgs e)
		{
			m_Result = AlertDialogResult.NO;
			Close();
		}
	}
}
