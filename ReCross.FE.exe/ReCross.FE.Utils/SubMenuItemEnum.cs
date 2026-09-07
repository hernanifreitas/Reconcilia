using System.ComponentModel;

namespace ReCross.FE.Utils;

[TypeConverter(typeof(LocalizedEnumConverter))]
public enum SubMenuItemEnum
{
	Persons,
	Companies,
	BankEntities,
	SoftwareEntities,
	BankMovementParam,
	SoftwareMovementParam,
	Roles,
	Groups,
	Users,
	ReconciliationResults
}
