using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    [Export, Item(Text = "Processes")]
    sealed class HandlesViewModel : TabItemViewModelBase {
        public HandlesViewModel() {

        }
    }
}

