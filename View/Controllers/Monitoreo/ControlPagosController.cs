using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Alertas;
using Blo.Monitoreo;
using Blo.Properties;
using Model;
using System.Data.Entity.Core.Objects;

namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class ControlPagosController : BaseControlPagosController
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
        public ControlPagosController
            (IAlertaBlo alertaBlo, IDatosAdicionalesTransaccionBlo datosAdicionalesBlo, ITipoAlertaBlo tipoAlertaBlo,
            IContactoAlertaBlo contactoAlertaBlo, IBloqueoBlo bloqueoBlo, IAgenciaBancariaBlo agenciaBancariaBlo)
            : base(alertaBlo, datosAdicionalesBlo, tipoAlertaBlo, contactoAlertaBlo, bloqueoBlo,agenciaBancariaBlo)
        {
            _alertaBlo = alertaBlo;
            _datosAdicionalesTransaccionBlo = datosAdicionalesBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetAlertas(int? page, int? limit, string prestamo, string fechaInicial, string fechaFinal,
            Nullable<decimal> montoMinimo, Nullable<decimal> montoMaximo, string estado)
        {
            try
            {
                int total = 0;
                List<SP_CONTROL_PAGOS_Result> records = new List<SP_CONTROL_PAGOS_Result>();
                string rangoFechas = "";
                string rangoMonto = "";
                string condicionSQL = "";
                string estados = "";

                //preparando condición para filtrar por estados de la transacción
                if (estado == "TODOS") estados = "";
                else if (estado == "DOCUMENTADO") estados = " AND B.ESTADO IS NULL";
                else if (estado == "PENDIENTE") estados = " AND B.ESTADO = 'PENDIENTE'";

                //Preparando condición para cuando contenga el número de prestamo
                prestamo = string.IsNullOrEmpty(prestamo) ? "" : "T.NUMERO_PRODUCTO = '" + prestamo.Trim() + "'";


                //preparando condición para cuando contenga el rango de fechas
                if (!string.IsNullOrEmpty(fechaInicial) && !string.IsNullOrEmpty(fechaFinal))
                    rangoFechas = "CAST(T.FECHA_CALENDARIO AS DATE) BETWEEN CAST('" + fechaInicial.Trim() + "'AS DATE) AND CAST('" + fechaFinal.Trim() + "'AS DATE)";


                //preparando condición para cuando sea un rango de montos
                if (montoMinimo != null && montoMaximo != null)
                    rangoMonto = "T.VALOR_TRANSACCION >=" + montoMinimo + " AND T.VALOR_TRANSACCION <= " + montoMaximo;
                else if (montoMinimo != null && montoMaximo == null)
                    rangoMonto = "T.VALOR_TRANSACCION >=" + montoMinimo;
                else if (montoMinimo == null && montoMaximo != null)
                    rangoMonto = "T.VALOR_TRANSACCION <= " + montoMaximo;


                //Solo existen 4 casos 
                //1- contien el rango de fechas, el número de prestamo y el rango de montos
                //2- contiene el rango de fechas y el rango de montos
                //3- contien el rango de fechas y el número de prestamo
                //4- solo contiene el rango de fechas
                if (!string.IsNullOrEmpty(rangoFechas) && !string.IsNullOrEmpty(prestamo) && !string.IsNullOrEmpty(rangoMonto))
                    condicionSQL = "T.CLASE_PRODUCTO = 'PA' AND " + rangoFechas + " AND " + prestamo + " AND " + rangoMonto + estados;
                else if (!string.IsNullOrEmpty(rangoFechas) && !string.IsNullOrEmpty(rangoMonto))
                    condicionSQL = "T.CLASE_PRODUCTO = 'PA' AND " + rangoFechas + " AND " + rangoMonto + estados;
                else if (!string.IsNullOrEmpty(rangoFechas) && !string.IsNullOrEmpty(prestamo))
                    condicionSQL = "T.CLASE_PRODUCTO = 'PA' AND " + rangoFechas + " AND " + prestamo + estados;
                else if (!string.IsNullOrEmpty(rangoFechas))
                    condicionSQL = "T.CLASE_PRODUCTO = 'PA' AND " + rangoFechas + estados;


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
