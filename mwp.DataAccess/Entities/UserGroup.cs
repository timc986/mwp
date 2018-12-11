using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mwp.DataAccess.Entities
{
    public class UserGroup
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
