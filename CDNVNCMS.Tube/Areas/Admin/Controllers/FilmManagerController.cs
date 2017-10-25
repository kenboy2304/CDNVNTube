using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CDNVNCMS.Tube.Entities;
using PagedList;
using WebGrease.Css.Extensions;

namespace CDNVNCMS.Tube.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class FilmManagerController : Controller
    {
        private readonly TubeContext db = new TubeContext();

        //
        // GET: /Admin/Film/

        public ActionResult Index(string s, int page = 1, int size = 20)
        {

            if (page == 0) page = 1;
            var films = string.IsNullOrWhiteSpace(s)? db.Films.OrderByDescending(f => f.Id).ToPagedList(page, size)
                                                    :db.Films.Where(item=> item.Name.ToLower().Contains(s.ToLower())).OrderByDescending(item=>item.CreatedDate).ToPagedList(page, size);
            ViewBag.Button = "Tất Cả Phim";
            return View(films);
        }
        public ActionResult Featured(int page = 1, int size = 20)
        {
            if (page == 0) page = 1;
            var films = db.Films.Where(item => item.IsTrainler).OrderByDescending(item => item.CreatedDate).ToPagedList(page, size);
            ViewBag.Button = "Phim Nổi Bật";
            return View("Index", films);
        }

        //
        // GET: /Admin/Film/Error

        public ActionResult Error(int page = 1, int size = 10)
        {
            if (page == 0) page = 1;
            var films = db.Films.Where(f=>f.FilmParts.Any(p=>p.isError)).OrderByDescending(f => f.Id).ToPagedList(page, size);
            ViewBag.Button = "Phim Báo Lỗi";
            return View("Index",films);
        }

        public ActionResult ErrorDetails(int id)
        {
            var film = db.Films.Single(f=>f.Id==id);
            return View(film);
        }
        [ValidateInput(false), HttpPost]
        public ActionResult UpdatePartFilm(int id, string data, string type, bool error)
        {
            var part = db.FilmParts.Find(id);
            if (part == null)
            {
                HttpNotFound();
            }
            part.isError = error;
            part.VideoData = data;
            part.VideoType = type;
            db.Entry(part).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ErrorDetails", new {id = part.FilmId});
        }


        //
        // POST: /Admin/Film/Create

        private List<Category> GetCategory(int[] cat)
        {
            var categories = new List<Category>();
            if (cat.Any())
            {
                foreach (var id in cat)
                {
                    var ct = db.Categories.Single(c => c.Id == id);
                    categories.Add(ct);
                }
            }
            return categories;
        }

        private List<FilmPart> GetPart(string[] partData,string[] partType, string url)
        {
            var partList = new List<FilmPart>();
            if (partData == null) return new List<FilmPart>();
            
            for (var i =0; i<partData.Length; i++)
            {
                if(!string.IsNullOrEmpty(partData[i]))
                partList.Add(new FilmPart
                                  {
                                      CreatedDate = DateTime.Now,
                                      ModifiedDate = DateTime.Now,
                                      Published = true,
                                      SEOName = url + "-phan-" + (i+1),
                                      VideoData = partData[i],
                                      VideoType = partType[i],
                                      Order = i+1
                                  });
            }
            return partList;
        }
        public ActionResult Create()
        {
            return View(new Film());
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Film film, int[] Cat, string[] part, string[] partType)
        {
            if (ModelState.IsValid)
            {
                film.CreatedDate = film.ModifiedDate = DateTime.Now;
                film.Categories = GetCategory(Cat);
                film.FilmParts = GetPart(part, partType, film.SEOName);
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        public ActionResult ViewNumber(int[] data)
        {
            var r = "";
            foreach (var i in data)
            {
                r = r + i + ", ";
            }
            return Content("xem");
        }

        //
        // GET: /Admin/Film/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        //
        // POST: /Admin/Film/Edit/5

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, int[] Cat, string[] part, string[] partType, int[] pId)
        {
            var film = db.Films.Single(f => f.Id == Id);
            TryUpdateModel(film);
            if (ModelState.IsValid)
            {
                film.Categories.Clear();
                film.Categories = GetCategory(Cat);
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                //UpdateParts(part, pId, film.Id, film.SEOName);
                db.FilmParts.RemoveRange(db.FilmParts.Where(item => item.FilmId == film.Id));
                for (var i = 0; i < part.Length; i++)
                {
                    if(!string.IsNullOrWhiteSpace(part[i]))
                        db.FilmParts.Add(new FilmPart
                        {
                            FilmId = film.Id,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Published = true,
                            SEOName = film.SEOName + "-phan-" + i + 1,
                            Order = i + 1,
                            VideoData = part[i],
                            VideoType = partType[i]
                        });
                }
                db.SaveChanges();
                Task.Run( () =>
                {
                    
                });
                return RedirectToAction("Index");
            }
            return View(film);
        }
        [HttpPost]
        public ActionResult ChangeStatus(int id = 0, string name = "Published")
        {
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            if (name == "Published") film.Published = !film.Published;
            if (name == "Featured") film.IsTrainler = !film.IsTrainler;
            db.Entry(film).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Đã cập nhật "+film.Name+": "+name);
        }

        public ActionResult ChangeImage(int id = 0, string action="update", string url="")
        {
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            switch (action)
            {
                case "insert":
                {
                    film.ImageUrl = url;
                    break;
                }
                case "update":
                {
                    film.ImageUrl = url;
                    break;
                }
                case "delete":
                {
                    film.ImageUrl = "";
                    break;
                }
            }
            db.Entry(film).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Hình ảnh của '" + film.Name + "' đã "+ action + " thành công!");
        }

        //
        // GET: /Admin/Film/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        //
        // POST: /Admin/Film/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      

        public ActionResult DeleteAll()
        {
            foreach (var film in db.Films)
            {
                db.Films.Remove(film);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}