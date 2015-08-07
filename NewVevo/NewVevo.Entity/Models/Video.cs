using System;
using System.ComponentModel.DataAnnotations;

namespace NewVevo.Entity.Models
{
    public class Video
    {
        [Key]
        public string Isrc { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime Premiered { get; set; }

        public TimeSpan Duration { get; set; }

        public override bool Equals(object obj)
        {
            return Isrc.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Isrc.GetHashCode();
        }
    }
}
