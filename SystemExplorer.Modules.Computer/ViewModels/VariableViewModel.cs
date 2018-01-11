using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Modules.Computer.ViewModels {
    sealed class VariableViewModel : BindableBase {
        public string Name { get; }

        string _value;
        public string Value {
            get => _value;
            set {
                if (SetProperty(ref _value, value)) {
                    Environment.SetEnvironmentVariable(Name, value, EnvironmentVariableTarget.Machine);
                }
            }
        }

        public VariableViewModel(string name, string value) {
            Name = name;
            _value = value;
        }

        int _lines;
        public int Lines {
            get {
                if (_lines == 0) {
                    var result = ValueFormatted;
                }
                return _lines;
            }
        }

        string _valueFormatted;
        public string ValueFormatted {
            get {
                if (_valueFormatted == null) {
                    var lines = Value.Split(';');
                    _lines = lines.Length;
                    _valueFormatted = string.Join(";" + Environment.NewLine, lines);
                }
                return _valueFormatted;
            }
        }

    }
}
