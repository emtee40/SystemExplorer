using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;
using ThreadState = Zodiacon.ManagedWindows.Core.ThreadState;

namespace SystemExplorer.Modules.Processes.ViewModels {
    sealed class ThreadViewModel : BindableBase, IDisposable {
        public ThreadExtendedInformation Info { get; private set; }
        NativeThread _nativeThread;
        static Brush _activityBrush = new SolidColorBrush(Colors.Red) { Opacity = .5 };
        static Brush _decreaseActivityBrush = new SolidColorBrush(Colors.Green) { Opacity = .5 };
        static string[] _threadStateIcons = Enum.GetNames(typeof(ThreadState)).
            Select(name => Helpers.ToPackUri(Assembly.GetExecutingAssembly(), $"/icons/threadstates/{name}.ico").ToString()).ToArray();

        public ThreadViewModel(ThreadExtendedInformation info) {
            Info = info;

            _nativeThread = NativeThread.TryOpen(ThreadAccessMask.QueryLimitedInformation, info.ThreadId);
        }

        public void Refresh() {
            RaisePropertyChanged(nameof(KernelTime));
            RaisePropertyChanged(nameof(UserTime));
            RaisePropertyChanged(nameof(TotalTime));
        }

        public void Dispose() {
            _nativeThread?.Dispose();
        }

        public DateTime CreateTime => Info.CreateTime.ToLocalTime();

        public TimeSpan KernelTime => Info.KernelTime;
        public TimeSpan UserTime => Info.UserTime;
        public TimeSpan TotalTime => KernelTime + UserTime;

        public int ProcessId => Info.Process.ProcessId;

        public string ProcessName => Info.Process.ImageName;
        public int BasePriority => Info.BasePriority;
        public int Priority => Info.Priority;

        ThreadState? _state;
        public ThreadState State => _state ?? (ThreadState)(_state = Info.State);

        public bool IsDead {
            get => _isDead;
            set {
                if (SetProperty(ref _isDead, value))
                    RaisePropertyChanged(nameof(Self));
            }
        }

        Stopwatch _stopwatch;
        public bool IsCreated {
            get => _isCreated;
            set {
                if (SetProperty(ref _isCreated, value)) {
                    if (value)
                        _stopwatch = Stopwatch.StartNew();
                    else
                        _stopwatch.Stop();
                }
            }
        }

        public ThreadViewModel Self => this;

        internal void Update(ThreadExtendedInformation info) {
            var oldInfo = Info;
            Info = info;
            RaisePropertyChanged(nameof(TotalTime));
            RaisePropertyChanged(nameof(KernelTime));
            RaisePropertyChanged(nameof(UserTime));
            _state = null;
            RaisePropertyChanged(nameof(State));
            RaisePropertyChanged(nameof(Icon));
            RaisePropertyChanged(nameof(WaitReason));
            RaisePropertyChanged(nameof(BasePriority));
            RaisePropertyChanged(nameof(Priority));

            CpuTimeBackground = SignToBrush(TotalTime.Ticks - (oldInfo.KernelTime + oldInfo.UserTime).Ticks);
            KernelTimeBackground = SignToBrush(KernelTime.Ticks - oldInfo.KernelTime.Ticks);
            UserTimeBackground = SignToBrush(UserTime.Ticks - oldInfo.UserTime.Ticks);

            if (IsCreated && _stopwatch.ElapsedMilliseconds > 3000) {
                IsCreated = false;
            }

            RaisePropertyChanged(nameof(Self));
        }

        Brush SignToBrush(long sign) {
            if (sign > 0)
                return _activityBrush;
            else if (sign < 0)
                return _decreaseActivityBrush;
            return Brushes.Transparent;
        }

        public Brush CpuTimeBackground { get => _cpuTimeBackground; set => SetProperty(ref _cpuTimeBackground, value); }
        public Brush KernelTimeBackground { get => _kernelTimeBackground; set => SetProperty(ref _kernelTimeBackground, value); }
        public Brush UserTimeBackground { get => _userTimeBackground; set => SetProperty(ref _userTimeBackground, value); }

        public string Icon => _threadStateIcons[(int)State];
        public string WaitReason => State == ThreadState.Waiting ? Info.WaitReason.ToString() : default;

        Brush _cpuTimeBackground, _kernelTimeBackground, _userTimeBackground;
        bool _isDead, _isCreated;
    }
}
