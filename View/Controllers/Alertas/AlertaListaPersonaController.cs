using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Alertas;
using Blo.Perfiles;
using Blo.Properties;

namespace View.Controllers.Alertas
{
    [NoCache]
    [Autorizacion]
    public class AlertaListaPersonaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAlertaListaPersonaBlo _alertaListaPersonaBlo;
        private readonly ICoincidenciaListaBlo _coincidenciaListaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public AlertaListaPersonaController(IAlertaListaPersonaBlo alertaListaPersonaBlo, ICoincidenciaListaBlo coincidenciaListaBlo)
        {
            _alertaListaPersonaBlo = alertaListaPersonaBlo;
            _coincidenciaListaBlo = coincidenciaListaBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAlertaListaPersona(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _alertaListaPersonaBlo.GetAlertaListaPersona(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de personas con coincidencias en las listas
        /// </summary>
        /// <returns>int cantidad de personas con coincidencias en listas</returns>
        [HttpPost]
        public JsonResult NotificarListas()
        {
            int cantidad = 0;

            try
            {
                cantidad = _alertaListaPersonaBlo.NotificarListas();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener cantidad de personas en listas");
            }

            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| VISTAS HIJAS

        public ActionResult _Cliente(int CODIGO_CLIENTE = 0)
        {
            int edad = 0;
            var record = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                .ToList();

            if (record.Any())
                if (record.FirstOrDefault().FECHA_DE_NACIMIENTO != null)
                    edad = DateTime.Today.AddTicks(-record.FirstOrDefault().FECHA_DE_NACIMIENTO.Value.Ticks).Year - 1;

            ViewBag.edad = edad;
            ViewBag.cliente = record;

            return PartialView("_Cliente");
        }


        [HttpGet]
        public JsonResult GetCoincidencias(int? page, int? limit, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_COINCIDENCIA_LISTA.Where(x => x.ID_CLIENTE == CODIGO_CLIENTE).ToList();

                total = records.Count();
                records = records.Skip(start).Take(limit.Value).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteCreditos");
            }
        }



        [HttpPost]
        public JsonResult SeguimientoCoincidencia(int ID, string SEGUIMIENTO)
        {
            PER_COINCIDENCIA_LISTA coincidencia = new PER_COINCIDENCIA_LISTA();
            string mensaje = PropertiesBlo.msgExito;
            String usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            try
            {
                _coincidenciaListaBlo.ValidarSave(ID);

                if (ID != 0)
                {
                    if (usuarioIdentity.Contains("\\"))
                        usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

                    coincidencia = _coincidenciaListaBlo.GetById(ID);

                    coincidencia.SEGUIMIENTO = SEGUIMIENTO;
                    coincidencia.FECHA_ACTUALIZACION = DateTime.Now;
                    coincidencia.USUARIO_ACTUALIZA = usuarioIdentity;

                    _coincidenciaListaBlo.Save(coincidencia);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClienteSolicitud(int? page, int? limit, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_SOLICITUDES_CREDITOS.Where(x => x.RESPONSABLE_SOLICITUD == CODIGO_CLIENTE).ToList();

                total = records.Count();
                records = records.Skip(start).Take(limit.Value).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteCreditos");
            }
        }


        [HttpGet]
        public JsonResult GetClienteContados(int? page, int? limit, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_VENTAS_CONTADO.AsNoTracking().Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE).ToList();

                total = records.Count();
                records = records.Skip(start).Take(limit.Value).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteContados");
            }
        }


        [HttpGet]
        public JsonResult GetClienteCreditos(int? page, int? limit, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_CREDITOS_ESCRITURADOS.Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE).ToList();

                total = records.Count();
                records = records.Skip(start).Take(limit.Value).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteCreditos");
            }
        }


        #endregion

    }
}