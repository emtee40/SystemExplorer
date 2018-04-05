using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SystemExplorer.Core;
using SystemExplorer.Interfaces;
using SystemExplorer.Models;
using SystemExplorer.Resources;
using Zodiacon.WPF;

namespace SystemExplorer.ViewModels {
    [Export, Export(typeof(ITabManager)), Export(typeof(ITreeManager)), PartCreationPolicy(CreationPolicy.Shared)]
    sealed class MainViewModel : BindableBase, ITabManager, IPartImportsSatisfiedNotification, ITreeManager {
        ObservableCollection<TabItemViewModelBase> _tabItems = new ObservableCollection<TabItemViewModelBase>();
        ObservableCollection<MenuItemViewModel> _menuItems;
        ObservableCollection<TreeViewItemBase> _treeItems = new ObservableCollection<TreeViewItemBase>();
        Dictionary<TreeViewItemBase, TabItemViewModelBase> _treeItemsToTabs = new Dictionary<TreeViewItemBase, TabItemViewModelBase>(16);
        Dictionary<TabItemViewModelBase, TreeViewItemBase> _tabsToTreeItems = new Dictionary<TabItemViewModelBase, TreeViewItemBase>(16);

        public static readonly ICommand DisabledCommand = new DelegateCommand(() => { }, () => false);

        public IList<TreeViewItemBase> TreeItems => _treeItems;

        [Import]
        public IUIServices UI;

        [Import]
        public ModuleManager ModuleManager;

        [Import]
        public ISystemExplorer Explorer;

        [Import]
        ISystemExplorerInternal ExplorerInternal;

        public MainViewModel() {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        public IList<TabItemViewModelBase> TabItems => _tabItems;

        public string Title => $"{Constants.Title} {Constants.Version}" + (Helpers.IsAdmin ? " (Administrator) " : (Helpers.IsLocalSystem ? " (System) " : " ")) + Constants.Copyright;

        TabItemViewModelBase _selectedTab;
        int _selectedTabIndex;

        public int SelectedTabIndex {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public TabItemViewModelBase SelectedTab {
            get => _selectedTab;
            set {
                if (value != _selectedTab) {
                    if (_selectedTab is IActiveAware inactive)
                        inactive.IsActive = false;
                    SetProperty(ref _selectedTab, value);
                    if (value is IActiveAware active)
                        active.IsActive = true;
                }
            }
        }

        public void AddTab(TabItemViewModelBase item, bool select = false) {
            TabItems.Add(item);
            if (select)
                SelectedTab = item;
        }

        public ICommand TabClosedCommand => new DelegateCommand<CloseTabEventArgs>(args => {
            var tab = args.TargetTabItem.DataContext as TabItemViewModelBase;
            Debug.Assert(tab != null);
            if (tab.CanClose) {
                RemoveTab(tab);
            }
        });

        public ICommand TabClosingCommand => new DelegateCommand<CancelingRoutedEventArgs>(args => args.Cancel = TabItems.Count <= 1);

        public void RemoveTab(TabItemViewModelBase item) {
            TabItems.Remove(item);
            item.OnClose();
            var treeItem = _tabsToTreeItems[item];
            _tabsToTreeItems.Remove(item);
            _treeItemsToTabs.Remove(treeItem);
        }

        public ICommand ExitCommand => new DelegateCommand(() => Application.Current.Shutdown());

        public void OnImportsSatisfied() {
            foreach(var item in _treeItems) {
                if (item.Text == "Processes") {
                    SwitchToTab(item);
                    break;
                }
            }
        }

        TreeViewItemBase _selectedTreeItem;
        public TreeViewItemBase SelectedTreeItem {
            get => _selectedTreeItem;
            set => SetProperty(ref _selectedTreeItem, value);
        }

        public void AddTreeItem(TreeViewItemBase item) {
            _treeItems.Add(item);
            if (SelectedTreeItem == null)
                SelectedTreeItem = item;
        }

        public void RemoveTreeItem(TreeViewItemBase item) {
            _treeItems.Remove(item);
        }

        async void SwitchToTab(TreeViewItemBase item) {
            if (_treeItemsToTabs.TryGetValue(item, out var tab)) {
                SelectedTab = tab;
            }
            else {
                var vm = await Dispatcher.CurrentDispatcher.InvokeAsync(() => item.CreateTabItem(), DispatcherPriority.Background);
                if (vm == null)
                    return;

                AddTab(vm, true);
                _treeItemsToTabs.Add(item, vm);
                _tabsToTreeItems.Add(vm, item);
            }
        }

        public ICommand SwitchToTabCommand => new DelegateCommand<EventArgs>(args => {
            Debug.Assert(SelectedTreeItem != null);
            SwitchToTab(SelectedTreeItem);
            SelectedTreeItem.IsExpanded = !SelectedTreeItem.IsExpanded;
        }, args => SelectedTreeItem != null).ObservesProperty(() => SelectedTreeItem);

        bool _isBusy;
        public bool IsBusy {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
