namespace Diagram
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Complaint
    {
        public Guid Id { get; set; }

        [Required]
        public string ComplaintText { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationData { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }

        public Guid? UserId { get; set; }

        public Guid CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual User User { get; set; }
    }
}
