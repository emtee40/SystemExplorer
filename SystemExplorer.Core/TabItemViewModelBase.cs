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
	public sealed class ItemAttribute : Attribute {
		public string Text { get; set; }
		public string Icon { get; set; }
	}

	public abstract class TabItemViewModelBase : BindableBase, IActiveAware {
		protected TabItemViewModelBase() {
			Init();
		}

		private void Init() {
			var attr = GetType().GetCustomAttribute<ItemAttribute>();
			if (attr != null) {
				Text = attr.Text;
				Icon = attr.Icon;
			}
		}

		private string _text;

		public string Text {
			get => _text; 
			set => SetProperty(ref _text, value); 
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

        protected internal virtual bool CanClose => true;

        protected internal virtual void OnClose() { }
	}
}
