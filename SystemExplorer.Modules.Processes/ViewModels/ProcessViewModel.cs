using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    class ProcessViewModel : BindableBase {
        readonly ProcessInfo _info;

        public ProcessViewModel(ProcessInfo info) {
            _info = info;
        }

        public int Id => _info.Id;
        public string Name => _info.Name;
    }
}
