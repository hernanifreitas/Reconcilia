using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[Guid("0002084D-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[CompilerGenerated]
[TypeIdentifier]
public interface Font
{
	object Color
	{
		[PreserveSig]
		[DispId(99)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[PreserveSig]
		[DispId(99)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
