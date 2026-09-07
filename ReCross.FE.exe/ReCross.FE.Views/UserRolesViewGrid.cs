using System.Windows;
using System.Windows.Markup;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class UserRolesViewGrid : UserControlGridBase, IComponentConnector, IStyleConnector
{
	public UserRolesViewGrid()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
	}
}
