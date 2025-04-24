using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Alertas;
using Blo.Properties;
using Model;
using System.Data.Entity.Core.Objects;

namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class PrestamosBloqueadosController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAlertaBlo _alertaBlo;
        private readonly IDatosAdicionalesTransaccionBlo _datosAdicionalesTransaccionBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;
        private readonly IBloqueoBlo _bloqueoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PrestamosBloqueadosController(IAlertaBlo alertaBlo, IDatosAdicionalesTransaccionBlo datosAdicionalesBlo, 
            ITipoAlertaBlo tipoAlertaBlo, IBloqueoBlo bloqueoBlo)
        {
            _alertaBlo = alertaBlo;
            _datosAdicionalesTransaccionBlo = datosAdicionalesBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
            _bloqueoBlo = bloqueoBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetAlertas(int? page, int? limit, string sortBy, string direction, string prestamo)
        {
            try
            {
                int total = 0;
                List<SP_CONTROL_PAGOS_Result> records = new List<SP_CONTROL_PAGOS_Result>();
                string condicionSQL = "";

                //Preparando condición para cuando contenga el número de prestamo
                condicionSQL = string.IsNullOrEmpty(prestamo) ? "" : "T.CLASE_PRODUCTO = 'PA' AND T.NUMERO_PRODUCTO = '" + prestamo.Trim() + "' AND B.ESTADO = 'PENDIENTE'";


                //Paramero de entrada y salida dentro del SP
                ObjectParameter pTotal = new ObjectParameter("total", 0);
                //Procedimiento que permite generar una condición dinamica y además permite paginar desde el servidor de SQL
                records = _SQLBDEntities.SP_CONTROL_PAGOS(condicionSQL, pTotal, page.Value, limit.Value).ToList();
                total = Convert.ToInt32(pTotal.Value);


                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

    }
}