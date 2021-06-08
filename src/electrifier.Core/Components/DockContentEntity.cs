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

using EntityLighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace electrifier.Core.Components
{
    #region = DockContentEntity ================================================================================================

    [Table(Name = "DockContent")]
    public class DockContentEntity
    {
        [PrimaryKey(DataType.Integer)]
        public long Id { get; protected set; }

        [Column(DataType.Text, Constraints = Constraint.NotNull | Constraint.Unique)]
        public Guid Guid { get; protected set; }

        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime DateCreated { get; protected set; }

        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime DateModified { get; protected set; }

    }

    #endregion =================================================================================================================
}
