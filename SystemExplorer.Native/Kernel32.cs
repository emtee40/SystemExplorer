using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Native {
	[Flags]
	public enum TokenAccessMask : uint {
	}

	[SuppressUnmanagedCodeSecurity]
	public static class Kernel32 {
		[DllImport("kernel32", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool OpenProcessToken(SafeWaitHandle hProcess, TokenAccessMask accessMask, out SafeTokenHandle handle); 
	}
}
