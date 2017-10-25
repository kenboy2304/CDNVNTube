using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CDNVNCMS.Tube.Entities;
using PagedList;

namespace CDNVNCMS.Tube.Controllers
{
    public class FilmController : Controller
    {
        private TubeContext db = new TubeContext();
        //
        // GET: /Film/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _NewsFilm(string category = "", int page = 1, int size = 12, bool havePaging = true)
        {
            ViewBag.HavePaging = havePaging;
            if (page <= 0) page = 1;
            var films = db.Films.Where(f => f.Published);
            ViewBag.CategorySEOName = "phim-moi";
            ViewBag.CategoryName = "Phim Mới";
            if (!String.IsNullOrWhiteSpace(category))
            {
                var cat = db.Categories.Single(item => item.SEOName.Equals(category));
                ViewBag.Id = cat.Id;
                ViewBag.CategorySEOName = category;
                ViewBag.CategoryName = cat.Name;
                films = films.Where(f => f.Published && f.Categories.Any(c => c.SEOName.Equals(category)));
            }
            if(havePaging==false) return PartialView("_HomeNewsFilm", films.OrderByDescending(f => f.Id).ToPagedList(page, size));
            return PartialView(films.OrderByDescending(f => f.Id).ToPagedList(page, size));
        }

        public ActionResult _RelatedFilms(string category = "")
        {
            var films = db.Films.Where(f => f.Published && f.Categories.Any(c => c.SEOName.Equals(category)));
            return PartialView("_RelatedFilms", films.OrderByDescending(f => f.Id).ToPagedList(1, 10));
        }
        public ActionResult _FeaturedFilm(string category = "")
        {
            var films = db.Films.Where(f => f.Published && f.IsTrainler);
            return PartialView("_FeaturedFilm", films.OrderByDescending(f => f.Id).ToPagedList(1, 10));
        }
        

        public JsonResult _RelatedFilmAjax(string category = "", int skin = 10, int size = 10)
        {
            var films = db.Films.Where(f => f.Published && f.Categories.Any(c => c.SEOName.Equals(category)))
                .OrderByDescending(f => f.Id).Skip(skin).Take(size).Select(f => new
                                                                            {
                                                                                Name = f.Name,
                                                                                SEOName = f.SEOName,
                                                                                ImageUrl = f.ImageUrl
                                                                            });
            if (string.IsNullOrWhiteSpace(category))
            {
                films = db.Films.Where(f => f.Published)
                .OrderByDescending(f => f.Id).Skip(skin).Take(size).Select(f => new
                {
                    Name = f.Name,
                    SEOName = f.SEOName,
                    ImageUrl = f.ImageUrl
                });
            }
            return Json(films.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ErrorFilm(string SEName, int order)
        {
            var filmpart = db.FilmParts.Single(p => p.Order == order && p.SEOName.StartsWith(SEName));
            filmpart.isError = true;
            db.Entry(filmpart).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilmDetails(string keyword = "")
        {
            
            ViewBag.Title = "Detail";
            var film = db.Films.Single(f => f.SEOName.Equals(keyword));
            return View(film);
        }

    }
}
