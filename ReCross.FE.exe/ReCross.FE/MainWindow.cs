using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FluidKit.Controls;
using ReCross.BE.DataObjects;
using ReCross.FE.Properties;
using ReCross.FE.Utils;
using ReCross.FE.Views;
using Strills.WPF.Localization;

namespace ReCross.FE;

public partial class MainWindow : Window, IComponentConnector
{
	private const string C_Resx_MainWindow = "ReCross.FE.MainWindow";

	private const string C_Resx_Resources = "ReCross.FE.Properties.Resources";

	private int _previousSelectedIndex = 0;

	private MenuItemDataCollection _menuItemDataSource;

	private LayoutBase[] _layouts = new LayoutBase[10]
	{
		new Wall(),
		new SlideDeck(),
		new CoverFlow(),
		new Carousel(),
		new TimeMachine2(),
		new ThreeLane(),
		new VForm(),
		new TimeMachine(),
		new RollerCoaster(),
		new Rolodex()
	};

	private List<MenuItemEnum> _menuIndexes = new List<MenuItemEnum>();

	public MainWindow()
	{
		InitializeComponent();
		if (ReCrossApp.Current.IsAppConfigured)
		{
			SPContent.Children.Add(new LoginView());
		}
		else
		{
			SPContent.Children.Add(new AppConfigurationView());
		}
		base.Loaded += MainWindow_Loaded;
	}

