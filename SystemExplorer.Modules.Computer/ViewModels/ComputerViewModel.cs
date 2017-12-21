using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Computer.ViewModels {
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
                yield return new DataItem {
                    Name = "User name",
                    Value = Environment.UserName
                };
                yield return new DataItem {
                    Name = "Domain",
                    Value = Environment.UserDomainName
                };
                yield return new DataItem {
                    Name = "64 bit OS",
                    Value = Environment.Is64BitOperatingSystem ? "Yes" : "No"
                };
                yield return new DataItem {
                    Name = "Page Size",
                    Value = $"{Environment.SystemPageSize >> 10} KB"
                };

                var si = SystemInformation.GetNativeSystemInfo();
                yield return new DataItem {
                    Name = "Processor Arch",
                    Value = si.ProcessorArchitecture.ToString()
                };
                yield return new DataItem {
                    Name = "Processor Count",
                    Value = si.NumberOfProcessors.ToString()
                };


            }
        }
    }
}
