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
    public class NotificacionAlertaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly INotificacionAlertaBlo _notificacionAlertaBlo;
        private readonly IContactoAlertaBlo _contactoAlertaBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public NotificacionAlertaController(INotificacionAlertaBlo notificacionAlertaBlo, IContactoAlertaBlo contactoAlertaBlo, ITipoAlertaBlo tipoAlertaBlo)
        {
            _notificacionAlertaBlo = notificacionAlertaBlo;
            _contactoAlertaBlo = contactoAlertaBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
        }

        public ActionResult Index()
        {
            ViewBag.contactos = _contactoAlertaBlo.GetAll();
            ViewBag.alertas = _tipoAlertaBlo.GetAll();

            return View();
        }

        [HttpGet]
        public JsonResult GetNotificacionAlerta(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                var queryAlert = _notificacionAlertaBlo.GetAll(true).Select(x => new
                {
                    x.ID,
                    x.ID_CONTACTO,
                    x.ID_TIPO_ALERTA,
                    x.ALE_TIPO_ALERTA.DESCRIPCION
                }).ToList();

                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _contactoAlertaBlo.GetAll(true)
                    .Where(x => _notificacionAlertaBlo.GetAll().Where(a => a.ID_CONTACTO == x.ID).Any())
                    .Select(x => new
                    {
                        x.ID,
                        x.NOMBRE,
                        IDALERTA = _notificacionAlertaBlo.GetAll().Where(y => y.ID_CONTACTO == x.ID).Select(y => y.ID_TIPO_ALERTA),
                        DESCALERTA = string.Join(",", queryAlert.Where(z => z.ID_CONTACTO == x.ID).Select(z => z.DESCRIPCION))
                    }).AsQueryable();

                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(int ID_CONTACTO, int[] ID_TIPO_ALERTA)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _notificacionAlertaBlo.ValidarSave(ID_CONTACTO);
                _notificacionAlertaBlo.SaveNotificacionContacto(ID_CONTACTO, ID_TIPO_ALERTA);
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
                _notificacionAlertaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _notificacionAlertaBlo.DeleteNotificacionContacto(id);
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