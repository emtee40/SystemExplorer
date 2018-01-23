﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace SystemExplorer.Modules.Processes.ViewModels {
    class ProcessViewModel : BindableBase, IDisposable {
        static Brush _activityBrush = new SolidColorBrush(Colors.Red) { Opacity = .5 };
        static Brush _decreaseActivityBrush = new SolidColorBrush(Colors.Green) { Opacity = .5 };
        readonly NativeProcess _nativeProcess;

        public ProcessExtendedInformation Info { get; private set; }

        public ProcessViewModel(ProcessExtendedInformation info) {
            Info = info;
            _nativeProcess = NativeProcess.TryOpen(ProcessAccessMask.QueryLimitedInformation, Info.ProcessId);
        }

        public TimeSpan TotalTime => Info.UserTime + Info.KernelTime;
        public TimeSpan UserTime => Info.UserTime;
        public TimeSpan KernelTime => Info.KernelTime;
        public int ThreadCount => Info.ThreadCount;
        public int HandleCount => Info.HandleCount;
        public long PrivateWorkingSetSize => Info.PrivateWorkingSetSize >> 10;
        public long WorkingSetSize => Info.WorkingSetSize >> 10;
        public bool? IsImmersive => _nativeProcess?.IsImmersive;
        public ProcessPriorityClass? PriorityClass => _nativeProcess?.PriorityClass;

        public Brush CpuTimeBackground { get => _cpuTimeBackground; set => SetProperty(ref _cpuTimeBackground, value); }
        public Brush KernelTimeBackground { get => _kernelTimeBackground; set => SetProperty(ref _kernelTimeBackground, value); }
        public Brush UserTimeBackground { get => _userTimeBackground; set => SetProperty(ref _userTimeBackground, value); }
        public Brush WorkingSetSizeBackground { get => _workingSetSizeBackground; set => SetProperty(ref _workingSetSizeBackground, value); }
        public Brush PrivateWorkingSetSizeBackground { get => _privateWorkingSetSizeBackground; set => SetProperty(ref _privateWorkingSetSizeBackground, value); }

        public ProcessViewModel Self => this;

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