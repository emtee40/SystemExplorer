using Syncfusion.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;
using SystemExplorer.Modules.Computer.ViewModels;

namespace SystemExplorer.Modules.Computer.ViewModels {
    [Export, Item(Text = "Environment Variables")]
    sealed class EnvironmentVariablesViewModel : TabItemViewModelBase {
        public EnvironmentVariablesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly().GetName().Name, "/icons/variables.ico");
        }

        ObservableCollection<VariableViewModel> _variables;

        public IEnumerable<VariableViewModel> Variables {
            get {
                if (_variables == null) {
                    var vars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
                    _variables = new ObservableCollection<VariableViewModel>();
                    foreach (DictionaryEntry item in vars) {
                        _variables.Add(new VariableViewModel((string)item.Key, (string)item.Value));
                    }
                }
                return _variables;
            }
        }

        public bool IsAdmin => Helpers.IsAdmin;

        public ICollectionViewAdv View { get; set; }

        string _filterText;
        public string FilterText {
            get => _filterText;
            set {
                if (SetProperty(ref _filterText, value)) {
                    if (string.IsNullOrWhiteSpace(value))
                        View.Filter = null;
                    else {
                        var text = value.ToLower();
                        View.Filter = obj => {
                            var variable = (VariableViewModel)obj;
                            return variable.Name.ToLower().Contains(text) || variable.Value.ToLower().Contains(text);
                        };
                    }
                    View.RefreshFilter();
                }
            }
        }
    }
}
