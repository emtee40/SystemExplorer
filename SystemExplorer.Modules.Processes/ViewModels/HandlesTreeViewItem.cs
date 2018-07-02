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
    sealed class HandlesTreeViewItem : TreeViewItemBase {
        public HandlesTreeViewItem() {
            Text = "Handles";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/handles.ico").ToString();
        }

        [Import]
        CompositionContainer Container;

        public override TabItemViewModelBase CreateTabItem() {
            return Container.GetExportedValue<HandlesViewModel>();
        }
    }
}
