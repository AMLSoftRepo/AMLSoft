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
    public class CatFrecuenciaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatFrecuenciaBlo _catFrecuenciaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatFrecuenciaController(ICatFrecuenciaBlo catFrecuenciaBlo)
        {
            _catFrecuenciaBlo = catFrecuenciaBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCatFrecuencia(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catFrecuenciaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_FRECUENCIA data)
        {
            MAT_CAT_FRECUENCIA catFrecuencia = new MAT_CAT_FRECUENCIA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catFrecuenciaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catFrecuencia = _catFrecuenciaBlo.GetById(data.ID);

                catFrecuencia.DESCRIPCION = data.DESCRIPCION;
                catFrecuencia.VALOR = data.VALOR;

                _catFrecuenciaBlo.Save(catFrecuencia);
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
                _catFrecuenciaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catFrecuenciaBlo.Remove(id);
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