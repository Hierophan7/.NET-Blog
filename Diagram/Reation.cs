namespace Diagram
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reation
    {
        public Guid Id { get; set; }

        public bool? Like { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationData { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }

        public Guid PostId { get; set; }

        public string UserIP { get; set; }

        public virtual Post Post { get; set; }
    }
}
