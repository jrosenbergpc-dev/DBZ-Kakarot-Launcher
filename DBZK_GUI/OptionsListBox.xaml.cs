using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace DBZK_GUI
{
	/// <summary>
	/// Interaction logic for OptionsListBox.xaml
	/// </summary>
	public partial class OptionsListBox : UserControl
	{
		private ObservableCollection<OptionsListItem> optionsListItems = new ObservableCollection<OptionsListItem>();

		private OptionsListItem? m_SelectedItem;

		public OptionsListBox()
		{
			InitializeComponent();

			optionsListItems.CollectionChanged += OptionsListItems_CollectionChanged;
		}

		public event EventHandler<OptionsListItem> SelectedChanged;

		public OptionsListItem? SelectedItem
		{
			get
			{
				return m_SelectedItem;
			}
		}

		private void OptionsListItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				if (e.NewItems.Count > 0)
				{
					foreach(var item in e.NewItems)
					{
						if (item is OptionsListItem)
						{
							OptionContainer.Children.Add(item as OptionsListItem);
							(item as OptionsListItem).OnClick += OptionsListBox_OnClick;
						}
					}
				}
			}
			else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
			{
				m_SelectedItem = null;
				OptionContainer.Children.Clear();
			}
		}

		private void OptionsListBox_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			m_SelectedItem = sender as OptionsListItem;

			foreach(OptionsListItem item in optionsListItems)
			{
				item.IsSelected = false;
			}

			m_SelectedItem.IsSelected = true;

			SelectedChanged?.Invoke(sender, m_SelectedItem);
		}

		[Category("Common")]
		[Description("List of Options to Show in the List Box.")]
		public ObservableCollection<OptionsListItem> OptionsListItems
		{
			get { return optionsListItems; }
		}
	}
}
