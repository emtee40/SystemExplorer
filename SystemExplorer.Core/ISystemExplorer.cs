using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemExplorer.Core {
    public interface ISystemExplorer {
        void AddResourceDictionary(ResourceDictionary resourceDictionary, IModule module);

        void AddTreeViewItem(TreeViewItemBase treeViewItem, IModule module);
        CompositionContainer Container { get; }
    }

}
