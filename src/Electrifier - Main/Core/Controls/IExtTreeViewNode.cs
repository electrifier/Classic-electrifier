//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IExtTreeViewNode.cs,v 1.1 2004/08/25 17:59:07 jung2t Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung f�r IExtTreeViewNode.
	/// </summary>
	public interface IExtTreeViewNode {
		void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender);
	}
}
