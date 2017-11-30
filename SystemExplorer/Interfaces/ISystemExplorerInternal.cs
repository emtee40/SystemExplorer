using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Interfaces {
    interface ISystemExplorerInternal {
        void RemoveModuleResources(IModule module);
    }
}
