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
        repuestosEntities db = new repuestosEntities();
        ProductosList prolList = new ProductosList();

        [HttpGet]
        // GET: Filtro
        public ActionResult Index()
        {
            page = 0; 
            prolList.product = db.productos.Take(20).ToList();            
            return View(prolList);
        }

        // POST: Filtro
        [HttpPost] 
        public ActionResult Index(ProductosList frl)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in frl.product)
            {
                if (item.MARCAR)
                {
                    sb.Append(item.NOMBRE + ",");
                }
            }
            ViewBag.selectfruits = "Frutas Seleccionadas: " + sb.ToString();
            return View(frl);
        }        
        public ActionResult Seleccion(ProductosList frl)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in frl.product)
            {
                if (item.MARCAR)
                {
                    sb.Append(item.NOMBRE + ",");
                }
            }
            ViewBag.selectfruits = "Frutas Seleccionadas: " + sb.ToString();
            return View(frl);
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
                prolList.product = db.productos.Where(j => j.NOMBRE.Contains(NOMBRE)).ToList();
            }
            return View(prolList);
        }
        public ActionResult BuscarCódigo(String CÓDIGO)
        {
            ViewBag.Id = new SelectList(db.productos, "CÓDIGO");
            var proveider = from s in db.productos select s;
            if (!String.IsNullOrEmpty(CÓDIGO))
            {
                prolList.product = db.productos.Where(j => j.CÓDIGO.Contains(CÓDIGO)).ToList();
            }            
            return View(prolList);
        }          
        public ActionResult Avanza()
        {            
            if (page == 0) { page = 1; }
            if (page < (db.productos.Count()) / 20) { page++; } else { page = ((db.productos.Count()) / 20) + 1; }
            prolList.product = db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList();
            return View(prolList);            
        }
        public ActionResult Retrocede()
        {
            if (page == 2) { return RedirectToAction("Index", "Filtro"); }
            if (page > 1) { page--; } else { page = 1; }
            prolList.product = db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList();
            return View(prolList);
        }
        public ActionResult Volver()
        {
            if (page == 0 || page == 1) { return RedirectToAction("Index", "Filtro"); }
            if (page > 1) {  } else { page = 1; }
            prolList.product = db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList();
            return View(prolList);
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
