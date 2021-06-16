/*
** 
**  electrifier
** 
**  Copyright 2017-19 Thorsten Jung, www.electrifier.org
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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// </summary>
    public partial class SessionSelector
      : Form
    {
        #region = Fields =======================================================================================================

        public enum SessionCreationMode
        {
            StartNew,
            Continue,
        }

        private SessionCreationMode creationMode = SessionCreationMode.StartNew;
//        private int ContinueSessionListView_LastSelectedItem = -1;

        #endregion

        #region = Properties ===================================================================================================

        public SessionContext SessionContext { get; }

        public SessionCreationMode CreationMode
        {
            get => this.creationMode;
            set
            {
                if (this.creationMode != value)
                {
                    this.creationMode = value;

                    if (this.CreationMode == SessionCreationMode.StartNew)
                    {
                        this.CreateSessionRadioButton.Checked = true;

                        //ContinueSessionListView_LastSelectedItem = this.ContinueSessionListView.SelectedIndices;      // TODO: Restore last selected session when changing from StartNew to Continue again!
                        this.ContinueSessionListView.SelectedItems.Clear();
                    }
                    else
                    {
                        if (this.ContinueSessionListView.SelectedItems.Count == 0)
                        {
                            this.ContinueSessionListView.Items[0].Selected = true;
                        }

                        this.ContinueSessionRadioButton.Checked = true;
                    }
                }
            }
        }

        public bool HasSessionsToContinue => this.ContinueSessionListView.Items.Count > 0;

        public override string Text
        {
            get => base.Text;
            set => base.Text = AppContext.BuildDefaultFormText(value);
        }

        public string SessionCreationName => this.CreateSessionNameTextBox.Text;
        public string SessionCreationDescription => this.CreateSessionDescriptionTextBox.Text;

        public long? SessionContinuingId
        {
            get
            {
                ListView.SelectedListViewItemCollection listViewItems = this.ContinueSessionListView.SelectedItems;

                if (1 == listViewItems.Count)
                {
                    var item = listViewItems[0] as SessionSelectorListViewItem;

                    return item?.Session.Id;
                }

                return null;
            }
        }

        #endregion

        #region = Published Events =============================================================================================

        /// <summary>
        /// Fires after this dialog gets confirmed by using the <b>Ok</b> button
        /// and <see cref="SessionCreationMode"/> is <i>StartNew</i>.
        /// </summary>
        [Category("Action")]
        [Description("User has chosen to start a new session.")]
        public event EventHandler<StartNewSessionEventArgs> StartNewSession;

        /// <summary>
        /// Fires after this dialog gets confirmed by using the <b>Ok</b> button
        /// and <see cref="SessionCreationMode"/> is <i>Continue</i>.
        /// </summary>
        [Category("Action")]
        [Description("User has chosen to continue an existing session.")]
        public event EventHandler<ContinueSessionEventArgs> ContinueSession;

        #endregion

        #region = Subclass: SessionSelectorListViewItem ========================================================================

        protected class SessionSelectorListViewItem
          : ListViewItem
        {
            public SessionEntity Session { get; }

            public SessionSelectorListViewItem(SessionEntity session)
            {
                this.Session = session ?? throw new ArgumentNullException(nameof(session));
                this.Text = session.Name;
                this.SubItems.AddRange(new string[] {
                    session.Description,
                    session.DateCreated.ToString(),
                    session.DateModified.ToString(),
                });
            }
        }

        #endregion

        public SessionSelector(SessionContext sessionContext)
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

            LogContext.Trace();

            this.InitializeComponent();
            this.Icon = sessionContext.ApplicationIcon;

            //this.DialogOkButton.Click += this.DialogOkButton_Click;

            // Set initial User Interface values
            this.CreateSessionNameTextBox.Text = "New Session";
            this.CreateSessionDescriptionTextBox.Text = "Enter your Description...";

            foreach (var session in this.SessionContext.PreviousSessions)
                this.ContinueSessionListView.Items.Add(new SessionSelectorListViewItem(session));

            if (!this.HasSessionsToContinue)
            {
                this.ContinueSessionRadioButton.Enabled = false;
                this.ContinueSessionListView.Enabled = false;
            }
        }

        private void DialogOkButton_Click(object sender, EventArgs args)
        {
            LogContext.Trace();
            Debug.Assert(this.CreateSessionRadioButton.Checked != this.ContinueSessionRadioButton.Checked);

            this.Cursor = Cursors.WaitCursor;

            if (SessionCreationMode.StartNew == this.CreationMode)
            {
                this.StartNewSession.Invoke(this, new StartNewSessionEventArgs(
                    this.SessionCreationName,
                    this.SessionCreationDescription,
                    this.DialogRememberChoiceCheckBox.Checked));
            }
            else if (SessionCreationMode.Continue == this.CreationMode)
            {
                ListView.SelectedListViewItemCollection items = this.ContinueSessionListView.SelectedItems;

                if (items.Count > 1)
                    throw new InvalidOperationException("SessionSelector: More than one session selected!");
                else if (items.Count < 1)
                    throw new InvalidOperationException("SessionSelector: No session selected!");

                SessionSelectorListViewItem listViewItem = items[0] as SessionSelectorListViewItem;

                this.ContinueSession.Invoke(this, new ContinueSessionEventArgs(
                    listViewItem.Session.Id,
                    this.DialogRememberChoiceCheckBox.Checked));
            }

            this.Close();
        }

        private void CreateSessionRadioButton_CheckedChanged(object sender, EventArgs args)
        {
            this.CreationMode = this.CreateSessionRadioButton.Checked ?
                SessionCreationMode.StartNew :
                SessionCreationMode.Continue;
        }

        private void CreateSessionNameOrDescriptionTextBox_TextChanged(object sender, EventArgs args)
        {
            this.CreationMode = SessionCreationMode.StartNew;
        }

        private void ContinueSessionRadioButton_CheckedChanged(object sender, EventArgs args)
        {
            this.CreationMode = this.ContinueSessionRadioButton.Checked ?
                SessionCreationMode.Continue :
                SessionCreationMode.StartNew;
        }

        private void ContinueSessionListView_SelectedIndexChanged(object sender, EventArgs args)
        {
            int selectedCount = this.ContinueSessionListView.SelectedItems.Count;
            Debug.Assert(2 > selectedCount);

            this.CreationMode = selectedCount == 1 ?
                SessionCreationMode.Continue :
                SessionCreationMode.StartNew;
        }

        private void ContinueSessionListView_DoubleClick(object sender, EventArgs args)
        {
            Debug.Assert(1 == this.ContinueSessionListView.SelectedItems.Count);

            this.DialogOkButton_Click(this, args);
        }
    }

    #region = EventArgs ========================================================================================================

    /// <summary>
    /// Event argument for the <see cref="SessionSelector.StartNewSession"/> event.
    /// </summary>
    public class StartNewSessionEventArgs : EventArgs
    {
        public StartNewSessionEventArgs(string name, string description, bool setAsDefault)
        {
            this.Name = name;
            this.Description = description;
            this.SetAsDefault = setAsDefault;
        }

        public string Name { get; }
        public string Description { get; }
        public bool SetAsDefault { get; }
    }

    /// <summary>
    /// Event argument for the <see cref="SessionSelector.ContinueSession"/> event.
    /// </summary>
    public class ContinueSessionEventArgs : EventArgs
    {
        public ContinueSessionEventArgs(long sessionId, bool setAsDefault)
        {
            this.SessionId = sessionId;
            this.SetAsDefault = setAsDefault;
        }
        public long SessionId { get; }
        public bool SetAsDefault { get; }
    }

    #endregion
}
