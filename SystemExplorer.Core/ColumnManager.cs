using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
    [Serializable]
    public sealed class ColumnInfo : BindableBase {
        bool _isEnabled = true, _isHidden = false;
        string _name, _text;

        public ColumnInfo(string name, string text = null, bool isHidden = false) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsHidden = isHidden;
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }
        public bool IsHidden {
            get => _isHidden;
            set {
                if (SetProperty(ref _isHidden, value)) {
                    RaisePropertyChanged(nameof(IsVisible));
                }
            }
        }

        public bool IsVisible { get => !IsHidden; set => IsHidden = !value; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ColumnInfoAttribute : Attribute {
        public ColumnInfoAttribute(string name) {
            Name = name;
        }

        public string Name { get; }
        public string Text { get; set; }
        public bool IsHidden { get; set; }
        public bool IsEnabled { get; set; } = true;
    }

    public class ColumnManager : IEnumerable<ColumnInfo> {
        readonly List<ColumnInfo> _columns;
        readonly Dictionary<string, ColumnInfo> _dcolumns;
        public IReadOnlyList<ColumnInfo> Columns => _columns;

        public ColumnManager(int capacity = 8) {
            _columns = new List<ColumnInfo>(capacity);
            _dcolumns = new Dictionary<string, ColumnInfo>(capacity, StringComparer.InvariantCultureIgnoreCase);
        }

        public ColumnInfo GetColumn(string name) => _dcolumns.TryGetValue(name, out var info) ? info : null;

        public bool this[string name] {
            get => _dcolumns.TryGetValue(name, out var column) ? column.IsHidden : false;
            set => _dcolumns[name].IsHidden = value;
        }

        public void BuildFromType(Type type) {
            foreach (var attr in type.GetCustomAttributes(true).OfType<ColumnInfoAttribute>()) {
                AddColumns(new ColumnInfo(attr.Name, attr.Text, attr.IsHidden) { IsEnabled = attr.IsEnabled });
            }
        }

        public void AddVisibleColumns(params (string name, string text)[] columns) {
            AddColumns(columns.Select(name => new ColumnInfo(name.name, name.text)).ToArray());
        }

        public void AddHiddenColumns(params (string name, string text)[] columns) {
            AddColumns(columns.Select(name => new ColumnInfo(name.name, name.text, true)).ToArray());
        }

        public void AddColumns(params ColumnInfo[] columns) {
            _columns.AddRange(columns);
            foreach (var column in columns)
                _dcolumns.Add(column.Name, column);
        }

        public IEnumerator<ColumnInfo> GetEnumerator() => _columns.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
