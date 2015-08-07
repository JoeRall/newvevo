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
        public async Task<NextResponse> Next(string userId)
        {
            var user = await GetUser(userId);

            if (null == user)
                return null;

            var response = new NextResponse();

            response.Videos = svc.GetRandomVideos(user.User);
            response.IsNewUser = user.IsNewUser;

            response.WatchHistory = user.User.WatchedVideos;
           
            //-- We want to remove the cirucular references here for the json serializer
            foreach (var wv in user.User.WatchedVideos)
            {
                wv.User = null;
            }
            return response;
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
                User = user.User,
                Video = video,
                WatchDate = DateTime.Now,
                AmountWatched = request.Duration,
                IsRoulette = request.IsRoulette,
                PausedVideo = request.HasPressedPaused
            };

            ctx.WatchHistory.Add(watched);
            ctx.SaveChanges();
        }

        private async Task<UserTuple> GetUser(string userId)
        {
            return await BaseController.CreateUserIfNotExists(userId, new ApplicationUserManager());
        }

        public class MWRequest
        {
            public string UserId { get; set; }
            public string Isrc { get; set; }
            public TimeSpan Duration { get; set; }
            public bool IsRoulette { get; set; }
            public bool HasPressedPaused { get; set; }
        }


        public class NextResponse
        {
            public Video[] Videos { get; set; }
            public bool IsNewUser { get; set; }
            public ICollection<WatchedVideo> WatchHistory { get; set; }
        }
    }
}
