using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[Guid("000208DA-0000-0000-C000-000000000046")]
[TypeIdentifier]
[CompilerGenerated]
public interface _Workbook
{
	void _VtblGap1_7();

	object ActiveSheet
	{
		[DispId(307)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}
}
