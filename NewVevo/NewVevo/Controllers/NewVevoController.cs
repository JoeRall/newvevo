using NewVevo.Entity;
using NewVevo.Entity.Models;
using NewVevo.Entity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
// Maybe this one too

namespace NewVevo.Controllers
{
    [EnableCors("*", "*", "*")]
    public class NewVevoController : ApiController
    {

        private static readonly RandomVideoService svc;
        private readonly HomeController _hc;
        static NewVevoController()
        {
            var db = new VevoContext();
            svc = new RandomVideoService(db.Videos.ToList());
            
        }

        private readonly VevoContext ctx = new VevoContext();
        private readonly VideoService vsvc = new VideoService();

        [HttpGet]
        public async Task<Video[]> Next(string userId)
        {
            var user = await GetUser(userId);

            if (null == user)
                return null;

            return svc.GetRandomVideos(user);
        }

        [HttpGet]
        public List<Stream> Streams(string isrc)
        {
            return vsvc.GetVideoUrls(isrc);
        }

        [HttpPost]
        public async Task MarkWatched(MWRequest request)
        {
            var user = await GetUser(request.UserId);

            var video = ctx.Videos.FirstOrDefault(v => v.Isrc == request.Isrc);

            var watched = new WatchedVideo
            {
                User = user,
                Video = video,
                WatchDate = DateTime.Now,
                AmountWatched = request.Duration,
                IsRoulette = request.IsRoulette,
                PausedVideo = request.HasPressedPaused
            };

            ctx.WatchHistory.Add(watched);
            ctx.SaveChanges();
        }

        private async Task<ApplicationUser> GetUser(string userId)
        {
            var user = await BaseController.CreateUserIfNotExists(userId, new ApplicationUserManager());
            
            return user;
        }

        public class MWRequest
        {
            public string UserId { get; set; }
            public string Isrc { get; set; }
            public TimeSpan Duration { get; set; }
            public bool IsRoulette { get; set; }
            public bool HasPressedPaused { get; set; }
        }
    }
}
