using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ReCross.FE.Properties;
using Strills.WPF.UI.Controls.Internal;
using Strills.WPF.UI.UserControls;

namespace ReCross.FE.Views;

public partial class AppConfigurationView : UserControlFormBase, IComponentConnector
{
	private bool _hasException = false;

	public AppConfigurationView()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		CBUseConfigFile.IsEnabled = TBConfigFile.Text.ToLower().EndsWith(".txt");
		TBDataSource.IsEnabled = !CBUseConfigFile.IsEnabled;
		TBUsername.IsEnabled = !CBUseConfigFile.IsEnabled;
		PBPassword.IsEnabled = !CBUseConfigFile.IsEnabled;
		TBRecDocPath.IsEnabled = !CBUseConfigFile.IsEnabled;
		TBCompanyName.IsEnabled = !CBUseConfigFile.IsEnabled;
		TBCompanyLogo.IsEnabled = !CBUseConfigFile.IsEnabled;
		TBCompanyFiscalNumber.IsEnabled = !CBUseConfigFile.IsEnabled;
		CBAutoLinkEnabled.IsEnabled = !CBUseConfigFile.IsEnabled;
	}

	private void TBConfigFile_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (CBUseConfigFile != null)
		{
			CBUseConfigFile.IsChecked = false;
			CBUseConfigFile.IsEnabled = TBConfigFile.Text.ToLower().EndsWith(".txt");
		}
	}

	private void CBUseConfigFile_Checked(object sender, RoutedEventArgs e)
	{
		if (TBDataSource == null)
		{
			return;
		}
		string text = TBConfigFile.Text;
		try
		{
			StreamReader streamReader = File.OpenText(text);
			string text2 = streamReader.ReadLine();
			while (text2 != null && !string.IsNullOrEmpty(text2.Trim()))
			{
				string[] array = text2.Split('=');
				array[0] = array[0].TrimStart();
				array[0] = array[0].TrimEnd();
				array[1] = array[1].TrimStart();
				array[1] = array[1].TrimEnd();
				switch (array[0].Trim())
				{
				case "DataSource":
					TBDataSource.Text = array[1];
					break;
				case "Username":
					TBUsername.Text = array[1];
					break;
				case "Password":
					PasswordBoxAssistant.SetBindPassword(PBPassword, value: true);
					PasswordBoxAssistant.SetBoundPassword(PBPassword, array[1]);
					break;
				case "RecDocPath":
					TBRecDocPath.Text = array[1];
					break;
				case "CompanyName":
					TBCompanyName.Text = array[1];
					break;
				case "CompanyLogo":
					TBCompanyLogo.Text = array[1];
					break;
				case "CompanyFiscalNumber":
					TBCompanyFiscalNumber.Text = array[1];
					break;
				case "AutoLinkEnabled":
					CBAutoLinkEnabled.IsChecked = bool.Parse(array[1]);
					break;
				}
				text2 = streamReader.ReadLine();
			}
			DateTime lastWriteTime = File.GetLastWriteTime(text);
			Settings.Default.ConfigFileLastModify = lastWriteTime;
			TBDataSource.IsEnabled = false;
			TBUsername.IsEnabled = false;
			PBPassword.IsEnabled = false;
			TBRecDocPath.IsEnabled = false;
			TBCompanyName.IsEnabled = false;
			TBCompanyLogo.IsEnabled = false;
			TBCompanyFiscalNumber.IsEnabled = false;
			CBAutoLinkEnabled.IsEnabled = false;
		}
		catch (Exception ex)
		{
			_hasException = true;
			CBUseConfigFile.IsChecked = false;
			MessageBox.Show("Erro ao abrir o ficheiro de configuração do Reconcilia '" + text + "':\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void CBUseConfigFile_Unchecked(object sender, RoutedEventArgs e)
	{
		if (TBDataSource == null)
		{
			return;
		}
		TBDataSource.IsEnabled = true;
		TBUsername.IsEnabled = true;
		PBPassword.IsEnabled = true;
		TBRecDocPath.IsEnabled = true;
		TBCompanyName.IsEnabled = true;
		TBCompanyLogo.IsEnabled = true;
		TBCompanyFiscalNumber.IsEnabled = true;
		CBAutoLinkEnabled.IsEnabled = true;
		if (!_hasException)
		{
			try
			{
				TBDataSource.Text = Settings.Default.GetPreviousVersion("DataSource") as string;
				TBUsername.Text = Settings.Default.GetPreviousVersion("Username") as string;
				PasswordBoxAssistant.SetBindPassword(PBPassword, value: true);
				PasswordBoxAssistant.SetBoundPassword(PBPassword, Settings.Default.GetPreviousVersion("Password") as string);
				TBRecDocPath.Text = Settings.Default.GetPreviousVersion("RecDocPath") as string;
				TBCompanyName.Text = Settings.Default.GetPreviousVersion("CompanyName") as string;
				TBCompanyLogo.Text = Settings.Default.GetPreviousVersion("CompanyLogo") as string;
				TBCompanyFiscalNumber.Text = Settings.Default.GetPreviousVersion("CompanyFiscalNumber") as string;
				CBAutoLinkEnabled.IsChecked = (bool)Settings.Default.GetPreviousVersion("AutoLinkEnabled");
				return;
			}
			catch
			{
				return;
			}
		}
		_hasException = false;
	}
}
