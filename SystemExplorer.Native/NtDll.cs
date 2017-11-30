using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Native {
	public enum SystemInformationClass : uint {
		BasicInformation
	}

	public enum ProcessInformationClass : uint {
		BasicInformation
	}

	[SuppressUnmanagedCodeSecurity]
	public static class NtDll {
		[DllImport("ntdll", ExactSpelling = true)]
		public static extern int NtQuerySystemInformation(SystemInformationClass infoClass, IntPtr buffer, uint size, out uint actualSize);

		[DllImport("ntdll", ExactSpelling = true)]
		public static extern int NtQueryInformationProcess(IntPtr hProcess, ProcessInformationClass infoClass, IntPtr buffer, uint size, out uint actualSize);
	}
}
