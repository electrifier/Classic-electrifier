//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IFileInfoThread.cs,v 1.1 2004/08/27 19:28:21 taj bender Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Shell32.Services {
	/// <summary>
	/// Zusammenfassung für IFileInfoThread.
	/// </summary>
	public interface IFileInfoThread {
		void Prioritize(IShellObject sender);
		void Remove(IShellObject sender);
	}
}
