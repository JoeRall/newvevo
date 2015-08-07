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



        public static async Task<UserTuple> CreateUserIfNotExists(string id, ApplicationUserManager um)
        {
            var response = new UserTuple();

            //-- Create User if it Doesn't exist
            response.User = await um.FindByNameAsync(id);

            if (null == response.User)
            {
                response.IsNewUser = true;
                response.User = new ApplicationUser { UserName = id, Email = string.Format("{0:N}@email.com", Guid.NewGuid()) };
                var result = await um.CreateAsync(response.User);

                if (!result.Succeeded)
                    throw new Exception("Ooops. Something went really wrong");

                //-- Make sure that if we're using the Watched Videos, it's set and not null
                response.User.WatchedVideos = new List<WatchedVideo>();
            }

            return response;
        }
    }

    public class UserTuple
    {
        public bool IsNewUser { get; set; }
        public ApplicationUser User { get; set; }
    }
}