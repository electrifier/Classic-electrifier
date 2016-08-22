using System;

namespace electrifier.Core.Shell32.Services {
	/// <summary>
	/// Zusammenfassung für IFileInfoThread.
	/// </summary>
	public interface IFileInfoThread {
		void Prioritize(IShellObject sender);
		void Remove(IShellObject sender);
	}
}
