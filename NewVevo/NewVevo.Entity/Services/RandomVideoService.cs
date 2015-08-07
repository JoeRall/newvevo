using NewVevo.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewVevo.Entity.Services
{
    public class RandomVideoService
    {
        private readonly List<Video> _library;
        private static readonly Random _r = new Random();

        public RandomVideoService(List<Video> videoLibrary)
        {
            _library = videoLibrary;
        }

        public Video[] GetRandomVideos(ApplicationUser user, int count = 10)
        {
            var videosIHaventSeen = _library.Except(user.WatchedVideos.Select(v => v.Video)).ToList();

            var ran = new List<Video>();

            for (int i = 0; i < count; i++)
            {
                var v = videosIHaventSeen.Skip(GetRandomVid(videosIHaventSeen)).First();

                videosIHaventSeen.Remove(v);
                ran.Add(v);
            }

            return ran.ToArray();
        }

        private static int GetRandomVid(List<Video> videosIHaventSeen)
        {
            return _r.Next(videosIHaventSeen.Count - 1);
        }

        public Video GetRandomVideo(ApplicationUser user)
        {
            var vids = GetRandomVideos(user);

            return vids.First();
        }
    }
}
