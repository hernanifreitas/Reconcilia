using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ReCross.FE.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ReCross.FE.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string _Edit_Header => ResourceManager.GetString("_Edit.Header", resourceCulture);

	internal static string _Exit_Header => ResourceManager.GetString("_Exit.Header", resourceCulture);

	internal static string _File_Header => ResourceManager.GetString("_File.Header", resourceCulture);

	internal static string _Help_Header => ResourceManager.GetString("_Help.Header", resourceCulture);

	internal static string _Options_Header => ResourceManager.GetString("_Options.Header", resourceCulture);

	internal static string MenuItemEnum_AccountMovements => ResourceManager.GetString("MenuItemEnum_AccountMovements", resourceCulture);

	internal static string MenuItemEnum_BankAccounts => ResourceManager.GetString("MenuItemEnum_BankAccounts", resourceCulture);

	internal static string MenuItemEnum_BankMovements => ResourceManager.GetString("MenuItemEnum_BankMovements", resourceCulture);

	internal static string MenuItemEnum_Customers => ResourceManager.GetString("MenuItemEnum_Customers", resourceCulture);

	internal static string MenuItemEnum_Parametrization => ResourceManager.GetString("MenuItemEnum_Parametrization", resourceCulture);

	internal static string MenuItemEnum_Reconciliations => ResourceManager.GetString("MenuItemEnum_Reconciliations", resourceCulture);

	internal static string MenuItemEnum_Search => ResourceManager.GetString("MenuItemEnum_Search", resourceCulture);

	internal static string OperationEnum_AutoLink => ResourceManager.GetString("OperationEnum_AutoLink", resourceCulture);

	internal static string OperationEnum_CloseReOpen => ResourceManager.GetString("OperationEnum_CloseReOpen", resourceCulture);

	internal static string OperationEnum_Import => ResourceManager.GetString("OperationEnum_Import", resourceCulture);

	internal static string OperationEnum_LinkUnlink => ResourceManager.GetString("OperationEnum_LinkUnlink", resourceCulture);

	internal static string SubMenuItemEnum_BankEntities => ResourceManager.GetString("SubMenuItemEnum_BankEntities", resourceCulture);

	internal static string SubMenuItemEnum_BankMovementParam => ResourceManager.GetString("SubMenuItemEnum_BankMovementParam", resourceCulture);

	internal static string SubMenuItemEnum_Companies => ResourceManager.GetString("SubMenuItemEnum_Companies", resourceCulture);

	internal static string SubMenuItemEnum_Groups => ResourceManager.GetString("SubMenuItemEnum_Groups", resourceCulture);

	internal static string SubMenuItemEnum_Persons => ResourceManager.GetString("SubMenuItemEnum_Persons", resourceCulture);

	internal static string SubMenuItemEnum_ReconciliationResults => ResourceManager.GetString("SubMenuItemEnum_ReconciliationResults", resourceCulture);

	internal static string SubMenuItemEnum_Roles => ResourceManager.GetString("SubMenuItemEnum_Roles", resourceCulture);

	internal static string SubMenuItemEnum_SoftwareEntities => ResourceManager.GetString("SubMenuItemEnum_SoftwareEntities", resourceCulture);

	internal static string SubMenuItemEnum_SoftwareMovementParam => ResourceManager.GetString("SubMenuItemEnum_SoftwareMovementParam", resourceCulture);

	internal static string SubMenuItemEnum_Users => ResourceManager.GetString("SubMenuItemEnum_Users", resourceCulture);

	internal Resources()
	{
	}
}
