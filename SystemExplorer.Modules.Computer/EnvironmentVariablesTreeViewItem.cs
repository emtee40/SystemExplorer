using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Modules.Computer {
    sealed class EnvironmentVariablesTreeViewItem : TreeViewItemBase {
        public EnvironmentVariablesTreeViewItem() {
            Text = "Env. Variables";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/variables.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() {
            return new EnvironmentVariablesViewModel();
        }
    }
}
