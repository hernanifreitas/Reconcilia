using System.Windows;
using System.Windows.Markup;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class BankMovementViewGrid : UserControlGridBase, IComponentConnector, IStyleConnector
{
	public BankMovementViewGrid()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
	}
}
