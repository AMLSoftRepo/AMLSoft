using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Listas;
using Blo.Properties;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]
    public class ParametroONUSDNController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IParametroONUSDNBlo _parametroONUSDNBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ParametroONUSDNController(IParametroONUSDNBlo parametroONUSDNBlo)
        {
            _parametroONUSDNBlo = parametroONUSDNBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetParametroONUSDN(int? page, int? limit, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _parametroONUSDNBlo.GetDatosGrid(out total, page, limit, null, null);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(LIS_PARAMETRO_ONU_SDN data)
        {
            LIS_PARAMETRO_ONU_SDN parametroONUSDN = new LIS_PARAMETRO_ONU_SDN();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _parametroONUSDNBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    parametroONUSDN = _parametroONUSDNBlo.GetById(data.ID);

                parametroONUSDN.TIPO = data.TIPO;
                parametroONUSDN.URL_XML = data.URL_XML;
                parametroONUSDN.DESCRIPCION = data.DESCRIPCION;
                parametroONUSDN.URL_DESCRIPCION = data.URL_DESCRIPCION;

                _parametroONUSDNBlo.Save(parametroONUSDN);
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
                _parametroONUSDNBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _parametroONUSDNBlo.Remove(id);
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