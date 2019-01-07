using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mwp.DataAccess.Dto
{
    public class CreateRecordRequest
    {
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public long RecordVisibilityId { get; set; }
    }
}
