using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mwp.DataAccess.Dto
{
    public class CreateRecordRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "The FeelingId field is required.")]
        public long FeelingId { get; set; }
        public long RecordVisibilityId { get; set; }
    }
}
