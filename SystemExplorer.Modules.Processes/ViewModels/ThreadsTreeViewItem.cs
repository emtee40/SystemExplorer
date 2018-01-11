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
    sealed class ThreadsTreeViewItem : TreeViewItemBase {
        [Import]
        CompositionContainer _container;

        public ThreadsTreeViewItem() {
            Text = "Threads";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/threads.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() {
            return _container.GetExportedValue<ThreadsViewModel>();
        }
    }
}
