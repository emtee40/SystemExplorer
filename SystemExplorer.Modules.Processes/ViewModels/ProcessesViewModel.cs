using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    sealed class ProcessComparer : IEqualityComparer<ProcessExtendedInformation> {
        public bool Equals(ProcessExtendedInformation x, ProcessExtendedInformation y) {
            return x.ProcessId == y.ProcessId && x.CreateTime == y.CreateTime;
        }

        public int GetHashCode(ProcessExtendedInformation obj) {
            return obj.ProcessId.GetHashCode() ^ obj.CreateTime.GetHashCode();
        }
    }

    [Export, Item(Text = "Processes")]
    sealed class ProcessesViewModel : TabItemViewModelBase {
        Dictionary<(int, DateTime), ProcessViewModel> _processMap;
        ObservableCollection<ProcessViewModel> _processes;
        IReadOnlyList<ProcessExtendedInformation> _processesRaw;
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        static ProcessComparer _comparer = new ProcessComparer();

        public IList<ProcessViewModel> Processes => _processes;

        public ProcessesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/processes.ico").ToString();
            _processesRaw = SystemInformation.EnumProcessesAndThreads();
            _processes = new ObservableCollection<ProcessViewModel>(_processesRaw.Select(process => new ProcessViewModel(process)));
            _processMap = _processes.ToDictionary(process => (process.Info.ProcessId, process.Info.CreateTime));
            _timer.Tick += delegate { Refresh(); };
            _timer.Start();
        }

        public void Refresh() {
            var processes = SystemInformation.EnumProcessesAndThreads();

            // remove dead processes
            var deadProcesses = _processesRaw.Except(processes, _comparer);
            foreach (var p in deadProcesses) {
                var key = (p.ProcessId, p.CreateTime);
                _processes.Remove(_processMap[key]);
                _processMap.Remove(key);
            }

            _processesRaw = processes;

            foreach (var process in processes) {
                if (_processMap.TryGetValue((process.ProcessId, process.CreateTime), out var vm)) {
                    // process still exists, refresh it
                    vm.Update(process);
                }
                else {
                    // new process, add
                    vm = new ProcessViewModel(process);
                    _processes.Add(vm);
                    _processMap.Add((process.ProcessId, process.CreateTime), vm);
                }
            }
        }

        protected override void OnActive(bool active) {
            if (!active)
                _timer.Stop();
            else
                _timer.Start();
        }
    }
}
