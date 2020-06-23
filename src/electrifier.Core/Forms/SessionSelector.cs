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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// OK          == > Session ausgewählt und GO!
    /// Ignore      == > Neue Session
    /// Cancel      == > Abbruch, electrifier schliessen
    /// </summary>
    public partial class SessionSelector
      : Form
    {
        #region Fields ========================================================================================================

        public enum SessionCreationMode
        {
            Create,
            Continue,
        }

        private SessionCreationMode creationMode = SessionCreationMode.Create;

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public SessionContext SessionContext { get; }

        public SessionCreationMode CreationMode
        {
            get => this.creationMode;
            set
            {
                if (this.creationMode != value)
                {
                    this.creationMode = value;

                    if (this.CreationMode == SessionCreationMode.Create)
                    {
                        this.CreateSessionRadioButton.Checked = true;
                        this.ContinueSessionListView.SelectedItems.Clear();
                    }
                    else
                    {
                        this.ContinueSessionRadioButton.Checked = true;
                    }

                    this.DialogOkButton.DialogResult = SessionCreationMode.Create == value ? DialogResult.Ignore : DialogResult.OK;
                }
            }
        }

        public bool HasSessionsToContinue => this.ContinueSessionListView.Items.Count > 0;

        public override string Text
        {
            get => base.Text;
            set => base.Text = AppContext.BuildDefaultFormText(value);
        }

        #endregion ============================================================================================================

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

        public SessionSelector(SessionContext sessionContext)
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

            AppContext.TraceScope();

            this.InitializeComponent();
            this.Icon = sessionContext.ApplicationIcon;

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

        private void SessionSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppContext.TraceScope();
            Debug.Assert(this.CreateSessionRadioButton.Checked != this.ContinueSessionRadioButton.Checked);

            this.DialogResult = this.creationMode == SessionCreationMode.Create ?
                DialogResult.Ignore :
                DialogResult.OK;
        }

        private void CreateSessionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreationMode = this.CreateSessionRadioButton.Checked ?
                SessionCreationMode.Create :
                SessionCreationMode.Continue;
        }

        private void CreateSessionNameOrDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            this.CreationMode = SessionCreationMode.Create;
        }

        private void ContinueSessionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreationMode = this.ContinueSessionRadioButton.Checked ?
                SessionCreationMode.Continue :
                SessionCreationMode.Create;
        }

        private void ContinueSessionListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCount = this.ContinueSessionListView.SelectedItems.Count;
            Debug.Assert(2 > selectedCount);

            this.CreationMode = selectedCount == 1 ?
                SessionCreationMode.Continue :
                SessionCreationMode.Create;
        }

        //private void SessionStartRadioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.CreateSessionRadioButton.Checked)
        //    {
        //        this.DialogOkButton.DialogResult = DialogResult.Ignore;
        //    }
        //    else if (this.ContinueSessionRadioButton.Checked)
        //    {
        //        var selectedCount = this.ContinueSessionListView.SelectedItems.Count;
        //        Debug.Assert(2 > selectedCount);

        //        if (0 == selectedCount)
        //        {   // TODO: Liste könnte auch leer sein!
        //            Debug.Assert(this.ContinueSessionListView.Items[0] != null);
        //            this.ContinueSessionListView.Items[0].Selected = true;
        //        }

        //        this.DialogOkButton.DialogResult = DialogResult.OK;
        //    }
        //}
    }
}
