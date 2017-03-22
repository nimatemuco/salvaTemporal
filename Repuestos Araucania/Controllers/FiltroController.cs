using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repuestos_Araucania.Models.Productos;
using System.Text;

namespace Repuestos_Araucania.Controllers
{
    public class FiltroController : Controller
    {
        public static int page = 0;
        private repuestosEntities db = new repuestosEntities();

        [HttpGet]
        // GET: Filtro
        public async Task<ActionResult> Index()
        {
            page = 0;
            return View(await db.productos.Take(20).ToListAsync());
        }

        // POST: Filtro
        [HttpPost]

        public string Index(IEnumerable<productos> prod)
        {
            if (prod.Count(x => x.MARCAR) == 0)
            {
                return "No ha seleccionado ningún producto!";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Has seleccionado: ");
                foreach (productos p in prod)
                {
                    if (p.MARCAR)
                    {
                        sb.Append(p.NOMBRE);
                    }
                }
                return sb.ToString();
            }
        }

        // GET: Filtro/Details/5
        public async Task<ActionResult> Details(double? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productos productos = await db.productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }       
        public ActionResult BuscarNombre(String NOMBRE)
        {
            ViewBag.Id = new SelectList(db.productos, "NOMBRE");
            var proveider = from s in db.productos select s;
            if (!String.IsNullOrEmpty(NOMBRE))
            {
                proveider = proveider.Where(j => j.NOMBRE.Contains(NOMBRE));
            }
            return View(proveider.Take(20).ToList());
        }
        public ActionResult BuscarCódigo(String CÓDIGO)
        {
            ViewBag.Id = new SelectList(db.productos, "CÓDIGO");
            var proveider = from s in db.productos select s;
            if (!String.IsNullOrEmpty(CÓDIGO))
            {
                proveider = proveider.Where(j => j.CÓDIGO.Contains(CÓDIGO));
            }
            return View(proveider.Take(20).ToList());
        }          
        public ActionResult Avanza()
        {
            if (page == 0) { page = 1; }
            if (page < (db.productos.Count()) / 20) { page++; } else { page = ((db.productos.Count()) / 20) + 1; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList());
        }
        public ActionResult Retrocede()
        {
            if (page == 2) { return RedirectToAction("Index", "Filtro"); }
            if (page > 1) { page--; } else { page = 1; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList());
        }
        public ActionResult Volver()
        {
            if (page == 0) { return RedirectToAction("Index", "Filtro"); }
            if (page == 1) { return RedirectToAction("Index", "Filtro"); }
            if (page > 1) {  } else { page = 1; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page -1 ) * 20).Take(20).ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
