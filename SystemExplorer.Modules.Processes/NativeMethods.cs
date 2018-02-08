using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Modules.Processes {
    [SuppressUnmanagedCodeSecurity]
    static class NativeMethods {
        [DllImport("Shell32", CharSet = CharSet.Unicode)]
        public static extern IntPtr ExtractIcon(IntPtr hInstance, string filename, int iconIndex = 0);
    }
}
