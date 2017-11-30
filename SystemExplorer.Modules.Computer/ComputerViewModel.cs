using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Modules.Computer {
    [Item(Text = "Computer")]
    sealed class ComputerViewModel : TabItemViewModelBase {
        public ComputerViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly().GetName().Name, "/icons/computer.ico");
        }

        public IEnumerable<DataItem> Items {
            get {
                yield return new DataItem {
                    Name = "Computer Name",
                    Value = Environment.MachineName
                };
                yield return new DataItem {
                    Name = "Windows Version",
                    Value = Environment.OSVersion.VersionString
                };

            }
        }
    }
}
