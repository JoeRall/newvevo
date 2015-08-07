using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewVevo.Entity.Models
{
    public class WatchHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public ApplicationUser User { get; set; }

        public virtual ICollection<Video> WatchedVideo { get; set; }
    }
}
