using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Core {
	public interface ITabManager {
		void AddTab(TabItemViewModelBase item, bool select = false);
		void RemoveTab(TabItemViewModelBase item);
        bool IsBusy { get; set; }
	}
}
