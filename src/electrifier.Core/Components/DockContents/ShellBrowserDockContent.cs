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
    /// ShellBrowserDockContent is electrifier's wrapper class for ExplorerBrowser Control.
    /// 
    /// A reference implementation of a wrapper for ExplorerBrowser can be found
    /// <a href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/Samples/ExplorerBrowser/CS/WinForms/ExplorerBrowserTestForm.cs">here</a>.
    /// </summary>

    public class ShellBrowserDockContent : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Fields ========================================================================================================

        protected ExplorerBrowser explorerBrowser;
        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        private ExplorerBrowserViewMode? initialViewMode = null;

        protected System.Windows.Forms.Timer UIDecouplingTimer = new System.Windows.Forms.Timer();
        protected System.Threading.AutoResetEvent explorerBrowser_itemsChangedEvent = new System.Threading.AutoResetEvent(false);
        protected System.Threading.AutoResetEvent explorerBrowser_selectionChangedEvent = new System.Threading.AutoResetEvent(false);

        private static readonly int UIDecouplingInterval = 100;                 // Wait 100ms for UI Thread to react on Item- and Selection-Changes


        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        private ShellObject initialNaviagtionTarget = (ShellObject)Microsoft.WindowsAPICodePack.Shell.KnownFolders.Desktop;

        public ShellObject InitialNaviagtionTarget { get => this.initialNaviagtionTarget; set => this.initialNaviagtionTarget = value; }

        public ExplorerBrowserViewMode ViewMode { get => this.explorerBrowser.ContentOptions.ViewMode; set => this.explorerBrowser.ContentOptions.ViewMode = value; }

        public int ItemsCount { get => this.explorerBrowser.Items.Count; }
        public int SelectedItemsCount { get => this.explorerBrowser.SelectedItems.Count; }

        public ExplorerBrowserNavigationLog NavigationLog { get => this.explorerBrowser.NavigationLog; }

        #endregion Properties =================================================================================================

        #region Published Events ==============================================================================================

        public delegate void ItemsChangedHandler(ShellBrowserDockContent sender, EventArgs eventArgs);               // TODO: EventArgs?!?
        public event ItemsChangedHandler ItemsChanged;

        public delegate void SelectionChangedHandler(ShellBrowserDockContent sender, EventArgs eventArgs);           // TODO: EventArgs?!?
        public event SelectionChangedHandler SelectionChanged;

        public event System.EventHandler<Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs> NavigationLogChanged {
            add { this.explorerBrowser.NavigationLog.NavigationLogChanged += value; }
            remove { this.explorerBrowser.NavigationLog.NavigationLogChanged -= value; }
        }

        #endregion Published Events ===========================================================================================

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

                // Connect ExplorerBrowser Events
                this.explorerBrowser.ItemsChanged += delegate (object o, EventArgs e) { this.explorerBrowser_itemsChangedEvent.Set(); };
                this.explorerBrowser.SelectionChanged += delegate (object o, EventArgs e) { this.explorerBrowser_selectionChangedEvent.Set(); };
                this.explorerBrowser.NavigationPending += this.ExplorerBrowser_NavigationPending;
                this.explorerBrowser.NavigationComplete += this.ExplorerBrowser_NavigationComplete;
                this.explorerBrowser.NavigationFailed += this.ExplorerBrowser_NavigationFailed;
                this.explorerBrowser.ViewEnumerationComplete += this.ExplorerBrowser_ViewEnumerationComplete;

                this.Controls.Add(this.explorerBrowser);

                // Initialize UIDecouplingTimer
                this.UIDecouplingTimer.Tick += new EventHandler(this.UIDecouplingTimer_Tick);
                this.UIDecouplingTimer.Interval = ShellBrowserDockContent.UIDecouplingInterval;
                this.UIDecouplingTimer.Start();



                // Evaluate persistString
                this.EvaluatePersistString(persistString);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.UIDecouplingTimer.Dispose();

            base.Dispose(disposing);
        }

        public void NavigateBackward()
        {
            this.explorerBrowser.NavigateLogLocation(NavigationLogDirection.Backward);
        }

        public void NavigateForward()
        {
            this.explorerBrowser.NavigateLogLocation(NavigationLogDirection.Forward);
        }

        public void NavigateLogLocation(int navigationLogIndex)
        {
            this.explorerBrowser.NavigateLogLocation(navigationLogIndex);
        }

        public void NavigateRefresh()
        {
            
        }

        #region DockContent Persistence Overrides =============================================================================

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

        #endregion DockContent Persistence Overrides ==========================================================================

        #region DockContent Event Handler =====================================================================================

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

        #endregion DockContent Event Handler ==================================================================================

        protected void UIDecouplingTimer_Tick(object sender, EventArgs e)
        {
            if (this.explorerBrowser_itemsChangedEvent.WaitOne(0)) {
                AppContext.TraceDebug("Firing of ItemsChanged event.");

                this.ItemsChanged?.Invoke(this, EventArgs.Empty);
            }

            if (this.explorerBrowser_selectionChangedEvent.WaitOne(0)) {
                AppContext.TraceDebug("Firing of SelectionChanged event.");

                this.SelectionChanged?.Invoke(this, EventArgs.Empty);
            }

        }

        #region ExplorerBrowser Internal Events Handler ========================================================================

        protected void ExplorerBrowser_NavigationPending(object sender, NavigationPendingEventArgs e)
        {
        }


        protected void ExplorerBrowser_NavigationComplete(object sender, Microsoft.WindowsAPICodePack.Controls.NavigationCompleteEventArgs args)
        {
            AppContext.TraceDebug("Firing of ExplorerBrowser_NavigationComplete event.");
            //this.Icon = args.NewLocation.Thumbnail.Icon;        // TODO: Icon-Property seems not to be thread-safe

            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                this.Text = args.NewLocation.Name;

                //this.Icon = args.NewLocation.Thumbnail.SmallIcon;

            }));
        }

        protected void ExplorerBrowser_NavigationFailed(object sender, NavigationFailedEventArgs args)
        {
            AppContext.TraceError("Firing of ExplorerBrowser_NavigationFailed event: " + args.FailedLocation.ParsingName + args.ToString());
        }

        protected void ExplorerBrowser_ViewEnumerationComplete(object sender, EventArgs e)
        {
            AppContext.TraceDebug("Firing of ExplorerBrowser_ViewEnumerationComplete event.");

            this.explorerBrowser_itemsChangedEvent.Set();
            this.explorerBrowser_selectionChangedEvent.Set();
        }

        #endregion ExplorerBrowser Internal Events Handler =====================================================================

    }
}
