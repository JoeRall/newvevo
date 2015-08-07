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
        public string ArtistId { get; set; }
        public int VideoYear { get; set; }

        public TimeSpan Duration { get; set; }

        public override bool Equals(object obj)
        {
            return Isrc.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Isrc.GetHashCode();
        }

        public long TotalViews { get; set; }

        public string Genre { get; set; }

        public string[] Genres()
        {
            if (null == Genre) return new string[0];

            return Genre.Split('|');
        }

        public override string ToString()
        {
            return string.Format(@"{0} '{1}' - {2} ({3})", Isrc, Title, Artist, VideoYear);
        }
    }
}
