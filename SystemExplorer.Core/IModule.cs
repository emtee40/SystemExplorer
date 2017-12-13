using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleAttribute : Attribute {
        public string Name { get; }
        public ModuleAttribute(string moduleName) {
            Name = moduleName;
        }

        public string Description { get; set; }
        public string Author { get; set; }
        public short MajorVersion { get; set; } = 1;
        public short MinorVersion { get; set; }
        public string Icon { get; set; }
    }

    public interface IModule {
        bool Init();
    }

    public interface IServiceProvider<T> where T : class {
        T GetService();
    }
}
