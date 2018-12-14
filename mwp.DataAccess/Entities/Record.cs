using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace mwp.DataAccess.Entities
{
    public class Record
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [ForeignKey("Id")]
        public User User { get; set; }
        public long UserId { get; set; }
        [ForeignKey("Id")]
        public RecordVisibility RecordVisibility { get; set; }
        public long RecordVisibilityId { get; set; }
    }
}
