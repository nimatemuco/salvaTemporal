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
using System.Data.SqlClient;

namespace Repuestos_Araucania.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        public static int page = 0;

        private repuestosEntities db = new repuestosEntities();

        [HttpGet]
        // GET: Productos
        public async Task<ActionResult> Index()
        {
            page = 0;
            return View(await db.productos.Take(20).ToListAsync());
        }

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

        // GET: Productos/Details/5
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

        // GET: Productos/Create
        public ActionResult Create()
        {
            SqlConnection cn = new SqlConnection("data source=winalfa.host.cl;initial catalog=repuestos;user id=repuestos;password=eaz629$J;MultipleActiveResultSets=True;App=EntityFramework&quot;");
            cn.Open();
            productos code = new productos();

            SqlCommand cmd = new SqlCommand("SELECT NEXT VALUE FOR MySequence", cn);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                code.CÓDIGO = Convert.ToString(dr[0]);
            }

            dr.Close();

            if (dr.IsClosed)
            {
                cmd = new SqlCommand("SELECT NEXT VALUE FOR Id_Sequence", cn);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    code.ID = Convert.ToInt32(dr[0]);
                }
            }

            code.PRECIO = 0;
            code.STOCK = 0;

            return View(code);
        }
        public ActionResult Avanza()
        {
            if (page == 0) { page = 1; }
            if (page < 131) { page++; } else { page = 131; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList());
        }

        public ActionResult Retrocede()
        {
            if (page == 2) { return RedirectToAction("Index", "Productos"); }
            if (page > 1) { page--; } else { page = 1; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList());
        }
        public ActionResult Volver()
        {
            if (page == 0) { return RedirectToAction("Index", "Productos"); }
            if (page == 1) { return RedirectToAction("Index", "Productos"); }
            if (page > 1) { } else { page = 1; }
            return View(db.productos.OrderBy(a => a.ID).Skip((page - 1) * 20).Take(20).ToList());
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CÓDIGO,NOMBRE,PRECIO,STOCK")] productos productos)
        {
            if (ModelState.IsValid)
            {
                db.productos.Add(productos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        // GET: Productos/Edit/5
        public async Task<ActionResult> Edit(double? id)
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

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CÓDIGO,NOMBRE,PRECIO,STOCK")] productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productos);
        }

        // GET: Productos/Delete/5
        public async Task<ActionResult> Delete(double? id)
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

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(double id)
        {
            productos productos = await db.productos.FindAsync(id);
            db.productos.Remove(productos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult BuscarNombre(String NOMBRE)
        {
            ViewBag.Id = new SelectList(db.productos, "NOMBRE");
            var proveider = from s in db.productos select s;
            if (!String.IsNullOrEmpty(NOMBRE))
            {
                proveider = proveider.Where(j => j.NOMBRE.Contains(NOMBRE));
            }
            return View(proveider.Take(20));
        }
        public ActionResult BuscarCódigo(String CÓDIGO)
        {
            ViewBag.Id = new SelectList(db.productos, "CÓDIGO");
            var proveider = from s in db.productos select s;
            if (!String.IsNullOrEmpty(CÓDIGO))
            {
                proveider = proveider.Where(j => j.CÓDIGO.Contains(CÓDIGO));
            }
            return View(proveider.Take(20));
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
