using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SystemExplorer.Core {
    public class MenuItemViewModel : BindableBase {
        string _text, _icon;
        bool _isCheckable, _isChecked, _isVisible = true;
        ICommand _command;
        object _commandParameter;
        ObservableCollection<MenuItemViewModel> _items;

        public string Text { get => _text; set => SetProperty(ref _text, value); }
        public string Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public bool IsCheckable { get => _isCheckable; set => SetProperty(ref _isCheckable, value); }
        public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked, value); }
        public ICommand Command { get => _command; set => SetProperty(ref _command, value); }
        public object CommandParameter { get => _commandParameter; set => SetProperty(ref _commandParameter, value); }

        public IList<MenuItemViewModel> Items => _items ?? (_items = new ObservableCollection<MenuItemViewModel>());

        public bool IsSeparator { get; set; }
    }
}
