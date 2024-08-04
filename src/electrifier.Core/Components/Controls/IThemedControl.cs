using System.Collections.Generic;

namespace electrifier.Core.Components.Controls
{
    /// <summary>
    /// This interface aids when adding theme functionality to existing controls.
    /// 
    /// The most common default implementations are provided via Extension Methods in
    /// <see cref="Extensions.ThemedControlExtensions"/>.
    /// </summary>
    public interface IThemedControl
    {
        string DefaultTheme { get; }
        string ThemeResourceNamespace { get; }

        IEnumerable<string> GetAvailableThemes();

        string CurrentTheme { get; set; }

        // TODO: Selecting theme via contextmenu
    }
}
