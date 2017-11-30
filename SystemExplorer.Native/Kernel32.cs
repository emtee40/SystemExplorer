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
	public enum ProcessAccessMask : uint {
		CreateProcess = 0x80,
		CreateThread = 2,
		DuplicateHandle = 0x40,
		QueryInformation = 0x400,
		QueryLimitedInformation = 0x1000,
		SetInformation = 0x200,
		SetQuota = 0x100,
		SuspendResume = 0x800,
		Terminate = 1,
		VmOperation = 8,
		VmRead = 0x10,
		VmWrite = 0x20,
		Synchronize = 0x100000,
		AllAccess = CreateProcess | CreateThread | DuplicateHandle | QueryInformation | SetInformation | SetQuota | SuspendResume | Terminate | VmOperation | VmRead | VmWrite | Synchronize
	}

	[Flags]
	public enum TokenAccessMask : uint {
	}

	[SuppressUnmanagedCodeSecurity]
	public static class Kernel32 {
		[DllImport("kernel32", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32", SetLastError = true)]
		public static extern SafeWaitHandle OpenProcess(ProcessAccessMask accessMask, bool inheritHandle, int pid);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool OpenProcessToken(SafeWaitHandle hProcess, TokenAccessMask accessMask, out SafeTokenHandle handle); 
	}
}
