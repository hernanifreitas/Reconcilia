using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel;

[ComImport]
[Guid("000208D5-0000-0000-C000-000000000046")]
[CompilerGenerated]
[DefaultMember("_Default")]
[TypeIdentifier]
public interface _Application
{
	void _VtblGap1_45();

	Workbooks Workbooks
	{
		[DispId(572)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_177();

	[DispId(302)]
	void Quit();
}
