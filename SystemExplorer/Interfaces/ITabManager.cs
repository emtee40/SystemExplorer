using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer {
	interface ITabManager {
		void AddTab(TabItemViewModelBase item, bool select = false);
		void RemoveTab(TabItemViewModelBase item);
	}
}
