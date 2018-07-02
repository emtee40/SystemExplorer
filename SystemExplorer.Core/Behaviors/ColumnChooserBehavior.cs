using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace SystemExplorer.Core.Behaviors {
    public sealed class ColumnChooserBehavior : Behavior<SfDataGrid> {

        public ColumnManager ColumnManager {
            get { return (ColumnManager)GetValue(ColumnManagerProperty); }
            set { SetValue(ColumnManagerProperty, value); }
        }

        public static readonly DependencyProperty ColumnManagerProperty =
            DependencyProperty.Register(nameof(ColumnManager), typeof(ColumnManager), typeof(ColumnChooserBehavior), 
                new PropertyMetadata(null, (s, e) => ((ColumnChooserBehavior)s).OnColumnChooserChanged(e)));



        public bool AutoAddColumns {
            get { return (bool)GetValue(AutoAddColumnsProperty); }
            set { SetValue(AutoAddColumnsProperty, value); }
        }

        public static readonly DependencyProperty AutoAddColumnsProperty =
            DependencyProperty.Register(nameof(AutoAddColumns), typeof(bool), typeof(ColumnChooserBehavior), new PropertyMetadata(false));


        private void OnColumnChooserChanged(DependencyPropertyChangedEventArgs e) {
            var chooser = (ColumnManager)e.NewValue;
            if (chooser != null) {
                foreach (var column in AssociatedObject.Columns) {
                    var ch = chooser.GetColumn(column.MappingName);
                    if (ch == null && AutoAddColumns) {
                        ch = new ColumnInfo(column.MappingName, column.HeaderText, true);
                        chooser.AddColumns(ch);
                    }
                    if (ch != null) {
                        var binding = new Binding(nameof(ch.IsHidden)) {
                            Source = ch
                        };
                        column.IsHidden = ch.IsHidden;
                        if (ch.Text == null)
                            ch.Text = column.HeaderText;
                        BindingOperations.SetBinding(column, GridColumnBase.IsHiddenProperty, binding);
                    }
                }
            }
        }
    }
}
