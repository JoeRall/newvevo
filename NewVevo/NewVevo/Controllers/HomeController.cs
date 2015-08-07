using NewVevo.Entity.Models;
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
        public async Task<ActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                //-- Create User if it Doesn't exist
                var user = await UserManager.FindByNameAsync(id);

                if (null == user)
                {
                    user = new ApplicationUser { UserName = id, Email = string.Format("{0:N}@email.com", Guid.NewGuid()) };
                    var result = await UserManager.CreateAsync(user);
                    
                    if (!result.Succeeded)
                        throw new Exception("Ooops. Something went really wrong");
                }
            }

            return View();
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