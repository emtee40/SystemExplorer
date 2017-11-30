using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
    public abstract class TreeViewItemBase : BindableBase {
        IEnumerable<MenuItemViewModel> _contextMenu;
        ObservableCollection<TreeViewItemBase> _items;

        string _text, _icon;

        public IEnumerable<MenuItemViewModel> ContextMenu { get => _contextMenu; set => SetProperty(ref _contextMenu, value); }
        public string Text { get => _text; set => SetProperty(ref _text, value); }
        public string Icon { get => _icon; set => SetProperty(ref _icon, value); }

        public virtual TabItemViewModelBase CreateTabItem() => null;

        public IList<TreeViewItemBase> Items => _items ?? (_items = new ObservableCollection<TreeViewItemBase>());
    }
}
