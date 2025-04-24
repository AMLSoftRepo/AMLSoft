using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Matriz;
using Blo.Properties;


namespace View.Controllers.Matriz
{
    [NoCache]
    [Autorizacion]
    public class CatUnidadController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatUnidadBlo _catUnidadBlo;
        private readonly ICatAgenciaBlo _catAgenciaBlo;
        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatUnidadController(ICatUnidadBlo catUnidadBlo, ICatAgenciaBlo catAgenciaBlo)
        {
            _catUnidadBlo = catUnidadBlo;
            _catAgenciaBlo = catAgenciaBlo;
        }

        public ActionResult Index()
        {
            ViewBag.agencia = _catAgenciaBlo.GetAll().Where(x => x.ID != 0).ToList();
 

            return View();
        }


        [HttpGet]
        public JsonResult GetCatUnidad(int? page, int? limit, string sortBy, string direction, string searchString = null, long ID_AGENCIA = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                //var records = _catUnidadBlo.GetAllPagina(out total).AsQueryable();
                var records = _catUnidadBlo.GetAll(true)
                  
                    .Select(u => new 
                    {
                        u.ID,
                        u.ID_AGENCIA,
                        AGENCIA = u.MAT_CAT_AGENCIA.NOMBRE,
                        u.DESCRIPCION 
                    })
                    .AsQueryable();
              
                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CAT_UNIDAD data)
        {
            MAT_CAT_UNIDAD unidad = new MAT_CAT_UNIDAD();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catUnidadBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    unidad = _catUnidadBlo.GetById(data.ID);

                unidad.ID_AGENCIA = data.ID_AGENCIA;
                unidad.DESCRIPCION = data.DESCRIPCION;

                _catUnidadBlo.Save(unidad);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catUnidadBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catUnidadBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }






    }
}