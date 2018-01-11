using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SystemExplorer.Core;
using SystemExplorer.Modules.Processes.ViewModels;

namespace SystemExplorer.Modules.Processes {
    [Module("Processes", Author = "Pavel Yosifovich")]
    [Export(typeof(IModule))]
    public sealed class Module : IModule {
        [Import]
        ISystemExplorer Explorer;

        [Import]
        CompositionContainer Container;

        public bool Init() {
            var resources = new ResourceDictionary { Source = Helpers.ToPackUri(Assembly.GetExecutingAssembly(), "/resources.xaml") };
            Explorer.AddResourceDictionary(resources, this);
            Explorer.AddTreeViewItem(Container.GetExportedValue<ProcessesTreeViewItem>(), this);
            Explorer.AddTreeViewItem(Container.GetExportedValue<ThreadsTreeViewItem>(), this);
            return true;
        }
    }
}
