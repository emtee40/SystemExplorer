using Prism.Commands;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.WPF;

namespace SystemExplorer.Modules.Processes.ViewModels {
    [Export, Item(Text = "Handles")]
    sealed class HandlesViewModel : TabItemViewModelBase, IPartImportsSatisfiedNotification {
        sealed class HandleComparer : IEqualityComparer<SystemHandleInformation> {
            public bool Equals(SystemHandleInformation x, SystemHandleInformation y) => x.Handle == y.Handle && x.ProcessId == y.ProcessId;

            public int GetHashCode(SystemHandleInformation obj) => obj.ProcessId ^ obj.Handle;

            public static readonly HandleComparer Instance = new HandleComparer();
        }

        SystemHandleInformation[] _rawHandles;
        ObservableCollection<HandleViewModel> _handles;
        Dictionary<SystemHandleInformation, HandleViewModel> _handleMap;
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromMilliseconds(100) };

        public IList<HandleViewModel> Handles => _handles;

        [Import]
        IUIServices UI;

        [Import]
        ITabManager TabManager;

        public HandlesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/handles.ico").ToString();
        }

        public void Update() {
            TabManager.IsBusy = true;

            _rawHandles = SystemInformation.EnumHandles() ?? throw new Exception("Failed to get system handles");
            _handles = new ObservableCollection<HandleViewModel>(_rawHandles.Take(1000).Select(handle => new HandleViewModel(handle)));
            _handleMap = _handles.ToDictionary(handle => handle.Info);
            RaisePropertyChanged(nameof(Handles));

            _timer.Tick += delegate {
                int count = _handles.Count;
                if (count == _rawHandles.Length) {
                    _timer.Stop();
                    TabManager.IsBusy = false;
                    return;
                }
                foreach (var item in _rawHandles.Skip(count).Take(Math.Min(5000, _rawHandles.Length - count)).Select(item => new HandleViewModel(item))) {
                    _handles.Add(item);
                    _handleMap.Add(item.Info, item);
                }
            };
            _timer.Start();

            //var deadHandles = _rawHandles.AsParallel().Except(handles.AsParallel(), HandleComparer.Instance);

        }

        public void OnImportsSatisfied() {
            Update();
        }

        public ICollectionViewAdv View { get; set; }

        public DelegateCommandBase RefreshCommand => new DelegateCommand(() => Update());
    }
}

