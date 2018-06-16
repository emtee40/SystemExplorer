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
    sealed class HandlesViewModel : TabItemViewModelBase {
        SystemHandleInformation[] _rawHandles;
        ObservableCollection<HandleViewModel> _handles;
        Dictionary<SystemHandleInformation, HandleViewModel> _handleMap;
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromMilliseconds(100) };

        public IList<HandleViewModel> Handles => _handles;

        [Import]
        IUIServices UI;

        public HandlesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/handles.ico").ToString();
            _rawHandles = SystemInformation.EnumHandles() ?? throw new Exception("Failed to get system handles");

            _handles = new ObservableCollection<HandleViewModel>(_rawHandles.Take(1000).Select(handle => new HandleViewModel(handle)));
            _handleMap = _handles.ToDictionary(handle => handle.Info);

            _timer.Tick += delegate {
                int count = _handles.Count;
                if (count == _rawHandles.Length) {
                    _timer.Stop();
                    return;
                }
                foreach (var item in _rawHandles.Skip(count).Take(Math.Min(5000, _rawHandles.Length - count)).Select(item => new HandleViewModel(item))) {
                    _handles.Add(item);
                    _handleMap.Add(item.Info, item);
                }
            };
            _timer.Start();
        }

        public void Update() {
            var handles = SystemInformation.EnumHandles();
            var deadHandles = _rawHandles.AsParallel().Except(handles.AsParallel());

        }
    }
}

