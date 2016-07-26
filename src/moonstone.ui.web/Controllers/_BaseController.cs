using Microsoft.AspNet.Identity;
using moonstone.ui.web.Models;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace moonstone.ui.web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public Current Current { get; set; }

        public BaseController(Current current)
        {
            this.Current = current;
        }

        public void AddModelError(string message)
        {
            this.ModelState.AddModelError(string.Empty, message);
        }

        protected void HandleError(Exception e)
        {
            // TODO: Add logging
        }

        protected ActionResult JsonError(dynamic data, string message)
        {
            return this.JsonError(data, message, returnUrl: null);
        }

        protected ActionResult JsonError(dynamic data, string message, string returnUrl)
        {
            return this.JsonResponse(false, data, message, returnUrl);
        }

        protected ActionResult JsonResponse(bool success, dynamic data, string message, string returnUrl)
        {
            JsonNetResult result = new JsonNetResult();
            //result.Formatting = Formatting.Indented; // for nice looking results
            result.Formatting = Formatting.None;
            result.Data = new { success = success, message = message, data = data, returnUrl = returnUrl };

            return result;
        }

        protected ActionResult JsonSuccess(dynamic data, string message)
        {
            return this.JsonSuccess(data, message, returnUrl: null);
        }

        protected ActionResult JsonSuccess(dynamic data, string message, string returnUrl)
        {
            return this.JsonResponse(true, data, message, returnUrl: returnUrl);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                this.Current.Services.EnvironmentService.SetCulture(this.Current.User.Culture);

                var logoutRoute = Routes.Logout;
                var selectGroupRoute = Routes.SelectGroup;
                if (!this.Current.User.CurrentGroupId.HasValue &&
                    !selectGroupRoute.IsSame(filterContext.RouteData) &&
                    !logoutRoute.IsSame(filterContext.RouteData))
                {
                    filterContext.Result = this.RedirectToAction(selectGroupRoute.Action, selectGroupRoute.Controller);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var res = Routes.Error;
            filterContext.Result = this.RedirectToAction(res.Action, res.Controller);

            base.OnException(filterContext);
        }
    }
}