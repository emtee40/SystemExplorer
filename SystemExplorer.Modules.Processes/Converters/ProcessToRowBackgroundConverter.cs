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
            if (process.IsManaged)
                return ManagedBackground;
            if (process.IsProtected)
                return ProtectedBackground;
            if (process.IsInAnyJob)
                return InJobBackground;

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


        public Brush ManagedBackground {
            get { return (Brush)GetValue(ManagedBackgroundProperty); }
            set { SetValue(ManagedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ManagedBackgroundProperty =
            DependencyProperty.Register(nameof(ManagedBackground), typeof(Brush), typeof(ProcessToRowBackgroundConverter), new PropertyMetadata(new SolidColorBrush(Colors.Yellow) { Opacity = .5 }));



        public Brush InJobBackground {
            get { return (Brush)GetValue(InJobBackgroundProperty); }
            set { SetValue(InJobBackgroundProperty, value); }
        }

        public static readonly DependencyProperty InJobBackgroundProperty =
            DependencyProperty.Register(nameof(InJobBackground), typeof(Brush), typeof(ProcessToRowBackgroundConverter), new PropertyMetadata(new SolidColorBrush(Colors.Brown) { Opacity = .5 }));



        public Brush ProtectedBackground {
            get { return (Brush)GetValue(ProtectedBackgroundProperty); }
            set { SetValue(ProtectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ProtectedBackgroundProperty =
            DependencyProperty.Register(nameof(ProtectedBackground), typeof(Brush), typeof(ProcessToRowBackgroundConverter), new PropertyMetadata(new SolidColorBrush(Colors.Purple) { Opacity = .5 }));



    }
}
