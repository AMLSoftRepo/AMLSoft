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
    public class CatSeveridadController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatSeveridadBlo _catSeveridadBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatSeveridadController(ICatSeveridadBlo catSeveridadBlo)
        {
            _catSeveridadBlo = catSeveridadBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCatSeveridad(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catSeveridadBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_SEVERIDAD data)
        {
            MAT_CAT_SEVERIDAD catSeveridad = new MAT_CAT_SEVERIDAD();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catSeveridadBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catSeveridad = _catSeveridadBlo.GetById(data.ID);

                catSeveridad.DESCRIPCION = data.DESCRIPCION;
                catSeveridad.NIVEL = data.NIVEL;

                _catSeveridadBlo.Save(catSeveridad);
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
                _catSeveridadBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catSeveridadBlo.Remove(id);
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