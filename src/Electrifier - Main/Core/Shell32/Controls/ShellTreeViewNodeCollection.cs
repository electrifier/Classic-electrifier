//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ShellTreeViewNodeCollection.cs,v 1.5 2004/08/25 18:49:07 jung2t Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Core.Controls;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellTreeViewNodeCollection.
	/// </summary>
	public sealed class ShellTreeViewNodeCollection : ExtTreeViewNodeCollection {

		public ShellTreeViewNodeCollection(ExtTreeViewNodeCollection collection)
			: base(collection) { }

		public void AddCollection(BasicShellObjectCollection collection) {
			ShellTreeViewNode[] nodes = new ShellTreeViewNode[collection.Count];
			int                 index = 0;

			foreach(BasicShellObject shellObject in collection) {
				nodes[index++] = new ShellTreeViewNode(shellObject);
			}

			AddRange(nodes);
		}
	}
}
