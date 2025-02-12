﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier.Core.Components.Controls.Extensions
{
    /// <summary>
    /// Static class that provides the most common Extension Methods used in conjunction with Interface
    /// <see cref="IThemedControl"/>.
    /// </summary>
    public static class ThemedControlExtensions
    {
        public static string ThemeFileExtension { get => ".png"; }

        public static string ThemeResourceNamespace(this IThemedControl control)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            return @"electrifier.Core.Resources.Themes." + control.GetType().Name + ".";
        }

        /// <summary>
        /// Search all embedded resources in this assembly for images that can be used as themes for an IThemedControl.
        /// 
        /// Their namespace has to match the following pattern:
        ///   "[IThemedControl.ThemeResourceNamespace].THEMENAME.[ThemedControlExtensions.ThemeFileExtension]"
        /// </summary>
        /// <param name="control">The control that implements IThemedControl interface.</param>
        /// <returns>An IEnumerable collection of the available themes.</returns>
        public static IEnumerable<string> EnumerateAvailableThemes(this IThemedControl control)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(
                Name =>
                Name.StartsWith(control.ThemeResourceNamespace, StringComparison.InvariantCultureIgnoreCase) &&
                Name.EndsWith(ThemeFileExtension, StringComparison.InvariantCultureIgnoreCase));

            foreach (var currentName in resourceNames)
            {
                yield return currentName.Substring(control.ThemeResourceNamespace.Length,
                    (currentName.Length - control.ThemeResourceNamespace.Length - ThemeFileExtension.Length));
            }
        }

        /// <summary>
        /// Load image strip from embedded resource and return as new <see cref="ImageList"/>.
        /// </summary>
        /// <param name="control">The control that implements IThemedControl interface.</param>
        /// <param name="themeName">The name of embedded resource that contains the theme.</param>
        /// <returns>The <see cref="ImageList"/> containing the images of the loaded image strip.</returns>
        public static ImageList LoadThemeImageListFromResource(this IThemedControl control, string themeName)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));
            if (string.IsNullOrWhiteSpace(themeName))
                throw new ArgumentOutOfRangeException(nameof(themeName), "No theme name provided");

            var resourceName = control.ThemeResourceNamespace + themeName + ThemeFileExtension;

            try
            {
                var imageList = new ImageList
                {
                    /// Fix bug with alpha channels by enabling transparency before adding any images to the list.
                    /// <seealso href="https://www.codeproject.com/articles/9142/adding-and-using-32-bit-alphablended-images-and-ic"/>
                    ColorDepth = ColorDepth.Depth32Bit
                };

                using (var bmpStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var bitmap = new Bitmap(bmpStream);

                    imageList.ImageSize = new Size(bitmap.Height, bitmap.Height);

                    if (-1 == imageList.Images.AddStrip(bitmap))
                        throw new Exception(@"ImageList.Images.AddStrip() failed.");
                }

                return imageList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to load ImageList for theme from embedded resource '{ resourceName }'.", ex);
            }
        }
    }
}
