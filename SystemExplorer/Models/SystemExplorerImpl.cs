using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SystemExplorer.Core;
using SystemExplorer.Interfaces;

namespace SystemExplorer.Models {
    [Export(typeof(ISystemExplorer)), Export(typeof(ISystemExplorerInternal))]
    sealed class SystemExplorerImpl : ISystemExplorer, ISystemExplorerInternal {
        [Import]
        ModuleManager _moduleManager;

        [Import]
        ITabManager _tabManager;

        [Import]
        ITreeManager _treeManager;

        Dictionary<Type, (Type viewType, IModule module)> _viewModelToViewMapping = new Dictionary<Type, (Type viewType, IModule module)>(8);
        List<(IModule module, ResourceDictionary rd)> _resourceDictionaries = new List<(IModule module, ResourceDictionary rd)>(8);
        List<(IModule module, MenuItemViewModel menu)> _moduleTopLevelMenu = new List<(IModule module, MenuItemViewModel menu)>(8);
        List<(IModule module, TreeViewItemBase item)> _treeItems = new List<(IModule module, TreeViewItemBase item)>(4);

        public void AddResourceDictionary(ResourceDictionary resourceDictionary, IModule module) {
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            _resourceDictionaries.Add((module, resourceDictionary));
        }

        public void AddTabViewMapping<TViewModel, TView>(IModule module)
            where TViewModel : TabItemViewModelBase
            where TView : FrameworkElement {
            _viewModelToViewMapping.Add(typeof(TViewModel), (typeof(TView), module));
        }

        public void AddTopLevelMenuItem(MenuItemViewModel menuItem, IModule module) {
            
        }

        public void AddTreeViewItem(TreeViewItemBase treeViewItem, IModule module) {
            _treeManager.AddTreeItem(treeViewItem);
            _treeItems.Add((module, treeViewItem));
        }

        public void RemoveModuleResources(IModule module) {
        }

        
    }
}
