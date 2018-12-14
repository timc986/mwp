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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public long UserId { get; set; }
        [ForeignKey("RecordVisibilityId")]
        public virtual RecordVisibility RecordVisibility { get; set; }
        public long RecordVisibilityId { get; set; }
    }
}
