using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.ManagedWindows.Core;
using Zodiacon.ManagedWindows.Processes;

namespace SystemExplorer.Modules.Processes.ViewModels {
    class ThreadViewModel : BindableBase {
        readonly ThreadInfo _info;
        readonly ProcessesViewModel _processes;

        public ThreadViewModel(ProcessesViewModel processes, ThreadInfo info) {
            _info = info;
            _processes = processes;
            using (var thread = NativeThread.TryOpen(ThreadAccessMask.QueryLimitedInformation, Id)) {
                if (thread != null) {
                    KernelTime = thread.KernelTime;
                    UserTime = thread.UserTime;
                    TotalTime = thread.TotalTime;
                    CreateTime = thread.CreateTime?.ToLocalTime();
                }
            }
        }

        public void Refresh() {
            using (var thread = NativeThread.TryOpen(ThreadAccessMask.QueryLimitedInformation, Id)) {
                if (thread != null) {
                    KernelTime = thread.KernelTime;
                    UserTime = thread.UserTime;
                    TotalTime = thread.TotalTime;
                    RaisePropertyChanged(nameof(KernelTime));
                    RaisePropertyChanged(nameof(UserTime));
                    RaisePropertyChanged(nameof(TotalTime));
                }
            }
        }

        public DateTime? CreateTime { get; private set; }
        public TimeSpan? KernelTime { get; private set; }
        public TimeSpan? UserTime { get; private set; }
        public TimeSpan? TotalTime { get; private set; }

        public int Id => _info.Id;
        public int ProcessId => _info.ProcessId;
        //public string ProcessName => _processes.GetProcessName(_info.ProcessId);
        public int BasePriority => _info.BasePriority;

        public int State => 0;

    }
}
