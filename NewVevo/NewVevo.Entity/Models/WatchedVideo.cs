using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewVevo.Entity.Models
{
    public class WatchedVideo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        
        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }
        public string VideoId { get; set; }

        [Required]
        public DateTime WatchDate { get; set; }
        
        [Required]
        public TimeSpan AmountWatched { get; set; }

        public bool IsRoulette { get; set; }

        public bool PausedVideo { get; set; }
    }
}
