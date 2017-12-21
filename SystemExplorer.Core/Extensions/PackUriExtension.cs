using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SystemExplorer.Core.Extensions {
    [MarkupExtensionReturnType(typeof(string))]
    public sealed class PackUriExtension : MarkupExtension {
        public string RelativeUri { get; set; }

        public PackUriExtension(string relativeUri) {
            RelativeUri = relativeUri;
        }

        public override object ProvideValue(IServiceProvider sp) {
            if (sp.GetService(typeof(IUriContext)) is IUriContext ctx) {
                var component = ctx.BaseUri.AbsoluteUri.LastIndexOf(";component/");
                if (component < 0)
                    throw new ArgumentException(ctx.BaseUri.ToString());
                return ctx.BaseUri.AbsoluteUri.Substring(0, component) + ";component" + RelativeUri;
            }
            return RelativeUri;
        }
    }
}
