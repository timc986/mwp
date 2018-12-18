using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mwp.DataAccess.Dto
{
    public class RecordDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long RecordVisibilityId { get; set; }
    }
}
