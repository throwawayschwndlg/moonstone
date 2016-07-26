using Microsoft.AspNet.Identity.Owin;
using moonstone.core.i18n;
using moonstone.core.services.results;
using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(Current current) : base(current)
        {
        }

        [HttpGet]
        public ActionResult GetProfileInformation()
        {
            try
            {
                var currentUser = this.Current.User;

                var res = new
                {
                    //email = currentUser.Email,
                    culture = currentUser.Culture,
                    timeZone = currentUser.TzdbTimeZoneId,
                    autoUpdateTimeZone = currentUser.AutoUpdateTimeZone
                };

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                return this.JsonError(data: null, message: ValidationResources.User_GetProfileInformation_Error);
            }
        }

        [HttpGet]
        public ActionResult GetTimeZones()
        {
            var res = TimeZoneUtils.GetAvailableTimeZones()
                    .Select(tz => new { name = tz, value = tz }).OrderBy(m => m.name);

            return this.JsonSuccess(data: res, message: null);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoggedOut()
        {
            var res = Routes.Login;
            return this.RedirectToAction(res.Action, res.Controller);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = this.Current.Services.LoginService.Login(
                        username: model.Email,
                        password: model.Password,
                        isPersistent: false,
                        shouldLockOut: false);

                    if (result == LoginResult.Success)
                    {
                        return this.JsonSuccess(
                            data: null,
                            message: string.Format(ValidationResources.User_Login_Success, model.Email),
                            returnUrl: Routes.Home.GetActionLink(Url));
                    }
                    else
                    {
                        switch (result)
                        {
                            case LoginResult.LockedOut:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_LockedOut);

                            case LoginResult.RequiresVerification:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_RequiresVerification);

                            case LoginResult.Failed:
                            default:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_Failed);
                        }
                    }
                }
                else
                {
                    return this.JsonError(data: null, message: ValidationResources.Generic_ModelState_Error);
                }
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.Current.Services.LoginService.Logout();

            var res = Routes.LoggedOut;
            return RedirectToAction(res.Action, res.Controller);
        }

        [HttpGet]
        public ActionResult SelectGroup()
        {
            var model = new SelectGroupViewModel();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectGroup(SelectGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.Current.Services.UserService.SetCurrentGroup(userId: Current.UserId.Value, groupId: model.GroupId);

                var res = Routes.Home;
                return this.RedirectToAction(res.Action, res.Controller);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetCulture(string culture)
        {
            this.Current.Services.UserService.SetCulture(this.Current.UserId.Value, culture);

            var res = Routes.Home;
            return this.RedirectToAction(res.Action, res.Controller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetTimeZone(string timeZone)
        {
            try
            {
                this.Current.Services.UserService.SetTimeZone(this.Current.UserId.Value, timeZone);

                return this.JsonSuccess(
                    data: null,
                    message: string.Format(ValidationResources.User_SetTimeZone_Success, timeZone));
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Settings()
        {
            var currentUser = this.Current.User;

            var model = new SettingsViewModel
            {
                Email = currentUser.Email,
                TimeZone = currentUser.TzdbTimeZoneId,
                AutoUpdateTimeZone = currentUser.AutoUpdateTimeZone
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.Current.Services.UserService.UpdateSettings(this.Current.UserId.Value, model.TimeZone, model.AutoUpdateTimeZone);
                    return this.JsonSuccess(data: null, message: ValidationResources.User_ProfileSettings_Success);
                }
                catch (Exception e)
                {
                    this.HandleError(e);
                    return this.JsonError(data: null, message: ValidationResources.Generic_Error);
                }
            }
            else
            {
                return this.JsonError(data: null, message: ValidationResources.Generic_ModelState_Error);
            }
        }
    }
}