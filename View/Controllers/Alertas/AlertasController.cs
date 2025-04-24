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
    public class AlertasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAlertaBlo _alertaBlo;
        private readonly IDatosAdicionalesTransaccionBlo _datosAdicionalesTransaccionBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public AlertasController(IAlertaBlo alertaBlo, IDatosAdicionalesTransaccionBlo datosAdicionalesBlo, ITipoAlertaBlo tipoAlertaBlo)
        {
            _alertaBlo = alertaBlo;
            _datosAdicionalesTransaccionBlo = datosAdicionalesBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
        }


        public ActionResult Notificada()
        {
            ViewBag.alertas = _tipoAlertaBlo.GetAll();
            return View();
        }


        public ActionResult Analizada()
        {
            ViewBag.alertas = _tipoAlertaBlo.GetAll();
            return View();
        }


        public ActionResult Historial()
        {
            ViewBag.alertas = _tipoAlertaBlo.GetAll();
            return View();
        }


        #region ============================= ALERTA


        [HttpGet]
        public JsonResult GetAlertas(int? page, int? limit, string sortBy, string direction, string estado, string searchString = null, int tipoAlerta = 0, string FECHA_INI_F = null, string FECHA_FIN_F = null)
        {
            try
            {
                int total = 0;
                Nullable<DateTime> fi = null;
                Nullable<DateTime> ff = null;
                if (FECHA_INI_F != null && FECHA_INI_F != "" && FECHA_FIN_F != null && FECHA_FIN_F != "")
                {
                    fi = DateTime.Parse(FECHA_INI_F);
                    ff = DateTime.Parse(FECHA_FIN_F);
                }

                var records = _alertaBlo.GetAlertasNotificadas(out total, page, limit, sortBy, direction, estado, tipoAlerta, searchString, fi, ff);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public JsonResult GetHistorial(int? page, int? limit, string sortBy, string direction, string searchString = null, int tipoAlerta = 0, string FECHA_INI_F = null, string FECHA_FIN_F = null)
        {

            try
            {
                int total = 0;
                Nullable<DateTime> fi = null;
                Nullable<DateTime> ff = null;
                if (FECHA_INI_F != null && FECHA_INI_F != "" && FECHA_FIN_F != null && FECHA_FIN_F != "")
                {
                    fi = DateTime.Parse(FECHA_INI_F);
                    ff = DateTime.Parse(FECHA_FIN_F);
                }

                var records = _alertaBlo.GetAlertasHistorial(out total, page, limit, sortBy, direction, tipoAlerta, searchString, fi, ff);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public JsonResult GetTransaccion(string SECUENCIA)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                    .Where(x => x.SECUENCIA == SECUENCIA)
                    .ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult CambiarEstado(string estado, int ID_ALE_ALERTA)
        {
            ALE_ALERTA alerta = new ALE_ALERTA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                alerta = _alertaBlo.GetById(ID_ALE_ALERTA);

                alerta.ESTADO_ANTERIOR = alerta.ESTADO_ACTUAL;
                alerta.ESTADO_ACTUAL = estado;

                _alertaBlo.Save(alerta);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, alerta.ID }, JsonRequestBehavior.AllowGet);
        }


        //Parcial para mostrar datos del cliente
        public ActionResult _Cliente(long codigoCliente)
        {
            try
            {
                int edad = 0;
                var record = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                    .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                    .ToList();

                if (record.Any())
                    if (record.FirstOrDefault().FECHA_DE_NACIMIENTO != null)
                        edad = DateTime.Today.AddTicks(-record.FirstOrDefault().FECHA_DE_NACIMIENTO.Value.Ticks).Year - 1;

                ViewBag.edad = edad;
                ViewBag.cliente = record;

                return PartialView("_Cliente");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista de datos");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveAnalisisCaso(long ID, string ANALISIS_CASO)
        {
            ALE_ALERTA casoAnalisis = new ALE_ALERTA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _alertaBlo.ValidarSave(ID);

                if (ID != 0)
                { casoAnalisis = _alertaBlo.GetById(ID); }

                casoAnalisis.ANALISIS_CASO = ANALISIS_CASO;

                _alertaBlo.Save(casoAnalisis);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> EMPLEOS

        [HttpPost]
        public ActionResult _Empleos(long CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;

            return PartialView("_Empleos");
        }


        [HttpGet]
        public JsonResult GetEmpleos(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_EMPLEOS.AsNoTracking()
                              .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                              .ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion


        #region ============================= DETALLE PRODUCTOS POR ALERTA

        [HttpGet]
        public JsonResult GetClienteSolicitud(int? page, int? limit, decimal producto)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_SOLICITUDES_CREDITOS.Where(x => x.NRO_SOLICITUD == producto).ToList();

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
        public JsonResult GetClienteContados(int? page, int? limit, decimal producto)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_VENTAS_CONTADO.AsNoTracking().Where(x => x.CODIGO_ACTIVO == producto).ToList();

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
        public JsonResult GetClienteCreditos(int? page, int? limit, string producto)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.VIEW_CREDITOS_ESCRITURADOS.Where(x => x.ID_PRESTAMO == producto).ToList();

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


        #region ============================= NOTIFICACIONES EN PANTALLA

        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas notificadas
        /// </summary>
        /// <returns>int cantidad de alertas notificadas</returns>
        [HttpPost]
        public JsonResult NotificarAlertas()
        {
            int cantidad = 0;

            try
            {
                cantidad = _alertaBlo.NotificarAlertas();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener cantidad de alertas notificadas");
            }

            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas pendientes de analizar
        /// </summary>
        /// <returns>int cantidad de alertas pendientes de analizar</returns>
        [HttpPost]
        public JsonResult NotificarAlertasAnalizar()
        {
            int cantidad = 0;

            try
            {
                cantidad = _alertaBlo.NotificarAlertasAnalizar();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener cantidad de alertas a analizar");
            }

            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ============================= NOTIFICACIONES EN PANTALLA

        [HttpGet]
        public JsonResult GetBitacora(int? page, int? limit, long ID)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _SQLBDEntities.SEG_LOG_AUDITORIA.Where(x => x.ID_ORIGEN == ID && x.TABLA == "ALE_ALERTA").ToList();

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