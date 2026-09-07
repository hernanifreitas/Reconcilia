using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[CompilerGenerated]
[TypeIdentifier]
[Guid("00020846-0000-0000-C000-000000000046")]
public interface Range : IEnumerable
{
	int Count
	{
		[PreserveSig]
		[DispId(118)]
		get;
	}

	[IndexerName("_Default")]
	object this[[Optional][In][MarshalAs(UnmanagedType.Struct)] object RowIndex, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ColumnIndex]
	{
		[PreserveSig]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[PreserveSig]
		[DispId(0)]
		[param: Optional]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	Font Font
	{
		[PreserveSig]
		[DispId(146)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	Interior Interior
	{
		[PreserveSig]
		[DispId(129)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	Range Rows
	{
		[PreserveSig]
		[DispId(258)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object Value2
	{
		[PreserveSig]
		[DispId(1388)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[PreserveSig]
		[DispId(1388)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}
}
