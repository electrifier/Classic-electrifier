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

using electrifier.Core.Forms;
using System;
using System.Collections;
using System.Windows.Forms;
using Vanara.PInvoke;
using Vanara.Windows.Shell;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components.DockContents
{
    public partial class SelectConditionalBox
        : DockContent
    {
        public SelectConditionalBox()
        {
            this.InitializeComponent();
        }

        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate { this.selectitemsinternal(); });
        }

        void selectitemsinternal()
        {
//            var founditems = new ArrayList();
//            var searchpatterncount = 0;

//            try
//            {
//                var lines = this.textBox1.Lines;
//                var searchpatternlist = new ArrayList();

//                // remove folderpath of lines
//                foreach (string fullfilename in lines)
//                    searchpatternlist.Add(System.IO.Path.GetFileName(fullfilename));
//                searchpatterncount = searchpatternlist.Count;

//                // get filenamelist from shellfolderdockcontent

//                IDockContent dockContent = this.DockPanel.ActiveDocument;

//                if (dockContent is ShellFolderDockContent)
//                {
//                    var shellfolderdock = dockContent as ShellFolderDockContent;
//                    shellfolderdock.UnselectAll();

//                    var shellItems = shellfolderdock.ShellItems;

//                    AppContext.TraceDebug($"0.) Selecting conditional, searching a in list of { shellItems.Count } items...");

//                    for (int i = 0; i < shellItems.Count; i++)
//                    {
//                        //AppContext.TraceDebug($" index i = {i}");

//                        var shellItem = shellItems[i];
//                        //AppContext.TraceDebug($" index i = {i}");
//                        AppContext.TraceDebug("1.)   Getting DisplayName");
//                        //var itemName = shellItem.GetDisplayName(ShellItemDisplayString.ParentRelative);
//                        var itemName = shellItem.Name;
////                        var itemName = shellItem.GetDisplayName()
                        
//                        AppContext.TraceDebug($"2.)     index i = {i}");
//                        AppContext.TraceDebug($"3.)       Checking SelectionState for item {i}: {itemName}");

//                        foreach (var searchpattern in searchpatternlist)
//                        {
//                            AppContext.TraceDebug($"4.)         Foreach pattern");

//                            if (itemName.Equals(searchpattern as string, StringComparison.CurrentCultureIgnoreCase))
//                            {
//                                AppContext.TraceDebug($"5.)         Adding item to list");

//                                if (shellfolderdock.SetSelectionState(shellItem))
//                                    founditems.Add(shellItem.Name);
//                            }
//                        }
//                    }

//                    //
//                    // OLD foreach-statement, which fails for unknown reason, cause not all items appear...
//                    //foreach (ShellItem shellItem in shellItems)
//                    //{
//                    //    var filename = shellItem.GetDisplayName(ShellItemDisplayString.ParentRelative);

//                    //    AppContext.TraceDebug($"filename: {filename}");

//                    //    // now check each filename agains the given list
//                    //    foreach (var searchpattern in searchpatternlist)
//                    //    {
//                    //        if (filename.Equals(searchpattern as string, StringComparison.CurrentCultureIgnoreCase))
//                    //        {
//                    //            if (shellfolderdock.SetSelectionState(shellItem))
//                    //                founditems.Add(shellItem.Name);
//                    //        }
//                    //    }
//                    //}
//                }
//                else throw new ArgumentOutOfRangeException(nameof(ShellFolderDockContent));
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }

//            /*
//             * TODO: If there is already one item selected nothing happens here!
//             */

//            if (founditems.Count > 0)
//            {
//                var resultstring = $"Searching { searchpatterncount} items in folder found { founditems.Count } matching item(s):\n\n";

//                foreach (var filename in founditems)
//                    resultstring += filename + "\n";

//                MessageBox.Show(resultstring, $"Select Conditional: { founditems.Count } selected");
//                //this.DockPanel.Focus();

//                IDockContent dockContent = this.DockPanel.ActiveDocument;
//                if (dockContent is ShellFolderDockContent)
//                {
//                    var shellfolderdock = dockContent as ShellFolderDockContent;
//                    shellfolderdock.Focus();
//                }

//            }

        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Not implemented yet!");
        }
    }
}
