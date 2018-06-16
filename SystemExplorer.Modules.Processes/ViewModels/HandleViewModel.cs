using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    sealed class HandleViewModel {
        public readonly SystemHandleInformation Info;
        public KernelObjectType ObjectType { get; }

        public HandleViewModel(SystemHandleInformation info) {
            Info = info;
            ObjectType = SystemInformation.GetKernelObjectTypeByIndex(info.ObjectTypeIndex);
        }

        public ulong Object => Info.Object.ToUInt64();
        public int Handle => Info.Handle;
        public int TypeIndex => Info.ObjectTypeIndex;
        public int ProcessId => Info.ProcessId;
        public uint AccessMask => Info.AccessMask;
        public HandleAttributes Attributes => (HandleAttributes)Info.Attributes;
        public string TypeName => ObjectType.Name;

    }
}
