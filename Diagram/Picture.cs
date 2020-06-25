namespace Diagram
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Picture
    {
        public Guid Id { get; set; }

        public string PictureName { get; set; }

        public string PicturePath { get; set; }

        public Guid? PostId { get; set; }

        public Guid? UserId { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
