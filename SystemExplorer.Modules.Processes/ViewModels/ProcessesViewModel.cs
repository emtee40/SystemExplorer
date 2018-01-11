using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    [Export, Item(Text = "Processes")]
    sealed class ProcessesViewModel : TabItemViewModelBase {
        Dictionary<int, ProcessViewModel> _processMap;

        public ProcessesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/processes.ico").ToString();
        }

        public IEnumerable<ProcessViewModel> Processes {
            get {
                var processes = SystemInformation.EnumProcesses().Select(process => new ProcessViewModel(process)).ToArray();
                _processMap = processes.ToDictionary(process => process.Id);
                return processes;
            }
        }

        public string GetProcessName(int pid) {
            if (_processMap == null) {
                var processes = Processes;
            }
            return _processMap.TryGetValue(pid, out var process) ? process.Name : null;
        }
    }
}
