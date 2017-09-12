using System.Linq;
using System.Web.Mvc;
using StyleChooser.Models;

namespace StyleChooser.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
        [HttpPost]
        [Authorize]
        public ActionResult Styles(int userChoice)
        {
            var currentUser = User.Identity.Name;
            if (Session[currentUser] == null)
            { 
                using (var db = new DatabaseContext())
                {
                    var styles = (from s in db.Styles
                                 select s.Name).ToList();
                    if (styles.Count == 0) return HttpNotFound();

                    Session[currentUser] = new ImageHandler(styles);
                }
            }
            var imgHandler = Session[currentUser] as ImageHandler;

            if (imgHandler == null) return HttpNotFound();

            if (userChoice > -1)
            {
                imgHandler.SetUserChoice(userChoice);
            }
            else
            {
                imgHandler.ResetStep();
            }

            imgHandler.NextStep();
            Session[currentUser] = imgHandler;

            if (imgHandler.HasWinner)
            {
                return RedirectToAction("Result", "Home", new {winner = imgHandler.GetWinner()});
            }
            return View(imgHandler.GetImageData());
        }
        [Authorize]
        public ActionResult Result(string winner)
        {
            ViewBag.Winner = winner;
            using (var db = new DatabaseContext())
            {
                var currentUser = User.Identity.Name;
                var imgHandler = Session[currentUser] as ImageHandler;
                if (imgHandler == null) return HttpNotFound();

                var user = db.Users.FirstOrDefault(u => u.Login == currentUser);
                if (user == null) return HttpNotFound();

                for (var i = 0; i < imgHandler.Max; i++)
                {
                    var styleName = imgHandler.GetStyleName(i);
                    var style = db.Styles.FirstOrDefault(s => s.Name == styleName);
                    if (style == null) continue;

                    var userStyle = db.UserStyles.FirstOrDefault(us => us.UserId == user.UserId && us.StyleId == style.StyleId);
                    if (userStyle == null)
                    {
                        db.UserStyles.Add(new UserStyle
                        {
                            UserId = user.UserId,
                            StyleId = style.StyleId,
                            Count = imgHandler.GetStyleClickCount(i)
                        });
                    }
                    else
                    {
                        userStyle.Count = imgHandler.GetStyleClickCount(i);
                    }
                    db.SaveChanges();
                }

                var winStyle = db.Styles.FirstOrDefault(s => s.Name == winner);
                if (winStyle != null)
                {
                    user.StyleId = winStyle.StyleId;
                    db.SaveChanges();
                }
                Session[currentUser] = null;
            }
            return View();
        }

        [Authorize]
        public ActionResult Statistics()
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                if (user == null) return HttpNotFound("Such user not found.");
                if (user.StyleId == null) return Content("You have no favorite style yet.");

                var style = db.Styles.Find(user.StyleId);
                ViewBag.Favorite = style.Name;
                var userStyles = (from us in db.UserStyles
                    join s in db.Styles on us.StyleId equals s.StyleId
                    where us.UserId == user.UserId
                    select new Stat {Style = s.Name, ClickCount = us.Count}).ToList();
                    
                return View(userStyles);
            }
        }
    }
}