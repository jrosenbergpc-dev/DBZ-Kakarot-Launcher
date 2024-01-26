using DBZK_Core.Models;
using DBZK_Core.Tools;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DBZK_GUI
{
	/// <summary>
	/// Interaction logic for OptionsListItem.xaml
	/// </summary>
	public partial class OptionsListItem : UserControl
	{
		private bool b_Selected = false;
		private string m_Text = string.Empty;
		private bool b_enabled = false;

		public OptionsListItem()
		{
			InitializeComponent();
		}

		public event MouseButtonEventHandler OnClick;

		[Category("Common")]
		[Description("Sets the Text for the Option Label.")]
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

		[Category("Common")]
		[Description("Sets the status of Mod Enabled/Disabled.")]
		public bool IsModEnabled
		{
			get
			{
				return b_enabled;
			}
			set
			{
				b_enabled = value;

				Enabled_Chk.IsChecked = b_enabled;
			}
		}

		public Mod ModPackage { get; set; }

		public bool IsSelected
		{
			get
			{
				return b_Selected;
			}
			set
			{
				b_Selected = value;

				//immediate return to normal state on select change!
				//only if false!!
				if (b_Selected == false)
				{
					BrushConverter bc = new BrushConverter();
					this.Background = (Brush)bc.ConvertFromString("#FFB3550F");
					this.OptionLabel.Foreground = (Brush)bc.ConvertFromString("#FFDCDCDC");

					TopHighlight.Visibility = System.Windows.Visibility.Collapsed;
				}
				else
				{
					BrushConverter bc = new BrushConverter();
					this.Background = (Brush)bc.ConvertFromString("#FFFEE63B");
					this.OptionLabel.Foreground = (Brush)bc.ConvertFromString("#FF8E794A");

					TopHighlight.Visibility = System.Windows.Visibility.Visible;
				}
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
			if (IsSelected == false)
			{
				BrushConverter bc = new BrushConverter();
				this.Background = (Brush)bc.ConvertFromString("#FFB3550F");
				this.OptionLabel.Foreground = (Brush)bc.ConvertFromString("#FFDCDCDC");

				TopHighlight.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		private void Enabled_Chk_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			
		}

		private void Enabled_Chk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (ModPackage != null)
			{
				if (Enabled_Chk.IsChecked == true)
				{
					new ModHandler().EnableMod(ModPackage);
				}
				else
				{
					new ModHandler().DisableMod(ModPackage);
				}
			}
		}
	}
}
