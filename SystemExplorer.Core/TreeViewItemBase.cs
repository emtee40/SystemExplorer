using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
    public abstract class TreeViewItemBase : BindableBase {
        IEnumerable<MenuItemViewModel> _contextMenu;
        ObservableCollection<TreeViewItemBase> _items;

        string _text, _icon;
        bool _isExpanded, _isSelected;

        public IEnumerable<MenuItemViewModel> ContextMenu { get => _contextMenu; set => SetProperty(ref _contextMenu, value); }
        public string Text { get => _text; set => SetProperty(ref _text, value); }
        public string Icon { get => _icon; set => SetProperty(ref _icon, value); }

        public virtual TabItemViewModelBase CreateTabItem() => null;

        public IList<TreeViewItemBase> Items => _items ?? (_items = new ObservableCollection<TreeViewItemBase>());

        public bool IsExpanded { get => _isExpanded; set => SetProperty(ref _isExpanded, value); }
        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

        protected TreeViewItemBase() {
            var item = GetType().GetCustomAttribute<ItemAttribute>();
            if (item != null) {
                Text = item.Text;
                Icon = item.Icon;
            }
        }
    }
}
