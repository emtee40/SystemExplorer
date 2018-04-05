using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SystemExplorer.Modules.Processes {
    static class ModuleHelpers {
        public static BitmapSource ExtractIcon(string filename) {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            var hIcon = NativeMethods.ExtractIcon(IntPtr.Zero, filename);
            if (hIcon == IntPtr.Zero)
                return null;
            return Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
