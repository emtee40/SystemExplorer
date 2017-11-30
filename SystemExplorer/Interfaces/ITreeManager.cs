using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExplorer.Core;

namespace SystemExplorer.Interfaces {
    interface ITreeManager {
        void AddTreeItem(TreeViewItemBase item);
        void RemoveTreeItem(TreeViewItemBase item);
    }
}
