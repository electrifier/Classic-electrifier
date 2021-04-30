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

using RibbonLib.Controls;
using RibbonLib.Controls.Events;
using RibbonLib.Controls.Properties;
using System;

namespace electrifier.Core.Components
{
    // INotifyPropertyChanged
    // https://docs.microsoft.com/de-de/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=net-5.0


    public interface IRibbonConsumer
    {
        RibbonItems RibbonItems { get; }

        //        RibbonConsumerStateMap RibbonStateMap { get; }


        BaseRibbonControlBinding<BaseRibbonControl>[] InitializeRibbonBinding(RibbonItems ribbonItems);

        void ActivateRibbonState();

        // TODO 27.04.21, 17:17: IsActivelyBoundToRibbon() Property


        /// <summary>
        /// Eigentlich Unsinn, weil wir könnten auch alle RibbonControls abklappern und
        /// diejenigen, die im neuen DockPanel nicht gesetzt sind einfach ausknippsen...
        /// </summary>
        void DeactivateRibbonState();

    }

    public class BaseRibbonControlBinding<T>
        where T : BaseRibbonControl
    {
        public T BaseRibbonControl { get; }

        public BaseRibbonControlBinding(T baseRibbonControl)
        {
            this.BaseRibbonControl = baseRibbonControl;
        }
    }


    /// <summary>
    /// The underlying <see cref="RibbonTab"/> derives from:
    /// <list type="bullet">
    /// <item><term>BaseRibbonControl</term><description>BaseRibbonControl</description></item>
    /// <item><term>IKeytipPropertiesProvider</term><description>IKeytipPropertiesProvider</description></item>
    /// <item><term>ILabelPropertiesProvider</term><description>ILabelPropertiesProvider</description></item>
    /// <item><term>ITooltipPropertiesProvider</term><description>ITooltipPropertiesProvider</description></item>
    /// </list>
    /// </summary>
    public class RibbonTabBinding :
        BaseRibbonControlBinding<RibbonTab>
    {
        public RibbonTabBinding(RibbonTab ribbonTab) : base(ribbonTab) { }
        // TODO: => Where is Visible property!?!

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding">The instance of this <see cref="RibbonTabBinding"/>.</param>
        public static implicit operator BaseRibbonControlBinding<BaseRibbonControl>(RibbonTabBinding binding) => binding;
    }

    /// <summary>
    /// The underlying <see cref="RibbonButton"/> derives from:
    /// <list type="bullet">
    /// <item><term>BaseRibbonControl</term><description>BaseRibbonControl</description></item>
    /// <item><term>IEnabledPropertiesProvider</term><description>IEnabledPropertiesProvider</description></item>
    /// <item><term>IKeytipPropertiesProvider</term><description>IKeytipPropertiesProvider</description></item>
    /// <item><term>ILabelPropertiesProvider</term><description>ILabelPropertiesProvider</description></item>
    /// <item><term>ILabelDescriptionPropertiesProvider</term><description>ILabelDescriptionPropertiesProvider</description></item>
    /// <item><term>IImagePropertiesProvider</term><description>IImagePropertiesProvider</description></item>
    /// <item><term>ITooltipPropertiesProvider</term><description>ITooltipPropertiesProvider</description></item>
    /// <item><term>IExecuteEventsProvider</term><description>IExecuteEventsProvider</description></item>
    /// </list>
    /// </summary>
    public class RibbonButtonBinding :
        BaseRibbonControlBinding<RibbonButton>,
        IEnabledPropertiesProvider,
        IExecuteEventsProvider
    {
        private bool enabled;
        public bool Enabled
        {
            get => this.enabled;
            set
            {
                this.enabled = value;
                // TODO: Enable base button! But only if this RibbonConsumer is the active DockPanelDocument
            }
        }

        public event EventHandler<ExecuteEventArgs> ExecuteEvent;

        public RibbonButtonBinding(RibbonButton ribbonButton, EventHandler<ExecuteEventArgs> executeEvent, bool enabled = true)
            : base(ribbonButton)
        {
            this.ExecuteEvent = executeEvent;
            this.Enabled = enabled;
        }

        public static implicit operator BaseRibbonControlBinding<BaseRibbonControl>(RibbonButtonBinding binding) => binding;
    }

    /// <summary>
    /// The underlying <see cref="RibbonSplitButton"/> derives from:
    /// <list type="bullet">
    /// <item><term>RibbonSplitButton</term><description>RibbonSplitButton</description></item>
    /// <item><term>BaseRibbonControl</term><description>BaseRibbonControl</description></item>
    /// <item><term>IEnabledPropertiesProvider</term><description>IEnabledPropertiesProvider</description></item>
    /// <item><term>IKeytipPropertiesProvider</term><description>IKeytipPropertiesProvider</description></item>
    /// <item><term>ITooltipPropertiesProvider</term><description>ITooltipPropertiesProvider</description></item>
    /// </list>
    /// </summary>
    public class RibbonSplitButtonBinding :
        BaseRibbonControlBinding<RibbonSplitButton>,
        IEnabledPropertiesProvider
    {
        private bool enabled;
        public bool Enabled
        {
            get => this.enabled;
            set
            {
                this.enabled = value;
                // TODO: Enable base button! But only if this RibbonConsumer is the active DockPanelDocument
            }
        }

        public RibbonSplitButtonBinding(RibbonSplitButton ribbonSplitButton, bool enabled = true)
            : base(ribbonSplitButton)
        {
            this.Enabled = enabled;
        }

        public static implicit operator BaseRibbonControlBinding<BaseRibbonControl>(RibbonSplitButtonBinding binding) => binding;
    }
}
