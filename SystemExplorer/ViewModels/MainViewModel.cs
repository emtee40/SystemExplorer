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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        }

        public IList<TabItemViewModelBase> TabItems => _tabItems;

        public string Title => Constants.Title + (Helpers.IsAdmin ? " (Administrator) " : (Helpers.IsLocalSystem ? " (System) " : " ")) + Constants.Copyright;

        TabItemViewModelBase _selectedTab;
        int _selectedTabIndex;

        public int SelectedTabIndex {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public TabItemViewModelBase SelectedTab {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public void AddTab(TabItemViewModelBase item, bool select = false) {
            TabItems.Add(item);
            if (select)
                SelectedTab = item;
        }

        public ICommand TabClosedCommand => new DelegateCommand<CloseTabEventArgs>(args => {
            var tab = args.TargetTabItem.DataContext as TabItemViewModelBase;
            Debug.Assert(tab != null);
            RemoveTab(tab);
        });

        public ICommand TabClosingCommand => new DelegateCommand<CancelingRoutedEventArgs>(args => args.Cancel = TabItems.Count <= 1);

        public void RemoveTab(TabItemViewModelBase item) {
            TabItems.Remove(item);
        }

        public IEnumerable<MenuItemViewModel> MenuItems => _menuItems ?? (_menuItems = CreateMenuItems());

        public ICommand ExitCommand => new DelegateCommand(() => Application.Current.Shutdown());

        private ObservableCollection<MenuItemViewModel> CreateMenuItems() {
            return new ObservableCollection<MenuItemViewModel> {
                new MenuItemViewModel {
                    Text = "_File",
                    Items = {
                        new MenuItemViewModel { Text = "E_xit", Command = ExitCommand }
                    }
                },
                new MenuItemViewModel {
                    Text = "_Edit",
                    Items = {
                        new MenuItemViewModel { Text = "_Copy", Icon = Icons.Copy, GestureText = "Ctrl+C" },
                        new MenuItemViewModel { Text = "C_ut", Icon = Icons.Cut, GestureText = "Ctrl+X" },
                        new MenuItemViewModel { Text = "_Paste", Icon = Icons.Paste, GestureText = "Ctrl+V" },
                    }
                },
                new MenuItemViewModel {
                    Text = "_Help",
                    Items = {
                        new MenuItemViewModel { Text = "_About System Explorer..." }
                    }
                },
            };
        }

        public void OnImportsSatisfied() {
        }

        TreeViewItemBase _selectedTreeItem;
        public TreeViewItemBase SelectedTreeItem {
            get => _selectedTreeItem;
            set {
                if (SetProperty(ref _selectedTreeItem, value)) {
                    if (value != null) {
                        TabItemViewModelBase tab;
                        if (_tabItems.Count == 0) {
                            tab = value.CreateTabItem();
                            AddTab(tab, true);
                        }
                        else {
                            int index = SelectedTabIndex;
                            _tabItems.RemoveAt(index);
                            _tabItems.Insert(index, value.CreateTabItem());
                        }

                    }
                }
            }
        }

        public void AddTreeItem(TreeViewItemBase item) {
            _treeItems.Add(item);
            if (SelectedTreeItem == null)
                SelectedTreeItem = item;
        }

        public void RemoveTreeItem(TreeViewItemBase item) {
            _treeItems.Remove(item);
        }
    }
}
