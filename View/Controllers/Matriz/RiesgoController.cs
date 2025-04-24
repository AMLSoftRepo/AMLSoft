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
    public class RiesgoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IRiesgoBlo _riesgoBlo;
        private readonly ICatTipoRiesgoBlo _tipoRiesgoBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RiesgoController(IRiesgoBlo riesgoBlo, ICatTipoRiesgoBlo tipoRiesgoBlo)
        {
            _riesgoBlo = riesgoBlo;
            _tipoRiesgoBlo = tipoRiesgoBlo;
        }


        public ActionResult Index()
        {
            ViewBag.tipoRiesgo = _tipoRiesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetRiesgo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _riesgoBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.DESCRIPCION,
                                 x.ID_TIPO_RIESGO,
                                 DESCTIPORIESGO = x.MAT_CAT_TIPO_RIESGO.DESCRIPCION
                             });


                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_RIESGO data)
        {
            MAT_RIESGO riesgo = new MAT_RIESGO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _riesgoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    riesgo = _riesgoBlo.GetById(data.ID);

                riesgo.ID_TIPO_RIESGO = data.ID_TIPO_RIESGO;
                riesgo.DESCRIPCION = data.DESCRIPCION;

                _riesgoBlo.Save(riesgo);
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
                _riesgoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _riesgoBlo.Remove(id);
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