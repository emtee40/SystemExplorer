using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using SystemExplorer.Modules.Processes.ViewModels;

namespace SystemExplorer.Modules.Processes.Converters {
    sealed class ProcessToRowBackgroundConverter : DependencyObject, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var process = (ProcessViewModel)value;

            if (process.IsDead)
                return Brushes.Red;
            if (process.IsCreated)
                return Brushes.LightGreen;
            if (process.IsImmersive == true)
                return ImmersiveBackground;

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }



        public Brush ImmersiveBackground {
            get { return (Brush)GetValue(ImmersiveBackgroundProperty); }
            set { SetValue(ImmersiveBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ImmersiveBackgroundProperty =
            DependencyProperty.Register(nameof(ImmersiveBackground), typeof(Brush), typeof(ProcessToRowBackgroundConverter), 
                new PropertyMetadata(new SolidColorBrush(Colors.Cyan) { Opacity = .5 }));


    }
}
