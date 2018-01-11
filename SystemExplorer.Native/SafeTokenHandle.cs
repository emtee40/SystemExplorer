using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Native {
	public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid {
		public SafeTokenHandle(bool ownsHandle) : base(ownsHandle) {
		}

		protected override bool ReleaseHandle() => Kernel32.CloseHandle(handle);
	}
}
