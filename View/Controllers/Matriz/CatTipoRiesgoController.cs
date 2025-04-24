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
    public class CatTipoRiesgoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatTipoRiesgoBlo _catTipoRiesgoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatTipoRiesgoController(ICatTipoRiesgoBlo catTipoRiesgoBlo)
        {
            _catTipoRiesgoBlo = catTipoRiesgoBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCatTipoRiesgo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catTipoRiesgoBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_TIPO_RIESGO data)
        {
            MAT_CAT_TIPO_RIESGO catTipoRiesgo = new MAT_CAT_TIPO_RIESGO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catTipoRiesgoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catTipoRiesgo = _catTipoRiesgoBlo.GetById(data.ID);

                catTipoRiesgo.DESCRIPCION = data.DESCRIPCION;

                _catTipoRiesgoBlo.Save(catTipoRiesgo);
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
                _catTipoRiesgoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catTipoRiesgoBlo.Remove(id);
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