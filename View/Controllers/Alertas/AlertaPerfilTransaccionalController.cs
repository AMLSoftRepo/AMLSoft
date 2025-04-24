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
    public class AlertaPerfilTransaccionalController : BaseController
    {

        private readonly IAlertaPerfilTransaccionalBlo _alertaPerfilTransaccionalBlo;


        public AlertaPerfilTransaccionalController(IAlertaPerfilTransaccionalBlo alertaPerfilTransaccionalBlo)
        {
            _alertaPerfilTransaccionalBlo = alertaPerfilTransaccionalBlo;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAlertaPerfilTransaccional(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _alertaPerfilTransaccionalBlo.GetAlertaPerfilTransaccional(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult NotificarPerfilTransaccional()
        {
            int cantidad = 0;

            try
            {
                cantidad = _alertaPerfilTransaccionalBlo.NotificarPerfilTransaccional();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener cantidad de Alertas de Perfil Transaccional");
            }

            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _alertaPerfilTransaccionalBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _alertaPerfilTransaccionalBlo.Remove(id);
               
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