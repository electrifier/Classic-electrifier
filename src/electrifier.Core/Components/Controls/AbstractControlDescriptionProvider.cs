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

namespace electrifier.Core.Components.Controls
{
    /// <summary>
    /// Allows using Visual Studio Designer on controls that derive from abstract base classes.
    /// 
    /// <br/><br/>
    /// 
    /// Code provided by <see href="https://stackoverflow.com/users/2584597/jucardi">jucardi</see> on StackOverflow.com:
    /// <see href="https://stackoverflow.com/a/17661276">How can I get Visual Studio 2008 Windows Forms
    /// designer to render a Form that implements an abstract base class?</see>
    /// 
/// Usage: Apply 
    /// 
    /// </summary>
    /// <typeparam name="TAbstract">Abstract class the control derives from</typeparam>
    /// <typeparam name="TBase">Base class </typeparam>

    public class AbstractControlDescriptionProvider<TAbstract, TBase>
        : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
