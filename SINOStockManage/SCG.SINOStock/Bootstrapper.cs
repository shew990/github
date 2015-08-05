using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace SCG.SINOStock
{
	public class Bootstrapper : UnityBootstrapper {
		protected override System.Windows.DependencyObject CreateShell() {
            Sell shell = Container.Resolve<Sell>();
			shell.Show();
			return shell;
		}
	}
}
