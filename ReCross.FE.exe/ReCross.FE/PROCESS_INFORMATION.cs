using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace ReCross.FE;

[StructLayout(LayoutKind.Sequential)]
[SuppressUnmanagedCodeSecurity]
internal class PROCESS_INFORMATION
{
	public IntPtr hProcess = IntPtr.Zero;

	public IntPtr hThread = IntPtr.Zero;

	public int dwProcessId;

	public int dwThreadId;

	private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

	~PROCESS_INFORMATION()
	{
		Close();
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal void Close()
	{
		if (hProcess != IntPtr.Zero && hProcess != INVALID_HANDLE_VALUE)
		{
			CloseHandle(new HandleRef(this, hProcess));
			hProcess = INVALID_HANDLE_VALUE;
		}
		if (hThread != IntPtr.Zero && hThread != INVALID_HANDLE_VALUE)
		{
			CloseHandle(new HandleRef(this, hThread));
			hThread = INVALID_HANDLE_VALUE;
		}
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern bool CloseHandle(HandleRef handle);
}
