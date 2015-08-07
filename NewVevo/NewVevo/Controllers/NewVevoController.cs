using NewVevo.Entity;
using NewVevo.Entity.Models;
using NewVevo.Entity.Services;
using System;
using System.Linq;
using System.Web.Http;

namespace NewVevo.Controllers
{
    public class NewVevoController : ApiController
    {
        private static readonly RandomVideoService svc;

        static NewVevoController()
        {
            var db = new VevoContext();
            svc = new RandomVideoService(db.Videos.ToList());
        }

        private readonly VevoContext ctx = new VevoContext();

        [HttpGet]
        public Video[] Next(string userId)
        {
            var user = ctx.Users.FirstOrDefault(u => u.Id == userId);

            if (null == user)
                return null;

            return svc.GetRandomVideos(user);
        }

        [HttpPost]
        public void MarkWatched(string userId, string isrc, TimeSpan duration)
        {
            var user = ctx.Users.FirstOrDefault(u => u.Id == userId);

            var video = ctx.Videos.FirstOrDefault(v => v.Isrc == isrc);

            var watched = new WatchedVideo
            {
                User = user,
                Video = video,
                WatchDate = DateTime.Now,
                AmountWatched = duration
            };

            ctx.WatchHistory.Add(watched);
            ctx.SaveChanges();
        }
    }
}
