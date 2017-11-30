using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Modules.Computer {
    [Item(Text = "Env. Variables")]
    class EnvironmentVariablesViewModel : TabItemViewModelBase {
        public EnvironmentVariablesViewModel() {
            Icon = Helpers.ToPackUri(Assembly.GetExecutingAssembly().GetName().Name, "/icons/variables.ico");
        }
    }
}
