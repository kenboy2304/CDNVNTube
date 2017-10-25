using System.Data.Entity;
using System.Web.Mvc;
using CDNVNCMS.Tube.Areas.Admin.Models;
using CDNVNCMS.Tube.Entities;

namespace CDNVNCMS.Tube.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class SettingsController : Controller
    {
        //
        // GET: /Admin/Settings/
        private readonly TubeContext db = new TubeContext();

        public ActionResult Index()
        {
           var settings = GenericModel.GetGeneric<SettingModels>(db.Settings);
            return View(settings);
        }
        [HttpPost]
        public ActionResult Index(SettingModels setting)
        {
            if(ModelState.IsValid)
            {
                var listSetting = GenericModel.SetGeneric(setting,db.Settings);

                foreach (var s in listSetting)
                {
                    if(s.Id==0)
                    {
                        db.Settings.Add(s);
                    }
                    else
                    {
                        db.Entry(s).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                
            }
            return View(setting);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
