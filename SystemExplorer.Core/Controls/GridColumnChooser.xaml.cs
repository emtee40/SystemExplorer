using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemExplorer.Core.Controls {
    /// <summary>
    /// Interaction logic for GridColumnChooser.xaml
    /// </summary>
    public partial class GridColumnChooser {
        public GridColumnChooser() {
            InitializeComponent();
        }

        public ColumnManager ColumnManager {
            get { return (ColumnManager)GetValue(ColumnManagerProperty); }
            set { SetValue(ColumnManagerProperty, value); }
        }

        public static readonly DependencyProperty ColumnManagerProperty =
            DependencyProperty.Register(nameof(ColumnManager), typeof(ColumnManager), typeof(GridColumnChooser), new PropertyMetadata(null));

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(GridColumnChooser), new PropertyMetadata(string.Empty));



    }
}
