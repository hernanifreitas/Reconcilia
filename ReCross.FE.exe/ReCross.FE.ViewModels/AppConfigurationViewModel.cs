using System;
using System.Windows;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using ReCross.FE.Properties;
using Strills.WPF.UI.Commands;
using Strills.WPF.UI.Configuration;

namespace ReCross.FE.ViewModels;

public class AppConfigurationViewModel
{
	private const string C_ReCrossBackup = "ReCrossBackup_{0}.bak";

	private RelayCommand _cmdUpdate;

	private RelayCommand _cmdCancel;

	private RelayCommand _cmdBackup;

	private RelayCommand _cmdRestore;

	public bool Enabled => !ReCrossApp.Current.StateData.IsEditingMode;

	public ICommand CMDUpdate => _cmdUpdate;

	public ICommand CMDCancel => _cmdCancel;

	public ICommand CMDBackup => _cmdBackup;

	public ICommand CMDRestore => _cmdRestore;

	public AppConfigurationViewModel()
	{
		Action<object> execute = delegate
		{
			Update();
		};
		_cmdUpdate = new RelayCommand(execute, (object param) => CanUpdate());
		_cmdCancel = new RelayCommand(delegate
		{
			Cancel();
		});
		_cmdBackup = new RelayCommand(delegate
		{
			Backup();
		}, (object param) => CanBakRest());
		_cmdRestore = new RelayCommand(delegate
		{
			Restore();
		}, (object param) => CanBakRest());
	}

	private bool CanUpdate()
	{
		return !string.IsNullOrEmpty(Settings.Default.Database) && (string.IsNullOrEmpty(Settings.Default.Username) || !string.IsNullOrEmpty(Settings.Default.Password)) && !string.IsNullOrEmpty(Settings.Default.CompanyName) && !string.IsNullOrEmpty(Settings.Default.CompanyFiscalNumber) && !string.IsNullOrEmpty(Settings.Default.RecDocPath);
	}

	private bool CanBakRest()
	{
		return ReCrossApp.Current.IsAppConfigured;
	}

	private bool Update()
	{
		bool result = true;
		string text = "";
		try
		{
		}
		catch (Exception ex)
		{
			MessageBox.Show("Conexão SQL à Base de Dados falhou!\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		text = ConfigManager.ChangeConnectionString("ReCrossEntities", Settings.Default.DataSource, "ReCross", Settings.Default.Username, Settings.Default.Password, "DataObjects.ReCross");
		try
		{
			ReCrossApp.Current.CurrentDataContext = new ReCrossEntities(text);
			ReCrossEntities.CurrentContext = ReCrossApp.Current.CurrentDataContext;
			ReCrossApp current = ReCrossApp.Current;
			bool isAppConfigured = (Settings.Default.IsAppConfigured = true);
			current.IsAppConfigured = isAppConfigured;
			Settings.Default.Save();
			ReCrossApp.Current.CompanyLogo = Settings.Default.CompanyLogo;
			MessageBox.Show("Dados actualizados com sucesso!\n\nA aplicação será reinicializada.", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			ReCrossApp.Restart();
		}
		catch (Exception ex)
		{
			ReCrossApp current2 = ReCrossApp.Current;
			bool isAppConfigured = (Settings.Default.IsAppConfigured = false);
			current2.IsAppConfigured = isAppConfigured;
			Settings.Default.Save();
			MessageBox.Show("Conexão à Base de Dados falhou!\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		return result;
	}

	private bool Cancel()
	{
		((MainWindow)ReCrossApp.Current.Windows[0]).SetPreviousScreen();
		return true;
	}

	private bool Backup()
	{
		if (ConfigManager.CreateServerInstance(Settings.Default.DataSource, Settings.Default.Username, Settings.Default.Password))
		{
			ConfigManager.BackupDataBase("ReCross", "ReCross_{0}.bak");
		}
		return true;
	}

	private bool Restore()
	{
		if (ConfigManager.CreateServerInstance(Settings.Default.DataSource, Settings.Default.Username, Settings.Default.Password) && ConfigManager.RestoreDataBase("ReCross"))
		{
			MessageBox.Show("Operação feita com sucesso.\n\nA aplicação vai reiniciar.", "Informação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			ReCrossApp.Restart();
		}
		return true;
	}
}
