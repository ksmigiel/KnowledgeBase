using System.Linq;
using System.Web.Mvc;

namespace BazaWiedzy.Controllers
{
    public class HomeController : Controller
    {
        private NpoziomDBEntities db = new NpoziomDBEntities();

        public ViewResult Index()
        {
            var grupy = db.Grupy;
            return View(grupy.ToList());
        }

        /// <summary>
        /// Returns categories assigned to the group.
        /// </summary>
        /// <param name="id">Group Id</param>
        public ViewResult Kategorie(int id)
        {
            var kategorie = db.Kategorie.Where(i => i.IdG == id);
            return View(kategorie.ToList());
        }

        /// <summary>
        /// Returns instruments assigned to the category.
        /// </summary>
        /// <param name="id">Category Id</param>
        public ViewResult Instrumenty(int id)
        {
            var instrumenty = db.Instrumenty.Where(i => i.KategoriaID == id);
            return View(instrumenty.ToList());
        }

        /// <summary>
        /// Prepares available relations for specific instrument.
        /// </summary>
        /// <param name="id">Instrument Id</param>
        public ViewResult Wiedza(int id)
        {
            var relacje = (from r in db.Wiedza
                           where r.IdI == id
                           select r.Relacje).Distinct();
            ViewBag.RelacjeId = new SelectList(relacje, "Id", "Relacja");
            
            var wiedza = db.Instrumenty.Single(i => i.Id == id);
            // var wiedza = db.Wiedza.Include("Instrumenty").Include("Relacje").Where(i => i.IdI == id);
            return View(wiedza);
        }

        /// <summary>
        /// Helper for supporting AJAX in dropdown.
        /// </summary>
        /// <param name="idI">Instrument Id</param>
        /// <param name="idR">Relation Id</param>
        public ActionResult WiedzaAJAXResult(int idI, int idR)
        {
            var result = from w in db.Wiedza
                         where w.IdI == idI
                         where w.IdR == idR
                         select w.Koncept;
                         
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepares knowledge for specific concept.
        /// </summary>
        /// <param name="koncept">Logical concept</param>
        public ViewResult Koncept(string koncept)
        {
            ViewBag.Koncept = koncept;
            var koncepts = db.Wiedza.Include("Instrumenty").Include("Relacje").Where(i => i.Koncept == koncept);
            return View(koncepts.ToList());
        }
    }
}