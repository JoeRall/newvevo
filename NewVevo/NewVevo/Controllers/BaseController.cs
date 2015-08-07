using Microsoft.AspNet.Identity.Owin;
using NewVevo.Entity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewVevo.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
             set
            {
                _userManager = value;
            }
        }


        public static async Task<ApplicationUser> CreateUserIfNotExists(string id, ApplicationUserManager um)
        {
            //-- Create User if it Doesn't exist
            var user = await um.FindByNameAsync(id);

            if (null == user)
            {
                user = new ApplicationUser { UserName = id, Email = string.Format("{0:N}@email.com", Guid.NewGuid()) };
                var result = await um.CreateAsync(user);

                if (!result.Succeeded)
                    throw new Exception("Ooops. Something went really wrong");

                //-- Make sure that if we're using the Watched Videos, it's set and not null
                user.WatchedVideos = new List<WatchedVideo>();
            }

            return user;
        }
    }
}