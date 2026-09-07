using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[TypeIdentifier]
[Guid("000208D8-0000-0000-C000-000000000046")]
[CompilerGenerated]
public interface _Worksheet
{
	void _VtblGap1_45();

	Range Cells
	{
		[DispId(238)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
