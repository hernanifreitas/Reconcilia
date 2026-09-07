using System.ComponentModel;

namespace ReCross.FE.Utils;

[TypeConverter(typeof(LocalizedEnumConverter))]
public enum MenuItemEnum
{
	Customers,
	BankAccounts,
	AccountMovements,
	BankMovements,
	Reconciliations,
	Search,
	Parametrization
}
