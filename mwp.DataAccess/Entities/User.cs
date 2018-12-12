using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace mwp.DataAccess.Entities
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("Id")]
        public UserGroup UserGroup { get; set; }
        public long UserGroupId { get; set; }
        [ForeignKey("Id")]
        public UserRole UserRole { get; set; }
        public long UserRoleId { get; set; }
    }
}
