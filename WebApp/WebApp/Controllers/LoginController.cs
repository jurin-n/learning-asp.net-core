using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using WebApp.Services;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private Authentication auth;

        public LoginController(AppConfig appConfig)
        {
            auth = new Authentication(appConfig);
        }

        public IActionResult Index()
        {
            return View(new User() { UserId = "", Password = "", isValid = true });
        }

        [HttpPost]
        public ActionResult Index(User user, String url)
        {
            if (ModelState.IsValid)
            {
                var result = auth.IdAndPasswordSignIn(user.UserId, user.Password);
                if (result == 0)
                {
                    //認証成功した場合
                    //Session["UserId"] = user.UserId;
                    HttpContext.Session.SetString("UserId", user.UserId);

                    if (string.IsNullOrEmpty(url))
                    {
                        return Redirect("home");
                    }
                    return Redirect(url);
                }
            }
            user.isValid = false;
            return View(user);
        }
    }
}
