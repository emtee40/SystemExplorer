using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SystemExplorer.Core;
using SystemExplorer.Modules.Computer.ViewModels;

namespace SystemExplorer.Modules.Computer.ViewModels {
    [Export, Item(Text = "Computer")]
    sealed class ComputerTreeViewItem : TreeViewItemBase {

        [Import]
        CompositionContainer Container;

        public ComputerTreeViewItem() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/computer.ico").ToString();
        }

        public override TabItemViewModelBase CreateTabItem() {
            return Container.GetExportedValue<ComputerViewModel>();
        }
    }
}
