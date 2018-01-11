using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;
using SystemExplorer.Modules.Computer.ViewModels;

namespace SystemExplorer.Modules.Computer.ViewModels {
    [Export]
    sealed class EnvironmentVariablesTreeViewItem : TreeViewItemBase {
        public EnvironmentVariablesTreeViewItem() {
            Text = "Environment Variables";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/variables.ico").ToString();
        }

        [Import]
        CompositionContainer Container;

        public override TabItemViewModelBase CreateTabItem() {
            return Container.GetExportedValue<EnvironmentVariablesViewModel>();
        }
    }
}
