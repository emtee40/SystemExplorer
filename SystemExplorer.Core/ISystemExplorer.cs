using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemExplorer.Core {
    public interface ISystemExplorer {
        void AddResourceDictionary(ResourceDictionary resourceDictionary, IModule module);
        void AddTopLevelMenuItem(MenuItemViewModel menuItem, IModule module);
        //void AddTabViewMapping<TViewModel, TView>(IModule module) 
        //    where TView : FrameworkElement 
        //    where TViewModel : TabItemViewModelBase;

        void AddTreeViewItem(TreeViewItemBase treeViewItem, IModule module);
    }

}
