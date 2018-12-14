﻿using System;
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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [ForeignKey("Id")]
        public UserGroup UserGroup { get; set; }
        public long UserGroupId { get; set; }
        [ForeignKey("Id")]
        public UserRole UserRole { get; set; }
        public long UserRoleId { get; set; }
    }
}
