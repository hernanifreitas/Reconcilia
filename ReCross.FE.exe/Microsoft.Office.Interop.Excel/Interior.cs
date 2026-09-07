using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[CompilerGenerated]
[Guid("00020870-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[TypeIdentifier]
public interface Interior
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
