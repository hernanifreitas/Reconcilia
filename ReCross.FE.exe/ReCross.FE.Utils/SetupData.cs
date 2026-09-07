using System;
using System.Globalization;
using System.IO;
using System.Windows;
using ReCross.FE.Properties;

namespace ReCross.FE.Utils;

internal class SetupData
{
	public static void SetBaseFunctionalities()
	{
	}

	public static void SetBaseRoles()
	{
	}

	public static void SetBaseUsers()
	{
	}

	public static string CodeNumer(int number)
	{
		return $"{number:X2}";
	}

	public static int UncodeNumer(string number)
	{
		number = number.Replace("0x", string.Empty);
		return int.Parse(number, NumberStyles.AllowHexSpecifier);
	}

	public static void UpdateConfigurationFromFile()
	{
		string configFile = Settings.Default.ConfigFile;
		try
		{
			DateTime lastWriteTime = File.GetLastWriteTime(configFile);
			if (!(Settings.Default.ConfigFileLastModify.ToString() != lastWriteTime.ToString()))
			{
				return;
			}
			StreamReader streamReader = File.OpenText(configFile);
			string text = streamReader.ReadLine();
			while (text != null && !string.IsNullOrEmpty(text.Trim()))
			{
				string[] array = text.Split('=');
				array[0] = array[0].TrimStart();
				array[0] = array[0].TrimEnd();
				array[1] = array[1].TrimStart();
				array[1] = array[1].TrimEnd();
				switch (array[0].Trim())
				{
				case "DataSource":
					Settings.Default.DataSource = array[1];
					break;
				case "Username":
					Settings.Default.Username = array[1];
					break;
				case "Password":
					Settings.Default.Password = array[1];
					break;
				case "RecDocPath":
					Settings.Default.RecDocPath = array[1];
					break;
				case "CompanyName":
					Settings.Default.CompanyName = array[1];
					break;
				case "CompanyLogo":
					Settings.Default.CompanyLogo = array[1];
					break;
				case "CompanyFiscalNumber":
					Settings.Default.CompanyFiscalNumber = array[1];
					break;
				case "AutoLinkEnabled":
					Settings.Default.AutoLinkEnabled = bool.Parse(array[1]);
					break;
				}
				text = streamReader.ReadLine();
			}
			Settings.Default.ConfigFileLastModify = lastWriteTime;
			Settings.Default.Save();
			ReCrossApp.Restart();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Erro ao abrir o ficheiro de configuração do Reconcilia '" + configFile + "':\n\n" + ex.Message + "\n\nA aplicação vai continuar com as configurações locais.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}
}
