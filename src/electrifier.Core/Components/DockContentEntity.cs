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
