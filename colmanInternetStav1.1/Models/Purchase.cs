using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Purchase
    {
        public int Id { get; set; }
        public int JewelryId { get; set; }
        public int? UserId { get; set; }

        public virtual Jewelry Jewelry { get; set; }
        public virtual Users User { get; set; }
    }
}
