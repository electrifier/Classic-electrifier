/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// ShellBrowserDockContent is our wrapper for ExplorerBrowser Control.
    /// 
    /// A reference implementation of a wrapper for ExplorerBrowser can be found
    /// <see href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/Samples/ExplorerBrowser/CS/WinForms/ExplorerBrowserTestForm.cs">
    /// here</see>.
    /// </summary>

    public class ShellBrowserDockContent : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Fields ========================================================================================================

        protected ExplorerBrowser explorerBrowser;
        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        private ExplorerBrowserViewMode? initialViewMode = null;

        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        private ShellObject initialNaviagtionTarget = (ShellObject)Microsoft.WindowsAPICodePack.Shell.KnownFolders.Desktop;

        public ShellObject InitialNaviagtionTarget { get => this.initialNaviagtionTarget; set => this.initialNaviagtionTarget = value; }


        public ExplorerBrowserViewMode ViewMode { get => this.explorerBrowser.ContentOptions.ViewMode; set => this.explorerBrowser.ContentOptions.ViewMode = value; }

        #endregion Properties =================================================================================================

            //
            // TODO: Points of interest:
            //     NavigationComplete
            //     NavigationFailed
            //     NavigationLog
            //     ItemsChanged
            //

            // TODO: Work on Get/Lost Focus in general!

        public ShellBrowserDockContent(string persistString = null) : base()
        {
            this.SuspendLayout();

            try
            {
                // Initialize ExplorerBrowser
                this.explorerBrowser = new ExplorerBrowser()
                {
                    Dock = DockStyle.Fill,
                };

                this.Controls.Add(this.explorerBrowser);

                // Connect ExplorerBrowser Events
                this.explorerBrowser.NavigationComplete += this.ExplorerBrowser_NavigationComplete;

                // Evaluate persistString
                this.EvaluatePersistString(persistString);
            }
            finally
            {
                this.ResumeLayout();
            }
        }




        /// <summary>
        /// Override of WeifenLuo.WinFormsUI.Docking.DockContent.GetPersistString()
        /// </summary>
        /// <returns>The string describing persistence information. E.g. persistString = "ShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";</returns>
        protected override string GetPersistString()
        {
            var sb = new StringBuilder();
            string paramFmt = " {0}{1}";

            // Append class name as identifier
            sb.Append(nameof(ShellBrowserDockContent));

            // Append URI
            sb.AppendFormat(paramFmt, ShellBrowserDockContent.persistParamURI,
                WindowsShell.Tools.UrlCreateFromPath(this.explorerBrowser.NavigationLog.CurrentLocation.ParsingName));

            // Append ViewMode
            // TODO: For any reason, this doesn't work... :(
            sb.AppendFormat(paramFmt, ShellBrowserDockContent.persistParamViewMode, this.ViewMode);

            return sb.ToString();
        }

        /// <summary>
        /// Example: persistString = "ShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";
        /// </summary>
        /// <param name="persistString"></param>
        protected void EvaluatePersistString(string persistString)
        {
            try
            {
                if ((null != persistString) && (persistString.Trim().Length > ShellBrowserDockContent.persistParamURI.Length))
                {
                    var args = WindowsShell.Tools.SplitArgumentString(persistString);
                    string strInitialNaviagtionTarget = null;
                    string strViewMode = null;

                    foreach (string arg in args)
                    {
                        if (arg.StartsWith(ShellBrowserDockContent.persistParamURI))
                        {
                            strInitialNaviagtionTarget = WindowsShell.Tools.PathCreateFromUrl(arg.Substring(ShellBrowserDockContent.persistParamURI.Length));
                        }

                        if (arg.StartsWith(ShellBrowserDockContent.persistParamViewMode))
                        {
                            strViewMode = arg.Substring(ShellBrowserDockContent.persistParamViewMode.Length);
                        }


                    }

                    // Finally, when all parameters have been parsed successfully, apply them
                    if (null != strInitialNaviagtionTarget)
                        this.InitialNaviagtionTarget = ShellObject.FromParsingName(strInitialNaviagtionTarget);

                    if (null != strViewMode)
                    {
                        ExplorerBrowserViewMode ebvm = ExplorerBrowserViewMode.Auto;

                        ebvm = (ExplorerBrowserViewMode) Enum.Parse(typeof(ExplorerBrowserViewMode), strViewMode);
                        this.initialViewMode = ebvm;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ShellBrowserDockContent.EvaluatePersistString: Error evaluating parameters"
                    + "\n\nParameters: '" + persistString + "'"
                    + "\n\nError description: '" + e.Message + "'"
                    + "\n\nResetting to default values.");
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // TODO: When multiple Tabs are opened initially, sometimes this won't be called for some folders... Most likely if invisible!
            this.explorerBrowser.Navigate(this.InitialNaviagtionTarget);

            if (null != this.initialViewMode)
            {
                this.ViewMode = (ExplorerBrowserViewMode)this.initialViewMode;
            }
        }

        protected void ExplorerBrowser_NavigationComplete(object sender, Microsoft.WindowsAPICodePack.Controls.NavigationCompleteEventArgs args)
        {
            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                this.Text = args.NewLocation.Name;
            }));
        }
    }
}
