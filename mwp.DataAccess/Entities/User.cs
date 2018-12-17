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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [ForeignKey("UserGroupId")]
        public virtual UserGroup UserGroup { get; set; }
        public long UserGroupId { get; set; }
        [ForeignKey("UserRoleId")]
        public virtual UserRole UserRole { get; set; }
        public long UserRoleId { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
