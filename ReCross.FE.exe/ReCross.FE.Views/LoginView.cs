using System.Windows;
using System.Windows.Markup;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class LoginView : UserControlFormBase, IComponentConnector
{
	public LoginView()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
	}
}
