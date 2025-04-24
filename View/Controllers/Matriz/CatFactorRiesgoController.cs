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
    public class CatFactorRiesgoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatFactorRiesgoBlo _catFactorRiesgoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatFactorRiesgoController(ICatFactorRiesgoBlo catFactorRiesgoBlo)
        {
            _catFactorRiesgoBlo = catFactorRiesgoBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetFactorRiesgo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catFactorRiesgoBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_FACTOR_RIESGO data)
        {
            MAT_CAT_FACTOR_RIESGO factorRiesgo = new MAT_CAT_FACTOR_RIESGO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catFactorRiesgoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    factorRiesgo = _catFactorRiesgoBlo.GetById(data.ID);

                factorRiesgo.DESCRIPCION = data.DESCRIPCION;

                _catFactorRiesgoBlo.Save(factorRiesgo);
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
                _catFactorRiesgoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catFactorRiesgoBlo.Remove(id);
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