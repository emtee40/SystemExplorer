using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    class ThreadViewModel : BindableBase {
        readonly ThreadInfo _info;
        readonly ProcessesViewModel _processes;

        public ThreadViewModel(ProcessesViewModel processes, ThreadInfo info) {
            _info = info;
            _processes = processes;
        }

        public int Id => _info.Id;
        public int ProcessId => _info.ProcessId;
        public string ProcessName => _processes.GetProcessName(_info.ProcessId);
        public int BasePriority => _info.BasePriority;

        public int State => 0;

        public void Refresh() {
        }
    }
}
