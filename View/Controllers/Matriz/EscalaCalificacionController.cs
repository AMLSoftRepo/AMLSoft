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
    public class EscalaCalificacionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IEscalaCalificacionBlo _escalaCalificacionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public EscalaCalificacionController(IEscalaCalificacionBlo escalaCalificacionBlo)
        {
            _escalaCalificacionBlo = escalaCalificacionBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEscalaCalificacion(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total=0;
                var records = _escalaCalificacionBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_ESCALA_CALIFICACION data)
        {
            MAT_ESCALA_CALIFICACION escalaCalificacion = new MAT_ESCALA_CALIFICACION();
            string mensaje = PropertiesBlo.msgExito;
            string validarRango = null;
            try
            {
                _escalaCalificacionBlo.ValidarSave(data.ID);
                validarRango = _escalaCalificacionBlo.validaRangoMinMax(data.ID, data.VALORMIN, data.VALORMAX);
                if (!string.IsNullOrEmpty(validarRango))
                    mensaje = "El rango de valores entra en conflicto con calificación : " + validarRango;


                if (data.ID != 0)
                    escalaCalificacion = _escalaCalificacionBlo.GetById(data.ID);

                escalaCalificacion.DESCRIPCION = data.DESCRIPCION;
                escalaCalificacion.VALORMIN = data.VALORMIN;
                escalaCalificacion.VALORMAX = data.VALORMAX;
                escalaCalificacion.COLOR = data.COLOR;

                _escalaCalificacionBlo.Save(escalaCalificacion);
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
                _escalaCalificacionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _escalaCalificacionBlo.Remove(id);
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