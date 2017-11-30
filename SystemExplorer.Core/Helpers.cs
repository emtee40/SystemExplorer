using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
	public static class Helpers {
		public static readonly bool IsAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		public static readonly bool IsLocalSystem = WindowsIdentity.GetCurrent().IsSystem;
        public static string ToPackUri(string module, string relativeUri) => $"pack://application:,,,/{module};component{relativeUri}";
        public static Uri ToPackUri(Assembly asm, string relativeUri) => new Uri($"pack://application:,,,/{asm.GetName().Name};component{relativeUri}");
    }
}
