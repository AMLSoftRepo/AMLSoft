using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Perfiles;
using Blo.Properties;

namespace View.Controllers.Perfiles
{
    [NoCache]
    [Autorizacion]
    public class TipoCalificacionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ITipoCalificacionBlo _tipoCalificacionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public TipoCalificacionController(ITipoCalificacionBlo tipoCalificacionBlo)
        {
            _tipoCalificacionBlo = tipoCalificacionBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTipoCalificacion(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _tipoCalificacionBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(PER_TIPO_CALIFICACION data)
        {
            PER_TIPO_CALIFICACION tipoCalificacion = new PER_TIPO_CALIFICACION();
            string mensaje = PropertiesBlo.msgExito;
            string validarRango = null;
            try
            {
                _tipoCalificacionBlo.ValidarSave(data.ID);
                validarRango = _tipoCalificacionBlo.validaRangoMinMax(data.ID, data.VALORMIN, data.VALORMAX);
                if (!string.IsNullOrEmpty(validarRango))
                    mensaje = "El rango de valores entra en conflicto con Calificación: " + validarRango;

                if (data.ID != 0)
                    tipoCalificacion = _tipoCalificacionBlo.GetById(data.ID);

                tipoCalificacion.DESCRIPCION = data.DESCRIPCION;
                tipoCalificacion.VALORMIN = data.VALORMIN;
                tipoCalificacion.VALORMAX = data.VALORMAX;
                tipoCalificacion.COLOR = data.COLOR;

                _tipoCalificacionBlo.Save(tipoCalificacion);
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
                _tipoCalificacionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _tipoCalificacionBlo.Remove(id);
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