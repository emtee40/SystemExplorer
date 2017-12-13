using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;
using SystemExplorer.Modules.Computer.ViewModels;

namespace SystemExplorer.Modules.Computer {
    sealed class EnvironmentVariablesTreeViewItem : TreeViewItemBase {
        public EnvironmentVariablesTreeViewItem() {
            Text = "Environment Variables";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/variables.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() {
            return new EnvironmentVariablesViewModel();
        }
    }
}
