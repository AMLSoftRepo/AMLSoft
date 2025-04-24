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
    public class ContactoAlertaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IContactoAlertaBlo _contactoAlertaBlo;
        private readonly INotificacionAlertaBlo _notificacionAlertaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ContactoAlertaController(IContactoAlertaBlo contactoAlertaBlo, INotificacionAlertaBlo notificacionAlertaBlo)
        {
            _contactoAlertaBlo = contactoAlertaBlo;
            _notificacionAlertaBlo = notificacionAlertaBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContactoAlerta(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _contactoAlertaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(ALE_CONTACTO_ALERTA data)
        {
            ALE_CONTACTO_ALERTA contactoAlerta = new ALE_CONTACTO_ALERTA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _contactoAlertaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    contactoAlerta = _contactoAlertaBlo.GetById(data.ID);

                contactoAlerta.CODIGO = data.CODIGO;
                contactoAlerta.NOMBRE = data.NOMBRE;
                contactoAlerta.CORREO = data.CORREO;

                _contactoAlertaBlo.Save(contactoAlerta);
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
                _contactoAlertaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _contactoAlertaBlo.Remove(id);
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