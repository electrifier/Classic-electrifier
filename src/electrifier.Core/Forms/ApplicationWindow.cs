using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;
using RibbonLib.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// <see cref="ApplicationWindow"/> is the Main Window of electrifier application.
    /// </summary>
    public partial class ApplicationWindow
      : System.Windows.Forms.Form
      , System.Windows.Forms.IWin32Window       // Used for ShellFileOperations
    {
        #region Fields ========================================================================================================

        // TODO: Add Creation timestamp! in debug mode
        //private static readonly string formTitle_Affix = $"electrifier { AppContext.AssemblyVersion.ToString(3) }";
        public RibbonItems RibbonItems { get; }

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public DockPanel DockPanel { get => this.dpnDockPanel; }

        public SessionContext SessionContext { get; }
        public SessionEntity Session => this.SessionContext.Session;

        public override string Text
        {
            get => base.Text;
            set => base.Text = AppContext.BuildDefaultFormText(value);
        }

        public SelectConditionalBox SelectConditionalBox { get; private set; }

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================


        #endregion ============================================================================================================


        public ApplicationWindow(SessionContext sessionContext)
          : base()
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

            LogContext.Trace();

            this.InitializeComponent();
            this.RibbonItems = new RibbonItems(this, this.rbnRibbon);

            // Initialize form's Icon and Title
            this.Text = this.Session.Name;
            this.Icon = this.SessionContext.ApplicationIcon;

            // Initialize DockPanel
            this.dpnDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.dpnDockPanel.ShowDocumentIcon = true;
            this.dpnDockPanel.ActiveContentChanged += this.DpnDockPanel_ActiveContentChanged;
            this.dpnDockPanel.ActiveDocumentChanged += this.DpnDockPanel_ActiveDocumentChanged;
        }

        /// <summary>
        /// Currently active DockContent has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpnDockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender.Equals(this.dpnDockPanel));

            IDockContent activeContent = this.dpnDockPanel.ActiveContent;

            // Process Ribbon-part of DockContent-Activation
//            this.RibbonItems.ActiveDockContent = activeContent;

            // Process Interface INavigationHost-part of DockContent-Activation
            if (null == activeContent)
            {
                LogContext.Debug("DpnDockPanel_ActiveContentChanged: activeContent is NULL");
            }
            else
            {
                var activatedContentType = activeContent?.GetType().Name;

                //DpnDockPanel_ActiveContentChanged: Sender = WeifenLuo.WinFormsUI.Docking.DockPanel, BorderStyle: System.Windows.Forms.BorderStyle.None, ActivatedContent = electrifier.Core.Components.DockContents.SelectConditionalBox, Text: Select Conditional..., ActivatedContentType = electrifier.Core.Components.DockContents.SelectConditionalBox @ 'W:\[Git.Workspace]\[electrifier.Workspace]\electrifier\src\electrifier.Core\Forms\ApplicationWindow.cs' in DpnDockPanel_ActiveContentChanged

                LogContext.Debug($"DpnDockPanel_ActiveContentChanged({ activeContent?.GetType().Name }): { activeContent }");
                //"DpnDockPanel_ActiveContentChanged: Sender=" + sender.ToString()
                //+ ", ActivatedContent=" + activeContent
                //+ ", ActivatedContentType=" + activatedContentType);

				// TODO: This is for test purposes only!
                if (activeContent is ExplorerBrowserDocument explorerBrowser)
                {
                    try
                    {
                        this.tspTopToolStripPanel.SuspendLayout();
                        this.tspTopToolStripPanel.Controls.Clear();
                        this.tspTopToolStripPanel.Join(explorerBrowser.ToolStrip);
                    }
                    finally
                    {
                        this.tspTopToolStripPanel.ResumeLayout();
                        this.tspTopToolStripPanel.PerformLayout();
                    }
                }
            }
        }

        public IDockContent CurrentDocument { get; private set; }

        private void DpnDockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender.Equals(this.dpnDockPanel));

            var oldDocument = this.CurrentDocument;
            var newDocument = this.dpnDockPanel.ActiveDocument;

//            Debug.Assert(!(newDocument is null));  => If all Panels all closed, this will be null!

            this.CurrentDocument = newDocument;

            if (null != oldDocument)
            {
                Debug.Assert(!oldDocument.Equals(newDocument));

                if (oldDocument is IRibbonConsumer oldRibbonConsumer)
                {
                    oldRibbonConsumer.DeactivateRibbonState();
                }
            }

            if (newDocument is IRibbonConsumer newRibbonConsumer)
            {
                newRibbonConsumer.ActivateRibbonState();
            }

            LogContext.Trace($"Achtung Baby! *ActiveDocumentChanged*: { newDocument?.GetType() }, this is *not* Active Content Changed: { this.dpnDockPanel.ActiveContent?.GetType() }");
        }

        private void FormStatePersistor_LoadFormState(object sender, FormStatePersistorEventArgs args)
        {
            TypeConverter pointConvert = TypeDescriptor.GetConverter(typeof(Point));
            TypeConverter sizeConvert = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter stateConvert = TypeDescriptor.GetConverter(typeof(FormWindowState));

            // NULL-Check!

            // TODO: Put Converters into EntityLighter!
            args.Location = (Point)pointConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ApplicationWindow.Location", "0,0"));
            args.Size = (Size)sizeConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ApplicationWindow.Size", "800, 600"));
            args.WindowState = (FormWindowState)stateConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ApplicationWindow.WindowState", "Normal"));

            string dockPanelConfig = this.SessionContext.Properties.SyncProperty("ApplicationWindow.DockPanelState", String.Empty);

            if (dockPanelConfig.Length > 0)
            {
                using (var dockPanelStateStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(dockPanelStateStream))
                    {
                        streamWriter.Write(dockPanelConfig);
                        streamWriter.Flush();
                        dockPanelStateStream.Position = 0;

                        this.dpnDockPanel.LoadFromXml(dockPanelStateStream,
                            new DeserializeDockContent(delegate (string persistString)
                            {
                                return DockContentFactory.Deserialize(this, persistString, this.dpnDockPanel); // TODO: Throw Exception cause of unkown type in XML ? !?
                            }));
                    }
                }
            }

            args.Cancel = false;
        }

        private void FormStatePersistor_SaveFormState(object sender, FormStatePersistorEventArgs args)
        {
            TypeConverter pointConvert = TypeDescriptor.GetConverter(typeof(Point));
            TypeConverter sizeConvert = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter stateConvert = TypeDescriptor.GetConverter(typeof(FormWindowState));
            string dockPanelConfig;

            this.SessionContext.Properties.SafeSetProperty("ApplicationWindow.Location", pointConvert.ConvertTo(args.Location, typeof(string)) as string);
            this.SessionContext.Properties.SafeSetProperty("ApplicationWindow.Size", sizeConvert.ConvertTo(args.Size, typeof(string)) as string);
            this.SessionContext.Properties.SafeSetProperty("ApplicationWindow.WindowState", stateConvert.ConvertTo(args.WindowState, typeof(string)) as string);

            using (var dockPanelStateStream = new MemoryStream())
            {
                this.dpnDockPanel.SaveAsXml(dockPanelStateStream, System.Text.Encoding.UTF8);

                dockPanelStateStream.Position = 0;
                using (var streamReader = new StreamReader(dockPanelStateStream, true))
                {
                    dockPanelConfig = streamReader.ReadToEnd();
                }
            }
                
            this.SessionContext.Properties.SafeSetProperty("ApplicationWindow.DockPanelState", dockPanelConfig);
        }
    }
}
