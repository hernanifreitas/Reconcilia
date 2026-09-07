using System.Windows;
using System.Windows.Markup;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class ReconciliationResultViewGrid : UserControlGridBase, IComponentConnector, IStyleConnector
{
	public ReconciliationResultViewGrid()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
	}
}
