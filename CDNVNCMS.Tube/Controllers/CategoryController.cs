using System.Linq;
using System.Web.Mvc;
using CDNVNCMS.Tube.Entities;

namespace CDNVNCMS.Tube.Controllers
{
    
    public class CategoryController : Controller
    {
        private TubeContext db = new TubeContext();
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _CategoryLayout(string current="")
        {
            ViewBag.CategoryCurrent = current;
            IQueryable<Category> cats = db.Categories.Where(f => f.Published).OrderBy(c=>c.Order);
            return PartialView("_CategoryMenu", cats);
        }
        public ActionResult _CategoryHome(string current = "")
        {
            ViewBag.CategoryCurrent = current;
            IQueryable<Category> cats = db.Categories.Where(f => f.Published).OrderBy(c => c.Order);
            return PartialView("_CategoryHome", cats);
        }
        
        public ActionResult CategoryDetails(string[] keyword)
        {
            var key = "";
            for (var i = keyword.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(keyword[i]))
                {
                    key = keyword[i];
                    break;
                }
            }
            var category = db.Categories.Single(c => c.SEOName.Equals(key));
            return View(category);
        }
    }
}
