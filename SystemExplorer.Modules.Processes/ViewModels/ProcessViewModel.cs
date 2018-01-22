using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Zodiacon.ManagedWindows.Core;

namespace SystemExplorer.Modules.Processes.ViewModels {
    class ProcessViewModel : BindableBase {
        static Brush _activityBrush = new SolidColorBrush(Colors.Red) { Opacity = .5 };
        static Brush _decreaseActivityBrush = new SolidColorBrush(Colors.Green) { Opacity = .5 };

        public ProcessExtendedInformation Info { get; private set; }

        public ProcessViewModel(ProcessExtendedInformation info) {
            Info = info;
        }

        public TimeSpan TotalTime => Info.UserTime + Info.KernelTime;
        public TimeSpan UserTime => Info.UserTime;
        public TimeSpan KernelTime => Info.KernelTime;
        public int ThreadCount => Info.ThreadCount;
        public int HandleCount => Info.HandleCount;
        public long PrivateWorkingSetSize => Info.PrivateWorkingSetSize >> 10;
        public long WorkingSetSize => Info.WorkingSetSize >> 10;

        public Brush CpuTimeBackground { get => _cpuTimeBackground; set => SetProperty(ref _cpuTimeBackground, value); }
        public Brush KernelTimeBackground { get => _kernelTimeBackground; set => SetProperty(ref _kernelTimeBackground, value); }
        public Brush WorkingSetSizeBackground { get => _workingSetSizeBackground; set => SetProperty(ref _workingSetSizeBackground, value); }
        public Brush PrivateWorkingSetSizeBackground { get => _privateWorkingSetSizeBackground; set => SetProperty(ref _privateWorkingSetSizeBackground, value); }
        public Brush UserTimeBackground { get => _userTimeBackground; set => SetProperty(ref _userTimeBackground, value); }

        internal void Update(ProcessExtendedInformation info) {
            var oldInfo = Info;
            Info = info;
            RaisePropertyChanged(nameof(TotalTime));
            RaisePropertyChanged(nameof(HandleCount));
            RaisePropertyChanged(nameof(KernelTime));
            RaisePropertyChanged(nameof(UserTime));
            RaisePropertyChanged(nameof(ThreadCount));
            RaisePropertyChanged(nameof(WorkingSetSize));
            RaisePropertyChanged(nameof(PrivateWorkingSetSize));

            CpuTimeBackground = SignToBrush(TotalTime.Ticks - (oldInfo.KernelTime + oldInfo.UserTime).Ticks);
            KernelTimeBackground = SignToBrush(KernelTime.Ticks - oldInfo.KernelTime.Ticks);
            UserTimeBackground = SignToBrush(UserTime.Ticks - oldInfo.UserTime.Ticks);

            PrivateWorkingSetSizeBackground = SignToBrush(PrivateWorkingSetSize - (oldInfo.PrivateWorkingSetSize >> 10));
            WorkingSetSizeBackground = SignToBrush(WorkingSetSize - (oldInfo.WorkingSetSize >> 10));
        }

        Brush SignToBrush(long sign) {
            if (sign > 0)
                return _activityBrush;
            else if (sign < 0)
                return _decreaseActivityBrush;
            return Brushes.Transparent;
        }

        Brush _cpuTimeBackground, _kernelTimeBackground, _userTimeBackground, _workingSetSizeBackground, _privateWorkingSetSizeBackground;
    }
}
