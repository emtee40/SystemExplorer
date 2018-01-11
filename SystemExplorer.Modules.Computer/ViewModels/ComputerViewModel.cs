using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Computer.ViewModels {
    [Export, Item(Text = "Computer")]
    sealed class ComputerViewModel : TabItemViewModelBase {
        DispatcherTimer _timer;

        public ComputerViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly().GetName().Name, "/icons/computer.ico");

            _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += delegate { Refresh(); };
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
                    Name = "Processor Architecture",
                    Value = si.ProcessorArchitecture.ToString()
                };
                yield return new DataItem {
                    Name = "Processor Count",
                    Value = si.NumberOfProcessors.ToString()
                };
                yield return new DataItem {
                    Name = "System Directory",
                    Value = Environment.SystemDirectory
                };

                var pi = SystemInformation.GetPerformanceInformation();
                yield return new DataItem {
                    Value = pi.ProcessCount.ToString(),
                    Name = "Processes"
                };
                yield return new DataItem {
                    Value = pi.ThreadCount.ToString(),
                    Name = "Threads"
                };
                yield return new DataItem {
                    Value = pi.HandleCount.ToString(),
                    Name = "Handles"
                };
                yield return new DataItem {
                    Name = "Performance Counter",
                    Value = SystemInformation.PerformanceCounter.ToString()
                };
                yield return new DataItem {
                    Name = "Performance Frequency",
                    Value = SystemInformation.PerformanceFrequency.ToString()
                };
            }
        }

        protected override void OnActive(bool active) {
            if (!active)
                _timer.Stop();
            else if (AutoRefresh)
                _timer.Start();
        }

        bool _autoRefresh;
        public bool AutoRefresh {
            get => _autoRefresh;
            set {
                if (SetProperty(ref _autoRefresh, value)) {
                    if (value && IsActive)
                        _timer.Start();
                    else
                        _timer.Stop();
                }
            }
        }

        public void Refresh() {
            RaisePropertyChanged(nameof(Items));
        }

        public ICommand RefreshCommand => new DelegateCommand(Refresh, () => !AutoRefresh).ObservesProperty(() => AutoRefresh);
    }
}
