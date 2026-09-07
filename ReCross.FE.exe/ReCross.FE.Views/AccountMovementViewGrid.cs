using System.Windows;
using System.Windows.Markup;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class AccountMovementViewGrid : UserControlGridBase, IComponentConnector, IStyleConnector
{
	public AccountMovementViewGrid()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
	}
}
