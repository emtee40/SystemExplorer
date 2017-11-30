using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using SystemExplorer.Core;
using SystemExplorer.ViewModels;
using Zodiacon.WPF;

namespace SystemExplorer {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
        public static CompositionContainer Container { get; private set; }
        public static AggregateCatalog Catalog { get; private set; }

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

            Catalog = new AggregateCatalog(
                new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Modules"),
                new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            Container = new CompositionContainer(Catalog);
			var ui = new UIServicesDefaults();
            Container.ComposeExportedValue<IUIServices>(ui);
            var vm = Container.GetExportedValue<MainViewModel>();
			var win = new MainWindow { DataContext = vm };
			win.Show();
			ui.MessageBoxService.SetOwner(win);
		}
	}
}
