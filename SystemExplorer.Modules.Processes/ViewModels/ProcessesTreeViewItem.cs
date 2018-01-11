using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    [Export]
    sealed class ProcessesTreeViewItem : TreeViewItemBase {
        [Import]
        CompositionContainer _container;

        public ProcessesTreeViewItem() {
            Text = "Processes";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/processes.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() => _container.GetExportedValue<ProcessesViewModel>();
    }
}
