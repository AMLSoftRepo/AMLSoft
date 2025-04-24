using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Alertas;
using Blo.Properties;


namespace View.Controllers.Alertas
{
    [NoCache]
    [Autorizacion]
    public class TipoAlertaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ITipoAlertaBlo _tipoAlertaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public TipoAlertaController(ITipoAlertaBlo tipoAlertaBlo)
        {
            _tipoAlertaBlo = tipoAlertaBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTipoAlerta(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total=0;
                var records = _tipoAlertaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(ALE_TIPO_ALERTA data)
        {
            ALE_TIPO_ALERTA tipoAlerta = new ALE_TIPO_ALERTA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _tipoAlertaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    tipoAlerta = _tipoAlertaBlo.GetById(data.ID);

                tipoAlerta.DESCRIPCION = data.DESCRIPCION;
                tipoAlerta.NOTIFICAR = data.NOTIFICAR;
                tipoAlerta.REPORTE = data.REPORTE;
                tipoAlerta.COLOR = data.COLOR;

                _tipoAlertaBlo.Save(tipoAlerta);
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
                _tipoAlertaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _tipoAlertaBlo.Remove(id);
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