using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemExplorer.Core;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace SystemExplorer.Modules.Processes.ViewModels {
    [ColumnInfo(nameof(CPU))]
    [ColumnInfo(nameof(TotalTime), IsHidden = true)]
    [ColumnInfo(nameof(KernelTime), IsHidden = true)]
    [ColumnInfo(nameof(UserTime), IsHidden = true)]
    [ColumnInfo(nameof(ThreadCount))]
    [ColumnInfo(nameof(HandleCount))]
    [ColumnInfo(nameof(WorkingSetSize), IsHidden = true)]
    [ColumnInfo(nameof(PrivateWorkingSetSize), IsHidden = true)]
    [ColumnInfo(nameof(BasePriority), IsHidden = true)]
    [ColumnInfo(nameof(VirtualSize), IsHidden = true)]
    [ColumnInfo(nameof(PeakVirtualSize), IsHidden = true)]
    [ColumnInfo(nameof(WriteOperationsCount), IsHidden = true, Text = "Write Operations")]
    [ColumnInfo(nameof(ReadOperationsCount), IsHidden = true, Text = "Read Operations")]
    class ProcessViewModel : BindableBase, IDisposable {
        static BitmapImage DefaultIcon = new BitmapImage(Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/icons/application.ico"));

        static Brush _activityBrush = new SolidColorBrush(Colors.Red) { Opacity = .5 };
        static Brush _decreaseActivityBrush = new SolidColorBrush(Colors.Green) { Opacity = .5 };
        readonly NativeProcess _nativeProcess;
        static int _totalProcessors = Environment.ProcessorCount;

        public ProcessExtendedInformation Info { get; private set; }

        public ProcessViewModel(ProcessExtendedInformation info) {
            Info = info;
            _nativeProcess = NativeProcess.TryOpen(ProcessAccessMask.QueryLimitedInformation, Info.ProcessId);
            if (_nativeProcess != null) {
                IsManaged = _nativeProcess.IsManaged;
                IsProtected = _nativeProcess.IsProtected;
                IsInAnyJob = _nativeProcess.IsInAnyJob;
            }
        }

        public string Name => Info.ImageName;
        public TimeSpan TotalTime => Info.UserTime + Info.KernelTime;
        public TimeSpan UserTime => Info.UserTime;
        public TimeSpan KernelTime => Info.KernelTime;
        public int ThreadCount => Info.ThreadCount;
        public int HandleCount => Info.HandleCount;
        public long PrivateWorkingSetSize => Info.PrivateWorkingSetSize >> 10;
        public long WorkingSetSize => Info.WorkingSetSize >> 10;
        public bool? IsImmersive => _nativeProcess?.IsImmersive;
        public int BasePriority => Info.BasePriority;
        public bool IsManaged { get; }
        public bool IsProtected { get; }
        public bool IsInAnyJob { get; }
        public long VirtualSize => Info.VirtualSize >> 10;
        public long WriteOperationsCount => Info.WriteOperationCount;
        public long ReadOperationsCount => Info.ReadOperationCount;
        public long PeakVirtualSize => Info.PeakVirtualSize >> 10;
        public BitmapSource Icon => ModuleHelpers.ExtractIcon(_nativeProcess?.FullImageName) ?? DefaultIcon;

        public ProcessPriorityClass? PriorityClass => _nativeProcess?.PriorityClass;

        long _lastTicks, _currentTicks;
        TimeSpan _lastTotal;
        double _cpu;
        public double CPU => _cpu;

        public void CalculateCPU() {
            if (_lastTicks == 0) {
                _lastTicks = _currentTicks;
                _lastTotal = Info.KernelTime + Info.UserTime;
                _cpu = 0;
                return;
            }

            var diff = _currentTicks - _lastTicks;
            double cpu = 0;
            if (diff != 0) {
                cpu = (Info.UserTime + Info.KernelTime - _lastTotal).Ticks * 100.0 / diff / _totalProcessors;
            }

            _lastTicks = _currentTicks;
            _lastTotal = Info.KernelTime + Info.UserTime;
            _cpu = cpu;
        }

        public Brush CpuTimeBackground { get => _cpuTimeBackground; set => SetProperty(ref _cpuTimeBackground, value); }
        public Brush KernelTimeBackground { get => _kernelTimeBackground; set => SetProperty(ref _kernelTimeBackground, value); }
        public Brush UserTimeBackground { get => _userTimeBackground; set => SetProperty(ref _userTimeBackground, value); }
        public Brush WorkingSetSizeBackground { get => _workingSetSizeBackground; set => SetProperty(ref _workingSetSizeBackground, value); }
        public Brush PrivateWorkingSetSizeBackground { get => _privateWorkingSetSizeBackground; set => SetProperty(ref _privateWorkingSetSizeBackground, value); }

        public ProcessViewModel Self => this;

        internal void Update(ProcessExtendedInformation info, long time) {
            var oldInfo = Info;
            Info = info;
            _currentTicks = time;
            bool timeChanged = false;
            if(oldInfo.HandleCount != info.HandleCount)
                RaisePropertyChanged(nameof(HandleCount));
            if (oldInfo.KernelTime != info.KernelTime) {
                RaisePropertyChanged(nameof(KernelTime));
                timeChanged = true;
            }
            if (oldInfo.KernelTime != info.UserTime) {
                RaisePropertyChanged(nameof(UserTime));
                timeChanged = true;
            }
            if(timeChanged)
                RaisePropertyChanged(nameof(TotalTime));

            if(info.ThreadCount != oldInfo.ThreadCount)
                RaisePropertyChanged(nameof(ThreadCount));

            if (info.WorkingSetSize != oldInfo.WorkingSetSize)
                RaisePropertyChanged(nameof(WorkingSetSize));

            if (info.PrivateWorkingSetSize!= oldInfo.PrivateWorkingSetSize)
                RaisePropertyChanged(nameof(PrivateWorkingSetSize));

            if (info.BasePriority != oldInfo.BasePriority)
                RaisePropertyChanged(nameof(BasePriority));
            if(info.VirtualSize != oldInfo.VirtualSize)
                RaisePropertyChanged(nameof(VirtualSize));
            if (info.PeakVirtualSize != oldInfo.PeakVirtualSize)
                RaisePropertyChanged(nameof(PeakVirtualSize));
            if (info.WriteOperationCount != oldInfo.WriteOperationCount)
                RaisePropertyChanged(nameof(WriteOperationsCount));
            if (info.ReadOperationCount != oldInfo.ReadOperationCount)
                RaisePropertyChanged(nameof(ReadOperationsCount));

            CalculateCPU();
            RaisePropertyChanged(nameof(CPU));

            CpuTimeBackground = SignToBrush(TotalTime.Ticks - (oldInfo.KernelTime + oldInfo.UserTime).Ticks);
            KernelTimeBackground = SignToBrush(KernelTime.Ticks - oldInfo.KernelTime.Ticks);
            UserTimeBackground = SignToBrush(UserTime.Ticks - oldInfo.UserTime.Ticks);

            PrivateWorkingSetSizeBackground = SignToBrush(PrivateWorkingSetSize - (oldInfo.PrivateWorkingSetSize >> 10));
            WorkingSetSizeBackground = SignToBrush(WorkingSetSize - (oldInfo.WorkingSetSize >> 10));

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

        public void Dispose() {
            _nativeProcess?.Dispose();
        }

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

        Brush _cpuTimeBackground, _kernelTimeBackground, _userTimeBackground, _workingSetSizeBackground, _privateWorkingSetSizeBackground;
        bool _isDead, _isCreated;
    }
}
