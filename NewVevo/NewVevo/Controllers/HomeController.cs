using NewVevo.Entity;
using NewVevo.Entity.Models;
using NewVevo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewVevo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly VevoContext ctx = new VevoContext();

        public async Task<ActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await BaseController.CreateUserIfNotExists(id, UserManager);
            }

            return View();
        }

        

        public async Task<ActionResult> History(string id)
        {
            var m = new HistoryModel();
            
            if (!string.IsNullOrEmpty(id))
            {
                var ut = await BaseController.CreateUserIfNotExists(id, UserManager);
                m.User = ut.User;
                m.WatchHistory = ut.User.WatchedVideos;
                m.IsValid = true;
            }
            else
            {
                m.IsValid = false;
            }

            return View(m);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}