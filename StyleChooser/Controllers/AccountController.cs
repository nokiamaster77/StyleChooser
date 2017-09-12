using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StyleChooser.Models;

namespace StyleChooser.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            User user;
            using (var db = new DatabaseContext())
            {
                user = db.Users.FirstOrDefault(u => u.Login == model.Name && u.Password == model.Password);
            }
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(model.Name, true);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "User with such name and password was not found");
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            User user;
            using (var db = new DatabaseContext())
            {
                user = db.Users.FirstOrDefault(u => u.Login == model.Name);
            }
            if (user == null)
            {
                using (var db = new DatabaseContext())
                {
                    db.Users.Add(new User { Login = model.Name, Password = model.Password});
                    db.SaveChanges();

                    user = db.Users.FirstOrDefault(u => u.Login == model.Name && u.Password == model.Password);
                }
                if (user == null) return View(model);

                FormsAuthentication.SetAuthCookie(model.Name, true);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "User with such name already existed");
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}