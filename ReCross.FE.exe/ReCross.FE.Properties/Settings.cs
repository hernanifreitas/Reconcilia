using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ReCross.FE.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UpgradeSettings
	{
		get
		{
			return (bool)this["UpgradeSettings"];
		}
		set
		{
			this["UpgradeSettings"] = value;
		}
	}

	[SettingsManageability(SettingsManageability.Roaming)]
	[DefaultSettingValue("localhost\\SQLEXPRESS")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public string DataSource
	{
		get
		{
			return (string)this["DataSource"];
		}
		set
		{
			this["DataSource"] = value;
		}
	}

	[DefaultSettingValue("")]
	[UserScopedSetting]
	[SettingsManageability(SettingsManageability.Roaming)]
	[DebuggerNonUserCode]
	public string Username
	{
		get
		{
			return (string)this["Username"];
		}
		set
		{
			this["Username"] = value;
		}
	}

	[DefaultSettingValue("")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	[SettingsManageability(SettingsManageability.Roaming)]
	public string Password
	{
		get
		{
			return (string)this["Password"];
		}
		set
		{
			this["Password"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[SettingsManageability(SettingsManageability.Roaming)]
	[DefaultSettingValue("ReCross")]
	public string Database
	{
		get
		{
			return (string)this["Database"];
		}
		set
		{
			this["Database"] = value;
		}
	}

	[UserScopedSetting]
	[SettingsManageability(SettingsManageability.Roaming)]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string RecDocPath
	{
		get
		{
			return (string)this["RecDocPath"];
		}
		set
		{
			this["RecDocPath"] = value;
		}
	}

	[SettingsManageability(SettingsManageability.Roaming)]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	[UserScopedSetting]
	public string UserId
	{
		get
		{
			return (string)this["UserId"];
		}
		set
		{
			this["UserId"] = value;
		}
	}

	[DefaultSettingValue("False")]
	[SettingsManageability(SettingsManageability.Roaming)]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public bool IsAppConfigured
	{
		get
		{
			return (bool)this["IsAppConfigured"];
		}
		set
		{
			this["IsAppConfigured"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[SettingsManageability(SettingsManageability.Roaming)]
	[DefaultSettingValue("")]
	public string CompanyName
	{
		get
		{
			return (string)this["CompanyName"];
		}
		set
		{
			this["CompanyName"] = value;
		}
	}

	[SettingsManageability(SettingsManageability.Roaming)]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string CompanyLogo
	{
		get
		{
			return (string)this["CompanyLogo"];
		}
		set
		{
			this["CompanyLogo"] = value;
		}
	}

	[SettingsManageability(SettingsManageability.Roaming)]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string CompanyFiscalNumber
	{
		get
		{
			return (string)this["CompanyFiscalNumber"];
		}
		set
		{
			this["CompanyFiscalNumber"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	[SettingsManageability(SettingsManageability.Roaming)]
	public string BankMovId
	{
		get
		{
			return (string)this["BankMovId"];
		}
		set
		{
			this["BankMovId"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	[SettingsManageability(SettingsManageability.Roaming)]
	[UserScopedSetting]
	public string AccountMovId
	{
		get
		{
			return (string)this["AccountMovId"];
		}
		set
		{
			this["AccountMovId"] = value;
		}
	}

	[SettingsManageability(SettingsManageability.Roaming)]
	[DefaultSettingValue("True")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public bool IsBankToAccountMov
	{
		get
		{
			return (bool)this["IsBankToAccountMov"];
		}
		set
		{
			this["IsBankToAccountMov"] = value;
		}
	}

	[UserScopedSetting]
	[DefaultSettingValue("False")]
	[DebuggerNonUserCode]
	public bool AutoLinkEnabled
	{
		get
		{
			return (bool)this["AutoLinkEnabled"];
		}
		set
		{
			this["AutoLinkEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("C:/reconcilia.txt")]
	public string ConfigFile
	{
		get
		{
			return (string)this["ConfigFile"];
		}
		set
		{
			this["ConfigFile"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	[UserScopedSetting]
	public bool UseConfigFile
	{
		get
		{
			return (bool)this["UseConfigFile"];
		}
		set
		{
			this["UseConfigFile"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	public DateTime ConfigFileLastModify
	{
		get
		{
			return (DateTime)this["ConfigFileLastModify"];
		}
		set
		{
			this["ConfigFileLastModify"] = value;
		}
	}

	private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
	{
	}

	private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
	{
	}
}
