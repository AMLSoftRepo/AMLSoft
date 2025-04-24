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
    public class CausaRiesgoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICausaRiesgoBlo _causaRiesgoBlo;
        private readonly ICatTipoRiesgoBlo _tipoRiesgoBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CausaRiesgoController(ICausaRiesgoBlo causaRiesgoBlo, ICatTipoRiesgoBlo tipoRiesgoBlo)
        {
            _causaRiesgoBlo = causaRiesgoBlo;
            _tipoRiesgoBlo = tipoRiesgoBlo;
        }


        public ActionResult Index()
        {
            ViewBag.tipoRiesgo = _tipoRiesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetCausaRiesgo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _causaRiesgoBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
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
        public JsonResult Save(MAT_CAUSA_RIESGO data)
        {
            MAT_CAUSA_RIESGO causaRiesgo = new MAT_CAUSA_RIESGO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _causaRiesgoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    causaRiesgo = _causaRiesgoBlo.GetById(data.ID);

                causaRiesgo.ID_TIPO_RIESGO = data.ID_TIPO_RIESGO;
                causaRiesgo.DESCRIPCION = data.DESCRIPCION;

                _causaRiesgoBlo.Save(causaRiesgo);
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
                _causaRiesgoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _causaRiesgoBlo.Remove(id);
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