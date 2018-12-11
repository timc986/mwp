using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mwp.DataAccess.Entities
{
    public class UserRole
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
