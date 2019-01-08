using System;
using System.Collections.Generic;
using System.Text;

namespace mwp.DataAccess.Dto
{
    public class LoginResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
        public long UserGroupId { get; set; }
        public long UserRoleId { get; set; }
        public string Token { get; set; }
    }
}
