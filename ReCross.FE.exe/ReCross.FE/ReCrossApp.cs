using System;
using System.Deployment.Application;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Properties;
using ReCross.FE.ReCrossService;
using ReCross.FE.Utils;
using Strills.WPF.LicenseManagement;
using Strills.WPF.Localization;
using Strills.WPF.UI.Configuration;
using Strills.WPF.UI.Windows;

namespace ReCross.FE;

public partial class ReCrossApp : ApplicationBase<ReCrossEntities>
{
	private const string C_RecDocPath = "RecDocPath";

	private const string C_DBStartUpScriptFile = "startupdb.rec";

	private const string C_DBStartUpBakFile = "ReCross.bak";

	private const string C_DBStartUpFile = "ReCross.mdf";

	private ReCrossServiceClient serviceClient;

	private string connectionString;

	public new static ReCrossApp Current => (ReCrossApp)ApplicationBase<ReCrossEntities>.Current;

	public string CompanyLogo { get; set; }

	public bool IsActiveForOwners { get; set; }

	public ReCrossApp()
	{
		if (Settings.Default.UpgradeSettings)
		{
			Settings.Default.Upgrade();
			Settings.Default.UpgradeSettings = false;
		}
		ConfigManager.ConfigLocation = Assembly.GetExecutingAssembly().Location;
		connectionString = ConfigManager.ChangeConnectionString("ReCrossEntities", Settings.Default.DataSource, "ReCross", Settings.Default.Username, Settings.Default.Password, "DataObjects.ReCross");
		try
		{
			if (!Settings.Default.IsAppConfigured || Settings.Default.UseConfigFile)
			{
				if (!Settings.Default.IsAppConfigured)
				{
					Settings.Default.UseConfigFile = true;
					Settings.Default.Save();
				}
				SetupData.UpdateConfigurationFromFile();
			}
			string dataSource = Settings.Default.DataSource;
			string username = Settings.Default.Username;
			string password = Settings.Default.Password;
			bool flag = !string.IsNullOrEmpty(dataSource);
			bool flag2 = !string.IsNullOrEmpty(username);
			bool flag3 = !string.IsNullOrEmpty(password);
			if ((flag && !connectionString.Contains("Data Source=" + dataSource + ";")) || (!flag && connectionString.Contains("Data Source=")) || (flag2 && !connectionString.Contains("User ID=" + username + ";")) || (!flag2 && connectionString.Contains("User ID=")) || (flag3 && !connectionString.Contains("Password=" + password + ";")) || (!flag3 && connectionString.Contains("Password=")))
			{
				Current.CurrentDataContext = new ReCrossEntities(connectionString);
				ReCrossEntities.CurrentContext = Current.CurrentDataContext;
				ReCrossApp current = Current;
				bool isAppConfigured = (Settings.Default.IsAppConfigured = true);
				current.IsAppConfigured = isAppConfigured;
				Restart();
			}
			else if (Current.IsAppConfigured)
			{
				try
				{
					string dataBasePath = ConfigManager.ConfigLocation.Replace("ReCross.FE.exe", "");
					string text = connectionString;
					if (!ConfigManager.CanConnectToDB(ref text) && ConfigManager.CreateServerInstance(Settings.Default.DataSource, Settings.Default.Username, Settings.Default.Password) && ConfigManager.AttachDataBase(dataBasePath, "ReCross"))
					{
						Restart();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro ao tentar ler/criar base de dados inicial.\n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
					Current.Shutdown();
					return;
				}
			}
		}
		catch
		{
			if (Current == null)
			{
				return;
			}
			ReCrossApp current2 = Current;
			bool isAppConfigured = (Settings.Default.IsAppConfigured = false);
			current2.IsAppConfigured = isAppConfigured;
		}
		if (string.IsNullOrEmpty(Settings.Default.CompanyFiscalNumber))
		{
			Settings.Default.IsAppConfigured = false;
		}
		Current.IsAppConfigured = Settings.Default.IsAppConfigured;
		if (string.IsNullOrEmpty(Settings.Default.CompanyLogo))
		{
			Settings.Default.CompanyLogo = "..\\Reports\\Templates\\Logo.jpg";
		}
		Settings.Default.Save();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		CultureManager.UICulture = new CultureInfo("pt");
		base.OnStartup(e);
		if (!Current.IsAppConfigured)
		{
			return;
		}
		try
		{
			ReCrossEntities.CurrentContext = Current.CurrentDataContext;
			try
			{
				string text = connectionString;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("USE [ReCross]");
				stringBuilder.AppendLine("SET ANSI_NULLS ON");
				stringBuilder.AppendLine("SET QUOTED_IDENTIFIER ON");
				stringBuilder.AppendLine("IF NOT EXISTS( SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'Group' AND [COLUMN_NAME] = 'IsOrEnabled') BEGIN   ALTER TABLE [Group] ADD [IsOrEnabled] [bit] NOT NULL Default((0))   END");
				stringBuilder.AppendLine("IF NOT EXISTS( SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'Group' AND [COLUMN_NAME] = 'IsActiveForOwners') BEGIN   ALTER TABLE [Group] ADD [IsActiveForOwners] [bit] NOT NULL Default((0))   END");
				string text2 = "IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRoles')) BEGIN    ";
				text2 += "CREATE TABLE [dbo].[UserRoles]( ";
				text2 += "[UserId] [bigint] NOT NULL, ";
				text2 += "[RoleId] [bigint] NOT NULL, ";
				text2 += ") ON [PRIMARY] ";
				text2 += "ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[User] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE ";
				text2 += "ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_User] ";
				text2 += "ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE ";
				text2 += "ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Role] ";
				text2 += "   END ";
				stringBuilder.AppendLine(text2);
				if (!ConfigManager.ConnectAndUpdateDB(ref text, stringBuilder.ToString()))
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Problema com SQL Server.\n\n" + ex.Message + "\n\n" + ((ex.InnerException != null) ? ex.InnerException.Message : ""), "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			OwnData ownData = null;
			bool flag = true;
			bool flag2 = true;
			DateTime dateTime = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);
			flag2 = Settings.Default.IsBankToAccountMov;
			bool flag3 = true;
			string text3 = null;
			CompiledQueries.OwnDataParams arg = default(CompiledQueries.OwnDataParams);
			try
			{
				string companyFiscalNumber = Settings.Default.CompanyFiscalNumber;
				if (!string.IsNullOrEmpty(companyFiscalNumber))
				{
					serviceClient = new ReCrossServiceClient();
					if (serviceClient != null)
					{
						try
						{
							Customer customerByID = serviceClient.GetCustomerByID(companyFiscalNumber);
							bool isAppConfigured;
							if (customerByID == null)
							{
								ReCrossApp current = Current;
								isAppConfigured = (Settings.Default.IsAppConfigured = false);
								current.IsAppConfigured = isAppConfigured;
								MessageBox.Show("NIF da empresa não reconhecido.\n\nPor favor verifique o NIF da empresa configurado no Reconcilia ou contacte a Cross.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
								return;
							}
							dateTime = customerByID.LicenseDate;
							if (string.IsNullOrEmpty(Settings.Default.BankMovId))
							{
								Settings.Default.BankMovId = SetupData.CodeNumer(customerByID.LicenseCount);
							}
							int licenseCount = customerByID.LicenseCount;
							isAppConfigured = (Settings.Default.IsBankToAccountMov = customerByID.Allowed);
							flag2 = isAppConfigured;
							Settings.Default.AccountMovId = SetupData.CodeNumer(customerByID.LicenseDays);
							arg.code = MachineInfo.GetVolumeSerial(ConfigManager.ConfigLocation.Split(':')[0]);
							arg.reference = MachineInfo.GetCPUId();
							arg.ownName = string.Empty;
							IQueryable<OwnData> queryable = Current.CurrentDataContext.OwnData.Select((OwnData rec) => rec);
							if (queryable != null && queryable.Count() >= licenseCount)
							{
								flag = false;
							}
							queryable = CompiledQueries.GetOwnDatas(Current.CurrentDataContext, arg);
							if (queryable == null || queryable.Count() == 0)
							{
								if (!flag)
								{
									MessageBox.Show("O número de licenças para o cliente " + Settings.Default.CompanyName + " foi excedido.\n\nPor favor, contactar a Cross para actualizar situação do licenciamente e respectivo número.\n\nA aplicação vai encerrar.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
									Current.Shutdown();
									return;
								}
								queryable = CompiledQueries.GetOwnDataCollection(Current.CurrentDataContext);
								int num = ((queryable == null || queryable.Count() == 0) ? 1 : (queryable.Count() + 1));
								ownData = OwnData.CreateOwnData(num, dateTime, arg.reference, arg.code, arg.ownName);
								Current.CurrentDataContext.OwnData.AddObject(ownData);
							}
							else
							{
								ownData = queryable.ToArray()[0];
							}
						}
						catch (Exception ex)
						{
							flag3 = false;
							text3 = ex.Message;
						}
					}
				}
			}
			catch (Exception ex)
			{
				flag3 = false;
				text3 = ex.Message;
			}
			if (!flag3)
			{
				bool flag5 = false;
				arg.code = MachineInfo.GetVolumeSerial(ConfigManager.ConfigLocation.Split(':')[0]);
				arg.reference = MachineInfo.GetCPUId();
				IQueryable<OwnData> queryable = CompiledQueries.GetOwnDatas(Current.CurrentDataContext, arg);
				if (queryable == null || queryable.Count() <= 0)
				{
					MessageBox.Show("Licença da aplicação Reconcilia não está activa!\n\nPor favor certifique que o computador se encontra ligado à internet para proceder à activação e abra a aplicação de novo.\n\nA aplicação vai encerrar.\n\n" + text3, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
					Current.Shutdown();
					return;
				}
				ownData = queryable.ToArray()[0];
			}
			if (ownData != null)
			{
				string text4 = DateTime.Now.ToString();
				int num2 = SetupData.UncodeNumer(Settings.Default.AccountMovId);
				if (!flag2)
				{
					ownData.ValueDate = new DateTime(2001, ownData.ValueDate.Month, ownData.ValueDate.Day);
				}
				else if (!flag3)
				{
					flag2 = ownData.ValueDate.Year != 2001;
				}
				else if (ownData.ValueDate.Year == 2001)
				{
					ownData.ValueDate = dateTime;
				}
				else if (ownData.ValueDate.CompareTo(dateTime) != 0)
				{
					ownData.ValueDate = dateTime;
				}
				bool flag6 = false;
				int num3 = 0;
				if (flag2)
				{
					TimeSpan timeSpan = DateTime.Today.Subtract(ownData.ValueDate);
					flag6 = timeSpan.Days > num2;
					if (flag6)
					{
						num3 = timeSpan.Days - num2;
					}
					else
					{
						int num4 = num2 - timeSpan.Days;
						if (num4 <= 15)
						{
							MessageBox.Show("Faltam " + num4 + " dias para a licença da aplicação expirar.\n\nA aplicação vai continuar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
						}
					}
				}
				Current.CurrentDataContext.SaveChanges();
				if (!flag2)
				{
					MessageBox.Show("São necessárias actualizações para continuar a utilização da aplicação.\n\nPor favor, contactar a Cross actualizar a situação.\n\nA aplicação vai encerrar.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
					Current.Shutdown();
					return;
				}
				if (flag6)
				{
					MessageBox.Show("Excedeu a data limite de utilização da aplicação por " + num3 + " dias.\n\nPor favor, contactar a Cross para actualizar situação do licenciamento.\n\nA aplicação vai encerrar.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
					Current.Shutdown();
					return;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Autenticação da licença da aplicação falhou!\n\nA aplicação vai encerrar.\n\n" + ex.Message + "\n\n" + ((ex.InnerException != null) ? ex.InnerException.Message : ""), "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			Current.Shutdown();
			return;
		}
		Current.CompanyLogo = Settings.Default.CompanyLogo;
		Settings.Default.Save();
	}

	[DllImport("clr.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	internal static extern void CorLaunchApplication(uint hostType, string applicationFullName, int manifestPathsCount, string[] manifestPaths, int activationDataCount, string[] activationData, PROCESS_INFORMATION processInformation);

	public void StartUpWindow()
	{
		((MainWindow)base.Windows[0]).StartUp();
	}

	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public static void Restart()
	{
		if (Assembly.GetEntryAssembly() == null)
		{
			throw new NotSupportedException("RestartNotSupported");
		}
		if (ApplicationDeployment.IsNetworkDeployed)
		{
			try
			{
				string updatedApplicationFullName = ApplicationDeployment.CurrentDeployment.UpdatedApplicationFullName;
				Application.Current.Shutdown();
				CorLaunchApplication(0u, updatedApplicationFullName, 0, null, 0, null, new PROCESS_INFORMATION());
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Erro de Deployment:\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		try
		{
			string updatedApplicationFullName = Assembly.GetExecutingAssembly().Location;
			Application.Current.Shutdown();
			CorLaunchApplication(0u, updatedApplicationFullName, 0, null, 0, null, new PROCESS_INFORMATION());
		}
		catch (Exception ex)
		{
			MessageBox.Show("Erro de Deployment:\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}
}
