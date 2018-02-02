using Prism.Commands;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        List<(ProcessViewModel process, DateTime time)> _deadProcesses = new List<(ProcessViewModel, DateTime)>(4);
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };

        public ColumnManager Columns { get; } = new ColumnManager();

        static ProcessComparer _comparer = new ProcessComparer();

        public IList<ProcessViewModel> Processes => _processes;

        public ProcessesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/processes.ico").ToString();

            _processesRaw = SystemInformation.EnumProcessesAndThreads();
            _processes = new ObservableCollection<ProcessViewModel>(_processesRaw.Select(process => new ProcessViewModel(process)));
            _processMap = _processes.ToDictionary(process => (process.Info.ProcessId, process.Info.CreateTime));

            Columns.BuildFromType(typeof(ProcessViewModel));

            _timer.Tick += delegate { Refresh(); };
            _timer.Start();
        }

        Stopwatch _ticks = Stopwatch.StartNew();

        public void Refresh() {
            for(int i = 0; i < _deadProcesses.Count; i++) {
                var process = _deadProcesses[i];
                if ((DateTime.UtcNow - process.time).TotalMilliseconds > 3000) {
                    // now really dead
                    _processes.Remove(process.process);
                    _deadProcesses.RemoveAt(i);
                    i--;
                }
            }

            var time = _ticks.Elapsed;
            var processes = SystemInformation.EnumProcessesAndThreads();

            // remove dead processes

            var deadProcesses = _processesRaw.Except(processes, _comparer);
            foreach (var p in deadProcesses) {
                var key = (p.ProcessId, p.CreateTime);
                var vm = _processMap[key];
                vm.IsDead = true;
                _deadProcesses.Add((vm, DateTime.UtcNow));
                //_processes.Remove(vm);
                vm.Dispose();
                _processMap.Remove(key);
            }

            _processesRaw = processes;

            foreach (var process in processes) {
                if (_processMap.TryGetValue((process.ProcessId, process.CreateTime), out var vm)) {
                    // process still exists, refresh it
                    if (!vm.IsDead)
                        vm.Update(process, time.Ticks);
                }
                else {
                    // new process, add
                    vm = new ProcessViewModel(process);
                    _processes.Add(vm);
                    _processMap.Add((process.ProcessId, process.CreateTime), vm);
                    vm.IsCreated = true;
                }
            }
        }

        protected override void OnActive(bool active) {
            if (!active)
                _timer.Stop();
            else
                _timer.Start();
        }

        public ICollectionViewAdv View { get; set; }

        string _filterText;
        public string FilterText {
            get => _filterText;
            set {
                if (SetProperty(ref _filterText, value)) {
                    if (string.IsNullOrWhiteSpace(value))
                        View.Filter = null;
                    else {
                        var text = value.ToLower();
                        View.Filter = obj => {
                            var vm = (ProcessViewModel)obj;
                            return vm.Info.ImageName.ToLower().Contains(text);
                        };
                    }
                    View.RefreshFilter();
                }
            }
        }

        public ICommand SearchCommand => new DelegateCommand<SfDataGrid>(dataGrid => {
        });
    }
}
