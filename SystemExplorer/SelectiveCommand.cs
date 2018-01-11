using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemExplorer.ViewModels;

namespace SystemExplorer {
    sealed class SelectiveCommand : DelegateCommandBase {
        readonly MainViewModel _vm;
        readonly string _commandProperty;

        public SelectiveCommand(MainViewModel vm, string commandProperty) {
            _vm = vm;
            _commandProperty = commandProperty;
        }

        protected override bool CanExecute(object parameter) {
            if (_vm.SelectedTab != null) {
                var cmd = GetCommand(_vm.SelectedTab);
                if (cmd != null)
                    return cmd.CanExecute(parameter);
            }
            var cmd2 = GetCommand(_vm);
            if (cmd2 != null)
                return cmd2.CanExecute(parameter);

            return false;
        }

        protected override void Execute(object parameter) {
            if (_vm.SelectedTab != null) {
                var cmd = GetCommand(_vm.SelectedTab);
                if (cmd != null) {
                    cmd.Execute(parameter);
                    return;
                }
            }
            var cmd2 = GetCommand(_vm);
            if (cmd2 != null)
                cmd2.Execute(parameter);
        }

        ICommand GetCommand(object obj) {
            var property = obj.GetType().GetProperty(_commandProperty);
            if (property == null)
                return null;

            return property.GetValue(obj) as ICommand;
        }
    }
}
