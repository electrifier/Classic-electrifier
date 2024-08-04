using electrifier.Core.Components.DockContents;
using electrifier.Core.Forms;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components
{
    internal static class DockContentFactory
    {
        private static SelectConditionalBox selectConditionalBox;
        private static DockPanel SelectConditionalDockPanel;
        private static DockState SelectConditionalDockState;


        /// <summary>
        /// Get the instance of the currently opened <see cref="DockContents.SelectConditionalBox"/>, if there is one.
        /// </summary>
        public static SelectConditionalBox SelectConditionalBox
        {
            get => selectConditionalBox; // ?? (selectConditionalBox = new SelectConditionalBox());
            private set => selectConditionalBox = value;
        }


        //public static SelectConditionalBox CreateSelectConditionalBox() => SelectConditionalBox ?? (SelectConditionalBox = new SelectConditionalBox());


        /// <summary>
        /// Deserialize given persistString as used in XML-files to create the corresponding DockContent instance.
        /// </summary>
        /// <param name="applicationWindow"><see cref="ApplicationWindow"/> instance the new DockContent will be added to.</param>
        /// <param name="persistString">The string stored in XML-Files with DockContent type and parameters,
        ///     e.g "ElShellBrowserDockContent URI=file:///S:/%5BGit.Workspace%5D/electrifier"</param>
        /// <returns>The valid IDockContent instance, or NULL if <see cref="persistString"/> is invalid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Unknown DockContent type.</exception>
        public static IDockContent Deserialize(ApplicationWindow applicationWindow, string persistString, DockPanel dockPanel)
        {
            IDockContent dockContent = default;
            var typeNameSeperatorPos = persistString.IndexOf(" ");
            string dockContentTypeName, dockContentArguments = default;

            if (typeNameSeperatorPos < 0)
                dockContentTypeName = persistString;
            else
            {
                dockContentTypeName = persistString.Substring(0, typeNameSeperatorPos);
                dockContentArguments = persistString.Substring(typeNameSeperatorPos);
            }

            /* TODO: Remove namespace, but why has SelectConditionalBox one, whereas ExplorerBrowserDocument has not? */
            if ((typeNameSeperatorPos = dockContentTypeName.LastIndexOf(".")) > 0)
                dockContentTypeName = persistString.Substring(typeNameSeperatorPos + 1);

            switch (dockContentTypeName)
            {
                case nameof(ExplorerBrowserDocument):
                    dockContent = CreateShellBrowser(applicationWindow, dockContentArguments);
                    break;
                case nameof(SelectConditionalBox):
                    dockContent = CreateSelectConditionalBox(dockPanel);       // TODO: Serialize DockState
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown DockContent of type { dockContentTypeName }");
            }

            return dockContent;
        }

        // TODO: Use recursive pattern in C#8
        [Obsolete("Not implemented yet", true)]
        public static T Create<T>(ApplicationWindow applicationWindow, bool showAfterCreation = true) where T : DockPanel
        {
            throw new NotImplementedException(nameof(Create));
        }

        public static ExplorerBrowserDocument CreateShellBrowser(ApplicationWindow applicationWindow, string persistString = null)
        {
            ExplorerBrowserDocument shellBrowser = new ExplorerBrowserDocument(applicationWindow, persistString);

            shellBrowser.Show();

            return shellBrowser;
        }

        public static void ToggleSelectConditionalBox(DockPanel defaultDockPanel)
        {
            if (SelectConditionalBox != null)
            {
                DockContentFactory.CloseSelectConditionalBox();
            }
            else
            {
                DockContentFactory.CreateSelectConditionalBox(defaultDockPanel);
            }
        }

        private static SelectConditionalBox CreateSelectConditionalBox(DockPanel defaultDockPanel, DockState defaultDockState = DockState.DockRightAutoHide)
        {
            if (SelectConditionalBox != null)
                throw new InvalidOperationException("An instance of SelectConditionalBox has already been created");

            SelectConditionalBox = new SelectConditionalBox();

            if (SelectConditionalDockPanel != null)
                SelectConditionalBox.Show(SelectConditionalDockPanel, SelectConditionalDockState);
            else
            {
                SelectConditionalBox.Show(defaultDockPanel, defaultDockState);
            }

            return SelectConditionalBox;
        }

        private static void CloseSelectConditionalBox()
        {
            var dockPanel = SelectConditionalBox;

            // TODO: Test pruposes only... Just return
            if (dockPanel == null)
                throw new InvalidOperationException("No instance of SelectConditionalBox has already been created");

            SelectConditionalDockPanel = SelectConditionalBox.DockPanel;
            SelectConditionalDockState = SelectConditionalBox.DockState;

            SelectConditionalBox = null;
            dockPanel.Close();
        }
    }
}
