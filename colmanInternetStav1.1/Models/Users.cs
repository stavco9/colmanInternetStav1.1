using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Users
    {
        public Users()
        {
            Branch = new HashSet<Branch>();
            Purchase = new HashSet<Purchase>();
        }

        public string Email { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Gender { get; set; }
        public int Id { get; set; }
        public string NameId { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<Branch> Branch { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
