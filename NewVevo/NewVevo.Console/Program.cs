using NewVevo.Entity;
using NewVevo.Entity.Services;
using System.Linq;
using System;
using NewVevo.Entity.Models;

namespace NewVevo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new VevoContext();

            var svc = new RandomVideoService(db.Videos.ToList());

            var user = db.Users.First();
            var watched = user.WatchedVideos.ToList();

            System.Console.WriteLine("Press Enter to get a new video");
            while (System.Console.ReadLine() != "n")
            {
                var nextVid = svc.GetRandomVideo(user);

                var wv = new WatchedVideo { 
                    User = user,
                    Video = nextVid, 
                    WatchDate = DateTime.Now };
                
                db.WatchHistory.Add(wv);
                db.SaveChanges();

                System.Console.WriteLine(nextVid);
            }



        }
    }
}
