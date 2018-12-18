using System;
using System.ComponentModel.DataAnnotations;

namespace mwp.DataAccess.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public long UserGroupId { get; set; }
        public long UserRoleId { get; set; }
    }
}
