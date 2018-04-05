using Prism;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SystemExplorer.Core {
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TabItemAttribute : Attribute {
		public string Header { get; set; }
		public string Icon { get; set; }
	}

	public abstract class TabViewModelBase : BindableBase, IActiveAware {
		protected TabViewModelBase() {
			Init();
		}

		private void Init() {
			var attr = GetType().GetCustomAttribute<TabItemAttribute>();
			if (attr != null) {
				Header = attr.Header;
				Icon = attr.Icon;
			}
		}

		private string _header;

		public string Header {
			get { return _header; }
			set { SetProperty(ref _header, value); }
		}

		private string _icon;

		public string Icon {
			get { return _icon; }
			set { SetProperty(ref _icon, value); }
		}

		bool _isActive;
		public bool IsActive {
			get { return _isActive; }
			set {
				if (SetProperty(ref _isActive, value)) {
					OnActive(value);
					OnActiveChanged();
				}
			}
		}

		protected virtual void OnActive(bool active) { }

		protected virtual void OnActiveChanged() {
			IsActiveChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler IsActiveChanged;
	}
}
