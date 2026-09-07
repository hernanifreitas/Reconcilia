using System.ComponentModel;

namespace ReCross.FE.Utils;

[TypeConverter(typeof(LocalizedEnumConverter))]
public enum OperationEnum
{
	Import,
	LinkUnlink,
	AutoLink,
	CloseReOpen
}
