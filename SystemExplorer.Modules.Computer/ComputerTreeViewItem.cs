using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SystemExplorer.Core;
using SystemExplorer.Modules.Computer.ViewModels;

namespace SystemExplorer.Modules.Computer {
    class ComputerTreeViewItem : TreeViewItemBase {

        public ComputerTreeViewItem() {
            Text = "Computer";
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/computer.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() {
            return new ComputerViewModel();
        }
    }
}
