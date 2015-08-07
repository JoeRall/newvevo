using NewVevo.Entity.Models;
using System.Collections.Generic;

namespace NewVevo.Models
{
    public class HistoryModel
    {
        public ApplicationUser User { get; set; }
        public ICollection<WatchedVideo> WatchHistory { get; set; }
        public bool IsValid { get; set; }
    }
}