using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Models {
    sealed class ModuleInfo {
        public IModule Module { get; }
        public ModuleAttribute Attributes { get; }

        public ModuleInfo(IModule module, ModuleAttribute attributes) {
            Module = module;
            Attributes = attributes;
        }

        public string Name => Attributes.Name;
    }

    [Export]
    sealed class ModuleManager : IPartImportsSatisfiedNotification {
        ObservableCollection<ModuleInfo> _modules = new ObservableCollection<ModuleInfo>();

        [ImportMany]
        List<IModule> _moduleInstances;

        public IEnumerable<ModuleInfo> Modules => _modules;

        public IEnumerable<ModuleInfo> AddModulesFromFile(string path) {
            var modules = new List<ModuleInfo>(4);
            var asm = Assembly.LoadFile(path);
            var types = from type in asm.GetExportedTypes()
                        let attr = type.GetCustomAttribute<ModuleAttribute>()
                        where attr != null && !string.IsNullOrWhiteSpace(attr.Name)
                        select new { Type = type, Attributes = attr };

            foreach (var m in types) {
                var instance = (IModule)Activator.CreateInstance(m.Type);
                var module = new ModuleInfo(instance, m.Attributes);
                if (!instance.Init())
                    continue;
                _modules.Add(module);
                modules.Add(module);
            }

            return modules;
        }

        public void OnImportsSatisfied() {
            foreach (var module in _moduleInstances) {
                AddModule(module);
            }
        }

        private bool AddModule(IModule module) {
            var attributes = module.GetType().GetCustomAttribute<ModuleAttribute>();
            var mi = new ModuleInfo(module, attributes);
            if (!module.Init())
                return false;

            _modules.Add(mi);
            return true;
        }
    }
}
