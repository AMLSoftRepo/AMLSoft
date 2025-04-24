using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Text;
using Blo.Alertas;
using Blo.Matriz;
using Blo.Properties;

namespace View.Controllers.Alertas
{
    [NoCache]
    [Autorizacion]
    public class AlertaManualController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ITipoAlertaBlo _tipoAlertaBlo;
        private readonly IContactoAlertaBlo _contactoAlertaBlo;
        private readonly IAlertaBlo _alertaBlo;
        private readonly ICatAgenciaBlo _catAgenciaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public AlertaManualController(ITipoAlertaBlo tipoAlertaBlo, IContactoAlertaBlo contactoAlertaBlo, IAlertaBlo alertaBlo, ICatAgenciaBlo catAgenciaBlo)
        {
            _tipoAlertaBlo = tipoAlertaBlo;
            _contactoAlertaBlo = contactoAlertaBlo;
            _alertaBlo = alertaBlo;
            _catAgenciaBlo = catAgenciaBlo;
        }


        // GET: AlertaManual
        public ActionResult Index()
        {
            ViewBag.alertas = _tipoAlertaBlo.GetAll();
            ViewBag.contactos = _contactoAlertaBlo.GetAll();
            return View();
        }


        [HttpGet]
        public JsonResult GetTransacciones(int? page, int? limit, string sortBy, string direction, Boolean filtrar = false,
            string fechaAplicacionO = null, string fechaAplicacion = null, string fechaCalendarioO = null, string fechaCalendario = null, string fechaCancelacionO = null, string fechaCancelacion = null,
            string fechaAperturaO = null, string fechaApertura = null, string fechaVencimientoO = null, string fechaVencimiento = null, string fechaSolicitudO = null, string fechaSolicitud = null,

            string valorTransaccionO = null, string valorTransaccion = null, string saldoCapitalO = null, string saldoCapital = null, string saldoInteresO = null, string saldoInteres = null,
            string saldoSegurosIVAO = null, string saldoSegurosIVA = null, string saldoOtrosO = null, string saldoOtros = null, string excedenteO = null, string excedente = null, string capitalVigenteO = null, string capitalVigente = null,
            string capitalMoraO = null, string capitalMora = null, string capitalVencidoO = null, string capitalVencido = null, string interesVigenteO = null, string interesVigente = null, string interesMoraO = null, string interesMora = null,
            string interesVencidoO = null, string interesVencido = null, string valorCuotaO = null, string valorCuota = null, string montoOtorgadoO = null, string montoOtorgado = null, string valorValuoO = null, string valorValuo = null,
            string valorContableO = null, string valorContable = null, string totalIngresosO = null, string totalIngresos = null,

            string claseProducto = null, string codigoCliente = null, string numeroProducto = null, string codigoUsuario = null, string secuencia = null, string codigoFinanciera = null, string colectorMovimiento = null, string tipoTransaccion = null,
            string tipoOperacion = null, string formaPago = null, string codigoAgenciaBanco = null, string numeroRecibo = null, string codigoPatrono = null, string estadoCartera = null, string colectorConvenio = null, string facturaConvenio = null,
            string nombreCompleto = null, string codigoAgencia = null)
        {
            try
            {
                string filtro = "";
                int total;
                int start = (page.Value - 1) * limit.Value;

                if ((fechaAplicacion != null) && (fechaAplicacion != ""))
                    filtro += "FECHA_APLICACION " + fechaAplicacionO + " convert(datetime, '" + fechaAplicacion + "', 103)";
                if ((fechaCalendario != null) && (fechaCalendario != ""))
                    if (filtro != "")
                        filtro += "AND FECHA_CALENDARIO " + fechaCalendarioO + " convert(datetime, '" + fechaCalendario + "', 103)";
                    else
                        filtro += "FECHA_CALENDARIO " + fechaCalendarioO + " convert(datetime, '" + fechaCalendario + "', 103)";
                if ((fechaCancelacion != null) && (fechaCancelacion != ""))
                    if (filtro != "")
                        filtro += "AND FECHA_CANCELACION " + fechaCancelacionO + " convert(datetime, '" + fechaCancelacion + "', 103)";
                    else
                        filtro += "FECHA_CANCELACION " + fechaCancelacionO + " convert(datetime, '" + fechaCancelacion + "', 103)";
                if ((fechaApertura != null) && (fechaApertura != ""))
                    if (filtro != "")
                        filtro += "AND FECHA_APERTURA " + fechaAperturaO + " convert(datetime, '" + fechaApertura + "', 103)";
                    else
                        filtro += "FECHA_APERTURA " + fechaAperturaO + " convert(datetime, '" + fechaApertura + "', 103)";
                if ((fechaVencimiento != null) && (fechaVencimiento != ""))
                    if (filtro != "")
                        filtro += "AND FECHA_VENCIMIENTO " + fechaVencimientoO + " convert(datetime, '" + fechaVencimiento + "', 103)";
                    else
                        filtro += "FECHA_VENCIMIENTO " + fechaVencimientoO + " convert(datetime, '" + fechaVencimiento + "', 103)";
                if ((fechaSolicitud != null) && (fechaSolicitud != ""))
                    if (filtro != "")
                        filtro += "AND FECHA_SOLICITUD " + fechaSolicitudO + " convert(datetime, '" + fechaSolicitud + "', 103)";
                    else
                        filtro += "FECHA_SOLICITUD " + fechaSolicitudO + " convert(datetime, '" + fechaSolicitud + "', 103)";

                if ((valorTransaccion != null) && (valorTransaccion != ""))
                    if (filtro != "")
                        filtro += " AND VALOR_TRANSACCION " + valorTransaccionO + " " + valorTransaccion;
                    else
                        filtro += "VALOR_TRANSACCION " + valorTransaccionO + " " + valorTransaccion;
                if ((saldoCapital != null) && (saldoCapital != ""))
                    if (filtro != "")
                        filtro += " AND SALDO_CAPITAL " + saldoCapitalO + " " + saldoCapital;
                    else
                        filtro += "SALDO_CAPITAL " + saldoCapitalO + " " + saldoCapital;
                if ((saldoInteres != null) && (saldoInteres != ""))
                    if (filtro != "")
                        filtro += " AND SALDO_INTERES " + saldoInteresO + " " + saldoInteres;
                    else
                        filtro += "SALDO_INTERES " + saldoInteresO + " " + saldoInteres;
                if ((saldoSegurosIVA != null) && (saldoSegurosIVA != ""))
                    if (filtro != "")
                        filtro += " AND SALDO_SEGUROS_IVA " + saldoSegurosIVAO + " " + saldoSegurosIVA;
                    else
                        filtro += "SALDO_SEGUROS_IVA " + saldoSegurosIVAO + " " + saldoSegurosIVA;
                if ((saldoOtros != null) && (saldoOtros != ""))
                    if (filtro != "")
                        filtro += " AND SALDO_OTROS " + saldoOtrosO + " " + saldoOtros;
                    else
                        filtro += "SALDO_OTROS " + saldoOtrosO + " " + saldoOtros;
                if ((excedente != null) && (excedente != ""))
                    if (filtro != "")
                        filtro += " AND EXCEDENTE " + excedenteO + " " + excedente;
                    else
                        filtro += "EXCEDENTE " + excedenteO + " " + excedente;
                if ((capitalVigente != null) && (capitalVigente != ""))
                    if (filtro != "")
                        filtro += " AND CAPITAL_VIGENTE " + capitalVigenteO + " " + capitalVigente;
                    else
                        filtro += "CAPITAL_VIGENTE " + capitalVigenteO + " " + capitalVigente;
                if ((capitalMora != null) && (capitalMora != ""))
                    if (filtro != "")
                        filtro += " AND CAPITAL_MORA " + capitalMoraO + " " + capitalMora;
                    else
                        filtro += "CAPITAL_MORA " + capitalMoraO + " " + capitalMora;
                if ((capitalVencido != null) && (capitalVencido != ""))
                    if (filtro != "")
                        filtro += " AND CAPITAL_VENCIDO " + capitalVencidoO + " " + capitalVencido;
                    else
                        filtro += "CAPITAL_VENCIDO " + capitalVencidoO + " " + capitalVencido;
                if ((interesVigente != null) && (interesVigente != ""))
                    if (filtro != "")
                        filtro += " AND INTERES_VIGENTE " + interesVigenteO + " " + interesVigente;
                    else
                        filtro += "INTERES_VIGENTE " + interesVigenteO + " " + interesVigente;
                if ((interesMora != null) && (interesMora != ""))
                    if (filtro != "")
                        filtro += " AND INTERES_MORA " + interesMoraO + " " + interesMora;
                    else
                        filtro += "INTERES_MORA " + interesMoraO + " " + interesMora;
                if ((interesVencido != null) && (interesVencido != ""))
                    if (filtro != "")
                        filtro += " AND INTERES_VENCIDO " + interesVencidoO + " " + interesVencido;
                    else
                        filtro += "INTERES_VENCIDO " + interesVencidoO + " " + interesVencido;
                if ((valorCuota != null) && (valorCuota != ""))
                    if (filtro != "")
                        filtro += " AND VALOR_CUOTA " + valorCuotaO + " " + valorCuota;
                    else
                        filtro += "VALOR_CUOTA " + valorCuotaO + " " + valorCuota;
                if ((montoOtorgado != null) && (montoOtorgado != ""))
                    if (filtro != "")
                        filtro += " AND MONTO_OTORGADO " + montoOtorgadoO + " " + montoOtorgado;
                    else
                        filtro += "MONTO_OTORGADO " + montoOtorgadoO + " " + montoOtorgado;
                if ((valorValuo != null) && (valorValuo != ""))
                    if (filtro != "")
                        filtro += " AND VALOR_VALUO " + valorValuoO + " " + valorValuo;
                    else
                        filtro += "VALOR_VALUO " + valorValuoO + " " + valorValuo;
                if ((valorContable != null) && (valorContable != ""))
                    if (filtro != "")
                        filtro += " AND VALOR_CONTABLE " + valorContableO + " " + valorContable;
                    else
                        filtro += "VALOR_CONTABLE " + valorContableO + " " + valorContable;
                if ((totalIngresos != null) && (totalIngresos != ""))
                    if (filtro != "")
                        filtro += " AND TOTAL_INGRESOS " + totalIngresosO + " " + totalIngresos;
                    else
                        filtro += "TOTAL_INGRESOS " + totalIngresosO + " " + totalIngresos;

                if ((claseProducto != null) && (claseProducto != ""))
                    if (filtro != "")
                        filtro += " AND CLASE_PRODUCTO LIKE '%" + claseProducto + "%'";
                    else
                        filtro += "CLASE_PRODUCTO LIKE '%" + claseProducto + "%'";
                if ((codigoCliente != null) && (codigoCliente != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_CLIENTE LIKE '%" + codigoCliente + "%'";
                    else
                        filtro += "CODIGO_CLIENTE LIKE '%" + codigoCliente + "%'";
                if ((numeroProducto != null) && (numeroProducto != ""))
                    if (filtro != "")
                        filtro += " AND NUMERO_PRODUCTO LIKE '%" + numeroProducto + "%'";
                    else
                        filtro += "NUMERO_PRODUCTO LIKE '%" + numeroProducto + "%'";
                if ((codigoUsuario != null) && (codigoUsuario != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_USUARIO LIKE '%" + codigoUsuario + "%'";
                    else
                        filtro += "CODIGO_USUARIO LIKE '%" + codigoUsuario + "%'";
                if ((secuencia != null) && (secuencia != ""))
                    if (filtro != "")
                        filtro += " AND SECUENCIA LIKE '%" + secuencia + "%'";
                    else
                        filtro += "SECUENCIA LIKE '%" + secuencia + "%'";
                if ((codigoFinanciera != null) && (codigoFinanciera != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_FINANCIERA LIKE '%" + codigoFinanciera + "%'";
                    else
                        filtro += "CODIGO_FINANCIERA LIKE '%" + codigoFinanciera + "%'";
                if ((colectorMovimiento != null) && (colectorMovimiento != ""))
                    if (filtro != "")
                        filtro += " AND COLECTOR_MOVIMIENTO LIKE '%" + colectorMovimiento + "%'";
                    else
                        filtro += "COLECTOR_MOVIMIENTO LIKE '%" + colectorMovimiento + "%'";
                if ((tipoTransaccion != null) && (tipoTransaccion != ""))
                    if (filtro != "")
                        filtro += " AND TIPO_TRANSACCION LIKE '%" + tipoTransaccion + "%'";
                    else
                        filtro += "TIPO_TRANSACCION LIKE '%" + tipoTransaccion + "%'";
                if ((tipoOperacion != null) && (tipoOperacion != ""))
                    if (filtro != "")
                        filtro += " AND TIPO_OPERACION LIKE '%" + tipoOperacion + "%'";
                    else
                        filtro += "TIPO_OPERACION LIKE '%" + tipoOperacion + "%'";
                if ((formaPago != null) && (formaPago != ""))
                    if (filtro != "")
                        filtro += " AND FORMA_PAGO LIKE '%" + formaPago + "%'";
                    else
                        filtro += "FORMA_PAGO LIKE '%" + formaPago + "%'";
                if ((codigoAgenciaBanco != null) && (codigoAgenciaBanco != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_AGENCIA_BANCO LIKE '%" + codigoAgenciaBanco + "%'";
                    else
                        filtro += "CODIGO_AGENCIA_BANCO LIKE '%" + codigoAgenciaBanco + "%'";
                if ((numeroRecibo != null) && (numeroRecibo != ""))
                    if (filtro != "")
                        filtro += " AND NUMERO_RECIBO LIKE '%" + numeroRecibo + "%'";
                    else
                        filtro += "NUMERO_RECIBO LIKE '%" + numeroRecibo + "%'";
                if ((codigoPatrono != null) && (codigoPatrono != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_PATRONO LIKE '%" + codigoPatrono + "%'";
                    else
                        filtro += "CODIGO_PATRONO LIKE '%" + codigoPatrono + "%'";
                if ((estadoCartera != null) && (estadoCartera != ""))
                    if (filtro != "")
                        filtro += " AND ESTADO_CARTERA LIKE '%" + estadoCartera + "%'";
                    else
                        filtro += "ESTADO_CARTERA LIKE '%" + estadoCartera + "%'";
                if ((colectorConvenio != null) && (colectorConvenio != ""))
                    if (filtro != "")
                        filtro += " AND COLECTOR_CONVENIO LIKE '%" + colectorConvenio + "%'";
                    else
                        filtro += "COLECTOR_CONVENIO LIKE '%" + colectorConvenio + "%'";
                if ((facturaConvenio != null) && (facturaConvenio != ""))
                    if (filtro != "")
                        filtro += " AND FACTURA_CONVENIO LIKE '%" + facturaConvenio + "%'";
                    else
                        filtro += "FACTURA_CONVENIO LIKE '%" + facturaConvenio + "%'";
                if ((nombreCompleto != null) && (nombreCompleto != ""))
                    if (filtro != "")
                        filtro += " AND NOMBRE_COMPLETO LIKE '%" + nombreCompleto + "%'";
                    else
                        filtro += "NOMBRE_COMPLETO LIKE '%" + nombreCompleto + "%'";
                if ((codigoAgencia != null) && (codigoAgencia != ""))
                    if (filtro != "")
                        filtro += " AND CODIGO_AGENCIA LIKE '%" + codigoAgencia + "%'";
                    else
                        filtro += "CODIGO_AGENCIA LIKE '%" + codigoAgencia + "%'";

                var records = _SQLBDEntities.SP_FILTRAR_TRANSACCIONES(filtro).ToList().AsQueryable();

                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista de datos");
            }
        }


        public ActionResult _Tabla(String[] ids)
        {
            try
            {
                var record = _SQLBDEntities.VIEW_TRANSACCIONES.ToList();
                record = record.Where(x => ids.Contains(x.SECUENCIA.ToString())).ToList();
                ViewBag.tabla = record;
                return PartialView("_Tabla");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista de datos");
            }
        }


        [HttpPost]
        public JsonResult NotificarAlertaManual(int ID_TIPO_ALERTA, String asunto, String correos, String[] ids)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.ToList();
                transacciones = transacciones.Where(x => ids.Contains(x.SECUENCIA.ToString())).ToList();

                var tipoalerta = _tipoAlertaBlo.GetAll().Where(y => y.ID == ID_TIPO_ALERTA).First();

                StringBuilder mBuilder = new StringBuilder();

                mBuilder.Append(@"<table border=""1"" align=""center"" cellpadding=""2"" cellspacing=""0"" style=""color:black;font-family:arial,helvetica,sans-serif;text-align:center;"" >");
                mBuilder.Append(@"<tr style =""font-size: 14px;font-weight: normal;background: '#BDBDBD';"">");
                mBuilder.Append(@"<th>Descripción</th>");
                mBuilder.Append(@"<th>Código Cliente</th>");
                mBuilder.Append(@"<th>Número Factura</th>");
                mBuilder.Append(@"<th>Fecha Movimiento</th>");
                mBuilder.Append(@"<th>Fecha Alerta</th>");
                mBuilder.Append(@"<th>Monto Transacción</th>");
                mBuilder.Append(@"</tr>");

                foreach (VIEW_TRANSACCIONES item in transacciones)
                {
                    mBuilder.Append(@"<tr>");
                    mBuilder.Append(@"<td>Alerta Manual</td>");
                    mBuilder.Append(@"<td>" + item.CODIGO_CLIENTE.ToString() + "</td>");
                    mBuilder.Append(@"<td>" + item.NUMERO_RECIBO + "</td>");
                    mBuilder.Append(@"<td>" + item.FECHA_CALENDARIO.Value.ToString("dd/MM/yyyy") + "</td>");
                    mBuilder.Append(@"<td>" + DateTime.Today.ToShortDateString() + "</td>");
                    mBuilder.Append(@"<td>$" + item.VALOR_TRANSACCION.Value.ToString("0,0.00") + "</td>");
                    mBuilder.Append(@"</tr>");
                }

                mBuilder.Append(@"</table>");

                String tabla = mBuilder.ToString();

                foreach (VIEW_TRANSACCIONES data in transacciones)
                {
                    ALE_ALERTA alerta = new ALE_ALERTA();

                    if (data.CODIGO_CLIENTE == null)
                        data.CODIGO_CLIENTE = 0000;
                    if (data.NUMERO_RECIBO == null)
                        data.NUMERO_RECIBO = "0000";

                    var agencia = _catAgenciaBlo.GetAll()
                                  .Where(x => x.CODIGO_AGENCIA == Convert.ToInt32(data.CODIGO_AGENCIA))
                                  .ToList();

                    alerta.ID_AGENCIA = (agencia.Count > 0) ? agencia.FirstOrDefault().ID : 1;
                    alerta.ID_TIPO_ALERTA = tipoalerta.ID;
                    alerta.ID_CLIENTE = (long)data.CODIGO_CLIENTE;

                    alerta.NOMBRE_CLIENTE = data.NOMBRE_COMPLETO;
                    alerta.NUMERO_FACTURA = data.NUMERO_RECIBO;
                    alerta.FECHA_MOVIMIENTO = (DateTime)data.FECHA_CALENDARIO;
                    alerta.VALOR_PAGO = (Decimal)data.VALOR_TRANSACCION;
                    alerta.DESCRIPCION = "Alerta manual";
                    alerta.FECHA_ALERTA = DateTime.Today;
                    alerta.COLOR = tipoalerta.COLOR;
                    alerta.DESCRIPCION_ALERTA = tipoalerta.DESCRIPCION;
                    alerta.ESTADO_ANTERIOR = ALE_ALERTA.ESTADO_NATIFICADA;
                    alerta.ESTADO_ACTUAL = ALE_ALERTA.ESTADO_NATIFICADA;

                    _alertaBlo.ValidarPermiso(SEG_PERMISO.AGREGAR);

                    _alertaBlo.Save(alerta);
                }

                correos = correos.Replace(",", ";");
                _SQLBDEntities.SP_EMVIOCORREO(asunto, tabla, correos);
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