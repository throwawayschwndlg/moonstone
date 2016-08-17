using moonstone.ui.web.Models;
using System;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(Current current) : base(current)
        {
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

#if DEBUG

        [AllowAnonymous]
        public string Seeed()
        {
			string m = "schwindelig@schwindelig.ch";
			string p = "p@55w0rd";
			
            var res = this.Current.Authentication.UserManager.CreateAsync(
                new core.models.User
                {
                    Email = m,
                    Culture = "en-US",
                    CreateDateUtc = DateTime.UtcNow,
                    AutoUpdateTimeZone = true,
                    DateFormat = "yyyy-MM-dd"
                }, p);

            if (!res.Result.Succeeded)
            {
                return "failed to create user";
            }

            var user = this.Current.Services.UserService.GetUerByEmail(m);
            var group = this.Current.Services.GroupService.CreateGroup(new core.models.Group()
            {
                CreateUserId = user.Id,
                Description = $"Seeded default group for {m}",
                Name = "David's group",
                CreateDateUtc = DateTime.UtcNow
            });
            user.CurrentGroupId = group.Id;
            this.Current.Services.UserService.SetCurrentGroup(user.Id, group.Id);

            return "seeded";
        }

#endif
    }
}