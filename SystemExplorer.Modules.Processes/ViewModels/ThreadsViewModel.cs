using Syncfusion.Data;
using System;
using System.Collections.Generic;
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
    [Export, Item(Text = "Threads")]
    sealed class ThreadsViewModel : TabItemViewModelBase {
        public ThreadsViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/threads.ico").ToString();
        }

        ThreadViewModel[] _threads;
        public ThreadViewModel[] Threads {
            get {
                if (_threads != null)
                    return _threads;
                _tabManager.IsBusy = true;
                Refresh();
                return _threads;
            }
        }

        [Import]
        ProcessesViewModel _processes;

        [Import]
        ITabManager _tabManager;

        public async void Refresh() {
            _threads = await Task.Run(() => SystemInformation.EnumThreads().Select(th => new ThreadViewModel(_processes, th)).ToArray());
            RaisePropertyChanged(nameof(Threads));
            _tabManager.IsBusy = false;
        }

        public ICollectionViewAdv View { get; set; }
    }
}