	private void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		EFMenu.Visibility = Visibility.Hidden;
		ContentPresenter cPCompany = CPCompany;
		ContentPresenter cPBusinessTool = CPBusinessTool;
		Visibility visibility = (CPLogTool.Visibility = Visibility.Hidden);
		visibility = (cPBusinessTool.Visibility = visibility);
		cPCompany.Visibility = visibility;
		EFMenu.Layout = _layouts[6];
		LoadMenuItemData();
		EFMenu.SelectedIndexChanged += EFMenu_SelectedIndexChanged;
		EFMenu.SelectedIndex = 6;
	}

	private void EFMenu_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!ReCrossApp.Current.StateData.IsLoggedIn)
		{
			return;
		}
		if (ReCrossApp.Current.StateData.IsViewingMode)
		{
			ReCrossApp.Current.StateData.IsViewingMode = false;
		}
		if (ReCrossApp.Current.StateData.IsEditingMode)
		{
			(sender as ElementFlow).SelectedIndex = _previousSelectedIndex;
			return;
		}
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
		}
		SetCurrentScreen(sender as ElementFlow);
		_previousSelectedIndex = (sender as ElementFlow).SelectedIndex;
	}

	private void ChangeSelectedIndex(object sender, RoutedPropertyChangedEventArgs<double> args)
	{
		EFMenu.SelectedIndex = (int)args.NewValue;
	}

	private void BLogOut_Click(object sender, RoutedEventArgs e)
	{
		EndUp();
	}

	private void BConfiguration_Click(object sender, RoutedEventArgs e)
	{
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
		}
		SPContent.Children.Add(new AppConfigurationView());
	}

	private void BChangePassword_Click(object sender, RoutedEventArgs e)
	{
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
		}
		SPContent.Children.Add(new ChangePasswordView());
	}

	private void BChangeLanguage_Click(object sender, RoutedEventArgs e)
	{
		string text = CultureManager.UICulture.TwoLetterISOLanguageName.ToLower();
		text = ((text == "pt") ? "en" : "pt");
		CultureManager.UICulture = new CultureInfo(text);
		((Button)sender).Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ReCross.FE;component/Images/Language_" + text.ToUpper() + ".png")));
		LoadMenuItemData();
		EFMenu.UpdateLayout();
		UpdateLayout();
	}

	private void BAbout_Click(object sender, RoutedEventArgs e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Reconcilia - Reconciliações Bancárias");
		stringBuilder.AppendLine("Versão " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
		stringBuilder.AppendLine(string.Empty);
		stringBuilder.AppendLine("Licenciado a: ");
		stringBuilder.AppendLine("      " + Settings.Default.CompanyName);
		stringBuilder.AppendLine("Produzido por: ");
		stringBuilder.AppendLine("      CROSS Informática, Lda. 2013 - Todos os Direitos Reservados");
		stringBuilder.AppendLine(string.Empty);
		stringBuilder.AppendLine("...::: WWW.CROSS.PT :::...");
		MessageBox.Show(stringBuilder.ToString(), "Informação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	private void MI_Exit_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void LoadMenuItemData()
	{
		_menuItemDataSource = FindResource("menuItemDataSource") as MenuItemDataCollection;
		_menuItemDataSource.Clear();
		_menuIndexes.Clear();
		foreach (MenuItemEnum value in Enum.GetValues(typeof(MenuItemEnum)))
		{
			string key = value.ToString().ToLower();
			if (!ReCrossApp.Current.StateData.IsLoggedIn)
			{
				_menuIndexes.Add(value);
				_menuItemDataSource.Add(new MenuItemData(ResourcesManager.GetValue("ReCross.FE.Properties.Resources", "MenuItemEnum_" + value).ToUpper(), "/Images/M" + value.ToString() + ".jpg"));
			}
			else if (ReCrossApp.Current.UserGenericPermissions.ContainsKey(key) && ReCrossApp.Current.UserGenericPermissions[key].CanRead)
			{
				_menuIndexes.Add(value);
				_menuItemDataSource.Add(new MenuItemData(ResourcesManager.GetValue("ReCross.FE.Properties.Resources", "MenuItemEnum_" + value).ToUpper(), "/Images/M" + value.ToString() + ".jpg"));
			}
		}
	}

	private void SetCurrentScreen(ElementFlow elementFlow)
	{
		if (_menuIndexes.Count > 0)
		{
			switch (_menuIndexes[elementFlow.SelectedIndex])
			{
			case MenuItemEnum.Customers:
				SPContent.Children.Add(new CustomerView());
				break;
			case MenuItemEnum.BankAccounts:
				SPContent.Children.Add(new BankAccountView());
				break;
			case MenuItemEnum.AccountMovements:
				SPContent.Children.Add(new AccountMovementView());
				break;
			case MenuItemEnum.BankMovements:
				SPContent.Children.Add(new BankMovementView());
				break;
			case MenuItemEnum.Reconciliations:
				SPContent.Children.Add(new ReconciliationView());
				break;
			case MenuItemEnum.Search:
				SPContent.Children.Add(new SearchView());
				break;
			case MenuItemEnum.Parametrization:
				SPContent.Children.Add(new ParametrizationView());
				break;
			default:
				SPContent.Children.Add(null);
				break;
			}
		}
	}

	public void StartUp()
	{
		EFMenu.Visibility = Visibility.Visible;
		ContentPresenter cPCompany = CPCompany;
		ContentPresenter cPBusinessTool = CPBusinessTool;
		Visibility visibility = (CPLogTool.Visibility = Visibility.Visible);
		visibility = (cPBusinessTool.Visibility = visibility);
		cPCompany.Visibility = visibility;
		LCompanyName.Text = Settings.Default.CompanyName;
		LUserFullName.Text = ReCrossApp.Current.UserFullName;
		LoadMenuItemData();
		EFMenu.SelectedIndex = 0;
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
			if (_menuIndexes.Count == 0)
			{
				SPContent.Children.Add(new ChangePasswordView());
			}
		}
		SetCurrentScreen(EFMenu);
		_previousSelectedIndex = EFMenu.SelectedIndex;
	}

	public void EndUp()
	{
		EFMenu.Visibility = Visibility.Hidden;
		ContentPresenter cPCompany = CPCompany;
		ContentPresenter cPBusinessTool = CPBusinessTool;
		Visibility visibility = (CPLogTool.Visibility = Visibility.Hidden);
		visibility = (cPBusinessTool.Visibility = visibility);
		cPCompany.Visibility = visibility;
		EFMenu.SelectedIndex = EFMenu.Items.Count - 1;
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
		}
		SPContent.Children.Add(new LoginView());
		ReCrossApp.Current.CurrentDataContext.SaveChanges();
		ReCrossApp.Current.CurrentDataContext = new ReCrossEntities();
		ReCrossEntities.CurrentContext = ReCrossApp.Current.CurrentDataContext;
		ReCrossApp.Current.UserFullName = null;
		ReCrossApp.Current.UserId = -1L;
		ReCrossApp.Current.UserIsAdmin = false;
		ReCrossApp.Current.StateData.IsLoggedIn = false;
		ReCrossApp.Current.UserGenericPermissions.Clear();
		ReCrossApp.Current.UserRecordPermissions.Clear();
		ReCrossApp.Restart();
	}

	public void SetPreviousScreen()
	{
		if (SPContent.Children.Count == 1)
		{
			SPContent.Children.RemoveAt(0);
		}
		SetCurrentScreen(EFMenu);
	}
}
