using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace SystemExplorer.Core.Behaviors {
    public sealed class AutoGridRowHeightBehavior : Behavior<SfDataGrid> {
        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.QueryRowHeight += AssociatedObject_QueryRowHeight;
        }


        public GridRowSizingOptions GridRowSizingOptions {
            get { return (GridRowSizingOptions)GetValue(GridRowSizingOptionsProperty); }
            set { SetValue(GridRowSizingOptionsProperty, value); }
        }

        public static readonly DependencyProperty GridRowSizingOptionsProperty =
            DependencyProperty.Register(nameof(GridRowSizingOptions), typeof(GridRowSizingOptions), typeof(AutoGridRowHeightBehavior), 
                new PropertyMetadata(new GridRowSizingOptions()));

        public double MaxRowHeight {
            get { return (double)GetValue(MaxRowHeightProperty); }
            set { SetValue(MaxRowHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxRowHeightProperty =
            DependencyProperty.Register(nameof(MaxRowHeight), typeof(double), typeof(AutoGridRowHeightBehavior), new PropertyMetadata(100.0));


        private void AssociatedObject_QueryRowHeight(object sender, QueryRowHeightEventArgs e) {
            if (AssociatedObject.GridColumnSizer.GetAutoRowHeight(e.RowIndex, GridRowSizingOptions, out var height)) {
                e.Height = Math.Min(MaxRowHeight, height) + 4;
                e.Handled = true;
            }
        }
    }
}
