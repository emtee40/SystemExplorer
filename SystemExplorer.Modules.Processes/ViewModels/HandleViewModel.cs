using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    sealed class HandleViewModel {
        public readonly SystemHandleInfo Info;

        public HandleViewModel(SystemHandleInfo info) {
            Info = info;
        }

        public long Object => Info.Object.ToInt64();
        public int Handle => Info.Handle;
        public int TypeIndex => Info.ObjectTypeIndex;
        public int ProcessId => Info.ProcessId;
        public uint AccessMask => Info.AccessMask;
        public HandleAttributes Attributes => (HandleAttributes)Info.Attributes;
        public string TypeName { get; }

    }
}
