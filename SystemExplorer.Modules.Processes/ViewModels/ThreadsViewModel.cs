using Syncfusion.Data;
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
    sealed class ThreadComparer : IEqualityComparer<ThreadExtendedInformation> {
        public bool Equals(ThreadExtendedInformation x, ThreadExtendedInformation y) {
            return x.ThreadId == y.ThreadId && x.CreateTime == y.CreateTime;
        }

        public int GetHashCode(ThreadExtendedInformation obj) {
            return obj.ThreadId.GetHashCode() ^ obj.CreateTime.GetHashCode();
        }
    }

    [Export, Item(Text = "Threads")]
    sealed class ThreadsViewModel : TabItemViewModelBase {
        Dictionary<(int, DateTime), ThreadViewModel> _threadMap;
        ObservableCollection<ThreadViewModel> _threads;
        IReadOnlyList<ThreadExtendedInformation> _threadsRaw;
        List<(ThreadViewModel thread, DateTime time)> _deadThreads = new List<(ThreadViewModel, DateTime)>(4);
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
        static ThreadComparer _comparer = new ThreadComparer();

        public ThreadsViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/threads.ico").ToString();
            _threadsRaw = (from p in SystemInformation.EnumProcessesAndThreads()
                           where p.ProcessId != 0 && p.ThreadCount > 0
                           from t in p.Threads
                           select t).ToList();

            _threads = new ObservableCollection<ThreadViewModel>(_threadsRaw.Select(t => new ThreadViewModel(t)));

            _threadMap = _threads.ToDictionary(t => (t.Info.ThreadId, t.Info.CreateTime));
            _timer.Tick += delegate { Refresh(); };
            _timer.Start();
        }

        public IList<ThreadViewModel> Threads => _threads;

        public void Refresh() {
            _timer.Stop();

            for (int i = 0; i < _deadThreads.Count; i++) {
                var thread = _deadThreads[i];
                if ((DateTime.UtcNow - thread.time).TotalMilliseconds > 3000) {
                    // now really dead
                    _threads.Remove(thread.thread);
                    _deadThreads.RemoveAt(i);
                    i--;
                }
            }

            var threads = (from p in SystemInformation.EnumProcessesAndThreads().AsParallel()
                           where p.ProcessId != 0 && p.ThreadCount > 0
                           from t in p.Threads
                           select t).ToList();


            // remove dead processes

            var deadThreads = _threadsRaw.Except(threads, _comparer);
            foreach (var t in deadThreads) {
                var key = (t.ThreadId, t.CreateTime);
                var vm = _threadMap[key];
                vm.IsDead = true;
                _deadThreads.Add((vm, DateTime.UtcNow));
                vm.Dispose();
                _threadMap.Remove(key);
            }

            _threadsRaw = threads;

            var dispatcher = Dispatcher.CurrentDispatcher;
            foreach (var thread in threads) {
                if (_threadMap.TryGetValue((thread.ThreadId, thread.CreateTime), out var vm)) {
                    // thread still exists, refresh it
                    if (!vm.IsDead) {
                        vm.Update(thread);
                    }
                }
                else {
                    // new process, add
                    vm = new ThreadViewModel(thread);
                    _threads.Add(vm);

                    _threadMap.Add((thread.ThreadId, thread.CreateTime), vm);
                    vm.IsCreated = true;
                }
            }

            _timer.Start();
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
                            var vm = (ThreadViewModel)obj;
                            return vm.ProcessName.ToLower().Contains(text);
                        };
                    }
                    View.RefreshFilter();
                }
            }
        }

    }
}
