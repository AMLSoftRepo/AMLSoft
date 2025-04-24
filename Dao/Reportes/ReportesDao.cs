using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Model;
using Model.Monitoreo;

namespace Dao.Reportes
{
    public class ReportesDao : IReportesDao
    {
        /// <summary>
        /// Propiedad para administrar el mecanismo de logeo 
        /// de informacion y fallas
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Propiedad que representa el objeto principal de acceso a datos
        /// </summary>
        protected SQLBDEntities _SQLBDEntities = new SQLBDEntities();


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE MATRIZ DE RIESGO"


        /// <summary>
        /// Metodo que permite obtener una lista de ids de matriz filtrado por
        /// un rango de fechas.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de ids de Matriz</returns>
        public List<long> idsMatrizPorRangoFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<long> idsMatriz = new List<long>();
            try
            {
                idsMatriz = _SQLBDEntities.MAT_MATRIZ.AsNoTracking()
                            .Where(x =>
                                x.FECHA >= fechaInicial &&
                                x.FECHA <= fechaFinal)
                            .Select(x => x.ID).ToList();

                return idsMatriz;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Error al obtener lista de Ids de matriz por rango de fecha", e);
            }
        }


        /// <summary>
        /// Metodo que permite generar reporte para matriz de riesgo por rango de fechas
        /// y agencias las cuales pueden ser una ó todas las agencias
        /// </summary>
        /// <param name="idsMatriz">Lista de ids de matriz</param>
        /// <param name="idAgencia">Identificador de CAT_AGENCIA</param>
        /// <returns>Lista de controles</returns>
        public List<MAT_MATRIZ_CONTROL> ReporteMatriz_Controles(List<long> idsMatriz, int idAgencia)
        {
            try
            {
                var controles = _SQLBDEntities.MAT_MATRIZ_CONTROL
                    .Where(x => idsMatriz.Contains(x.ID_MATRIZ) && x.ID_AGENCIA == (idAgencia == 0 ? x.ID_AGENCIA : idAgencia))
                    .ToList()
                    .Select(x => new MAT_MATRIZ_CONTROL
                    {
                        ID = x.ID,
                        ID_EVENTOS = string.Join(", ", _SQLBDEntities.MAT_MATRIZ_CONTROL_EVENTO.AsNoTracking()
                                                        .Where(c => c.ID_CONTROL == x.ID && idsMatriz.Contains(c.ID_MATRIZ))
                                                        .Select(c => c.ID_EVENTO).ToList()),
                        AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                        AUTOMATIZACION = x.AUTOMATIZACION,
                        DESCRIPCION = x.DESCRIPCION,
                        DISENO = x.DISENO,
                        DOCUMENTACION = x.DOCUMENTACION,
                        FRECUENCIA = x.FRECUENCIA,
                        MEZCLA = x.MEZCLA,
                        OBSERVACIONES = x.OBSERVACIONES,
                        TIPO_CONTROL =x.TIPO_CONTROL,
                        TOTAL_POR = x.TOTAL_POR
                    }).ToList();

                return controles;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Error al obtener controles para reporte de matriz de riesgo", e);
            }
        }

        /// <summary>
        /// Metodo que permite obtener el nemero de trimestre partiendo de una fecha
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns>String con el numero de trimestre</returns>
        private string FromDate(DateTime fecha)
        {
            return Math.Round(Convert.ToDouble((fecha.Month - 1) / 3) + 1, 0) + "° T";
        }

        /// <summary>
        /// Metodo que permite generar reporte para matriz de riesgo por rango de fechas
        /// y agencias las cuales pueden ser una ó todas las agencias
        /// </summary>
        /// <param name="idsMatriz">Lista de ids de matriz</param>
        /// <param name="idAgencia">Identificador de CAT_AGENCIA</param>
        /// <returns>Lista de eventos</returns>
        public List<MAT_MATRIZ_EVENTO_RIESGO> ReporteMatriz_Eventos(List<long> idsMatriz, int idAgencia)
        {
            try
            {
                _SQLBDEntities.Configuration.ProxyCreationEnabled = true;

                var fechas = _SQLBDEntities.MAT_MATRIZ.AsNoTracking().ToList();

                var eventos = _SQLBDEntities.MAT_MATRIZ_EVENTO_RIESGO.AsNoTracking()
                    .Where(x => idsMatriz.Contains(x.ID_MATRIZ) && x.ID_AGENCIA == (idAgencia == 0 ? x.ID_AGENCIA : idAgencia))
                    .ToList()
                    .Select(x => new MAT_MATRIZ_EVENTO_RIESGO
                    {
                        ID = x.ID,
                        AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                        CAUSA_RIESGO = x.CAUSA_RIESGO,
                        COLOR_RIESGO_INHERENTE = x.COLOR_RIESGO_INHERENTE,
                        COLOR_RIESGO_RESIDUAL = x.COLOR_RIESGO_RESIDUAL,
                        COMO = x.COMO,
                        DESCRIPCION = x.DESCRIPCION,
                        FACTOR_RIESGO = x.FACTOR_RIESGO,
                        IMPACTO = x.IMPACTO,
                        PROBABILIDAD_OCURRENCIA = x.PROBABILIDAD_OCURRENCIA,
                        RIESGO = x.RIESGO,
                        RIESGO_INHERENTE = x.RIESGO_INHERENTE,
                        UNIDAD = x.UNIDAD,
                        EFICACIA_CONTROL = x.EFICACIA_CONTROL,
                        RIESGO_RESIDUAL = x.RIESGO_RESIDUAL,
                        TRIMESTRE = FromDate(x.MAT_MATRIZ.FECHA),
                        ANIO = x.MAT_MATRIZ.FECHA.Year
                    }).ToList();

                return eventos;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Error al obtener eventos para reporte de matriz de riesgo", e);
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE OFICIOS"


        /// <summary>
        /// Metodo en el que genera reporte de resumen de requerimiento de información
        /// ó información de OFICIOS - se muestran información de los oficios que ya estan procesados
        /// y se agrega un filtro de fechas para luego obtener todas las personas de estos OFICIOS
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<MON_OFICIO_PERSONA> ReporteInformacionOficio(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var idsOficios = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                                .Where(x =>
                                  x.PROCESADO &&
                                    x.FECHA_RESPUESTA_UIF >= fechaIni &&
                                    x.FECHA_RESPUESTA_UIF <= fechaFin
                                  ).Select(x => x.ID)
                                .ToList();

                var personas = _SQLBDEntities.MON_OFICIO_PERSONA
                             .Where(x => idsOficios.Contains(x.ID_OFICIO))
                             .ToList()
                             .Select(x => new MON_OFICIO_PERSONA
                             {
                                 INSTITUCIONES = string.Join(" ", _SQLBDEntities.MON_CONTACTO_INSTITUCION.AsNoTracking()
                                                 .Where(c => c.ID == x.MON_OFICIO.ID_CONTACTO_SOLICITANTE)
                                                 .Select(c => c.MON_CAT_UNIDAD.MON_CAT_INSTITUCION.NOMBRE + ": " + c.MON_CAT_UNIDAD.DESCRIPCION)),
                                 ID_OFICIO = x.ID_OFICIO,
                                 AEX = x.AEX,
                                 CHEQUES = x.CHEQUES,
                                 COTIZACIONES = x.COTIZACIONES,
                                 PRESTAMOS = x.PRESTAMOS,
                                 SOLICITUDES = x.SOLICITUDES,
                                 TIPO_PERSONA = x.TIPO_PERSONA
                             }).ToList();

                return personas;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteInformacionOficio  - " + e);
                throw new Exception("Error en generar reporte de : ReporteInformacionOficio");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de respuestas de oficios procesados, filtrados por 
        /// un rango de fechas, además la informaión de los OFICIOS esta filtrada por la unidad 
        /// a la cual se enviará la información 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="idUnidad">Identificador único de CAT_UNIDAD_INSTITUCION</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<MON_OFICIO> ReporteRespuestaOficios(DateTime fechaIni, DateTime fechaFin, int idUnidad)
        {
            try
            {
                var solicitantes = _SQLBDEntities.MON_CONTACTO_INSTITUCION.AsNoTracking()
                        .Where(x => x.ID_UNIDAD == idUnidad)
                        .Select(x => x.ID)
                        .ToList();

                if (!solicitantes.Any())
                {
                    log.Warn("No se encontraron solicitantes para la unidad: " + idUnidad);
                    return new List<MON_OFICIO>(); // Devolver una lista vacía
                }

                var oficios = _SQLBDEntities.MON_OFICIO
                    .Where(x => x.PROCESADO)
                    .ToList()
                    .Where(x =>
                            x.FECHA_RESPUESTA_UIF >= fechaIni &&
                            x.FECHA_RESPUESTA_UIF <= fechaFin &&
                            solicitantes.Contains(x.ID_CONTACTO_SOLICITANTE))
                    .Select(x => new MON_OFICIO
                    {
                        NUMERO_OFICIO = x.NUMERO_OFICIO,
                        REFERENCIA = x.REFERENCIA,
                        FECHA_OFICIO = x.FECHA_OFICIO,
                        FECHA_RECIBIDO = x.FECHA_RECIBIDO
                    }).ToList();

                return oficios;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteRespuestaOficios  - " + e);
                throw new Exception("Error en generar reporte de : ReporteRespuestaOficios");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE ALERTAS"


        /// <summary>
        /// Metodo en el que se genera reporte de alertas procesadas,
        /// las cuales estan filtradas por un rango de fechas
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="tipoAlerta">Identificador de tipo de alerta</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<ALE_ALERTA> ReporteAlertas(DateTime fechaIni, DateTime fechaFin, int tipoAlerta)
        {
            try
            {
                var alertas = _SQLBDEntities.ALE_ALERTA.AsNoTracking()
                              .Where(x => x.FECHA_ALERTA >= fechaIni &&
                                  x.FECHA_ALERTA <= fechaFin &&
                                  x.ID_TIPO_ALERTA == (tipoAlerta == 0 ? x.ID_TIPO_ALERTA : tipoAlerta))
                              .ToList();

                return alertas;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteAlertas  - " + e);
                throw new Exception("Error en generar reporte de : ReporteAlertas");
            }
        }

        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE PAGOS"

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios entre $573.43 y $5,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_REPORTE_CONTROLPAGOS> ReportePagosDiariosIgualesSuperiores571_5000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_REPORTE_CONTROLPAGOS.AsNoTracking()
                                   .Where(x => x.FECHA_CALENDARIO.HasValue &&
                                               x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                               x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                               x.VALOR_TRANSACCION.HasValue &&
                                               x.VALOR_TRANSACCION >= 571.43 &&
                                               x.VALOR_TRANSACCION < 5000)
                                           .ToList();
                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperiores571_5000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperiores571_5000");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $5,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_REPORTE_CONTROLPAGOS> ReportePagosDiariosIgualesSuperioresA5000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_REPORTE_CONTROLPAGOS.AsNoTracking()
                                   .Where(x =>
                                        x.FECHA_CALENDARIO.HasValue &&
                                        x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                        x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                        x.VALOR_TRANSACCION.HasValue &&
                                        x.VALOR_TRANSACCION >= 5000 &&
                                        x.VALOR_TRANSACCION < 10000).ToList();
                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA5000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA5000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $10,000.00 
        /// COLECTORES DE PAGOS EN EFECTIVO
        /// 2014
        /// 2035
        /// 2036
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote
        /// </returns>
        public List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                   .Where(x =>
                                        x.FECHA_CALENDARIO.HasValue &&
                                        x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                        x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                        x.VALOR_TRANSACCION.HasValue &&
                                        x.VALOR_TRANSACCION >= 10000 &&
                                        x.CLASE_PRODUCTO == "PA" &&
                                        (x.COLECTOR_MOVIMIENTO == "2014" ||
                                        x.COLECTOR_MOVIMIENTO == "2035" ||
                                        x.COLECTOR_MOVIMIENTO == "2036"))
                                   .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $10,000.00 
        /// COLECTORES DE PAGOS EN EFECTIVO
        /// 2014
        /// 2035
        /// 2036
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<String> listSecuencia = new List<string>();

                var listIds = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                            .Where(x =>
                                x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                x.CLASE_PRODUCTO == "PA" &&
                                (x.COLECTOR_MOVIMIENTO == "2014" ||
                                x.COLECTOR_MOVIMIENTO == "2035" ||
                                x.COLECTOR_MOVIMIENTO == "2036"))
                            .GroupBy(x => new { x.NUMERO_PRODUCTO, x.CODIGO_CLIENTE })
                            .Select(x => new
                             {
                                 MONTO = x.Sum(y => y.VALOR_TRANSACCION),
                                 TOTAL_PÁGOS = x.Count(),
                                 ListIds = x.Select(ids => ids.SECUENCIA).ToList()
                             })
                            .Where(x => x.TOTAL_PÁGOS >= 2 && x.MONTO >= 10000)
                            .Select(x => x.ListIds)
                            .ToList();


                foreach (List<String> item in listIds)
                    listSecuencia.AddRange(item);


                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                    .Where(x => listSecuencia.Contains(x.SECUENCIA))
                                    .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $25,000.00 
        /// COLECTORES DE PAGOS EN OTROS MEDIOS 
        /// 2042
        /// 2043
        /// 2044
        /// 2045
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                   .Where(x =>
                                        x.FECHA_CALENDARIO.HasValue &&
                                        x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                        x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                        x.VALOR_TRANSACCION.HasValue &&
                                        x.VALOR_TRANSACCION >= 25000 &&
                                        x.CLASE_PRODUCTO == "PA" &&
                                        (x.COLECTOR_MOVIMIENTO == "2042" ||
                                        x.COLECTOR_MOVIMIENTO == "2043" ||
                                        x.COLECTOR_MOVIMIENTO == "2044" ||
                                        x.COLECTOR_MOVIMIENTO == "2045"))
                                   .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA25000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA25000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $25,000.00 
        /// COLECTORES DE PAGOS EN OTROS MEDIOS 
        /// 2042
        /// 2043
        /// 2044
        /// 2045
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<String> listSecuencia = new List<string>();

                var listIds = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                            .Where(x =>
                                x.FECHA_CALENDARIO.Value >= fechaIni.Date &&
                                x.FECHA_CALENDARIO.Value <= fechaFin.Date &&
                                x.CLASE_PRODUCTO == "PA" &&
                                (x.COLECTOR_MOVIMIENTO == "2042" ||
                                x.COLECTOR_MOVIMIENTO == "2043" ||
                                x.COLECTOR_MOVIMIENTO == "2044" ||
                                x.COLECTOR_MOVIMIENTO == "2045"))
                            .GroupBy(x => new { x.NUMERO_PRODUCTO, x.CODIGO_CLIENTE })
                            .Select(x => new
                            {
                                MONTO = x.Sum(y => y.VALOR_TRANSACCION),
                                TOTAL_PÁGOS = x.Count(),
                                ListIds = x.Select(ids => ids.SECUENCIA).ToList()
                            })
                            .Where(x => x.TOTAL_PÁGOS >= 2 && x.MONTO >= 25000)
                            .Select(x => x.ListIds)
                            .ToList();


                foreach (List<String> item in listIds)
                    listSecuencia.AddRange(item);


                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                    .Where(x => listSecuencia.Contains(x.SECUENCIA))
                                    .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA25000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA25000");
            }
        }

        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE LISTAS"


        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista de la ONU 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_ONU> ReporteListaONU()
        {
            try
            {
                var personasONU = _SQLBDEntities.LIS_ONU.AsNoTracking().ToList();

                return personasONU;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaONU  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaONU");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_ONU> ReporteListaONUDetalle(int idPersona)
        {
            try
            {
                var personaONUDetalle = _SQLBDEntities.LIS_ONU.AsNoTracking()
                                        .Where(x => x.ID == idPersona)
                                        .ToList();

                return personaONUDetalle;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaONUDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaONUDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_ONU_DOCUMENTO> ReporteListaONUDetalleDocumento(int idPersona)
        {
            try
            {
                var personaONUDetalleDocumento = _SQLBDEntities.LIS_ONU_DOCUMENTO.AsNoTracking()
                                                .Where(x => x.ID_LIS_ONU == idPersona)
                                                .ToList();

                return personaONUDetalleDocumento;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaONUDetalleDocumento  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaONUDetalleDocumento");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_ONU_DIRECCION> ReporteListaONUDetalleDireccion(int idPersona)
        {
            try
            {
                var personaONUDetalleDireccion = _SQLBDEntities.LIS_ONU_DIRECCION.AsNoTracking()
                                                .Where(x => x.ID_LIS_ONU == idPersona)
                                                .ToList();

                return personaONUDetalleDireccion;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaONUDetalleDireccion  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaONUDetalleDireccion");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_ONU_ALIAS> ReporteListaONUDetalleAlias(int idPersona)
        {
            try
            {
                var personaONUDetalleDireccion = _SQLBDEntities.LIS_ONU_ALIAS.AsNoTracking()
                                                .Where(x => x.ID_LIS_ONU == idPersona)
                                                .ToList();

                return personaONUDetalleDireccion;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaONUDetalleAlias  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaONUDetalleAlias");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista SDN de la OFAC 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN> ReporteListaSDN()
        {
            try
            {
                var personasSDN = _SQLBDEntities.LIS_SDN.AsNoTracking().ToList();

                return personasSDN;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDN  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDN");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN> ReporteListaSDNDetalle(int idPersona)
        {
            try
            {
                var personaSDNDetalle = _SQLBDEntities.LIS_SDN.AsNoTracking()
                                        .Where(x => x.ID == idPersona)
                                        .ToList();

                return personaSDNDetalle;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDNDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDNDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN_DOCUMENTO> ReporteListaSDNDetalleDocumento(int idPersona)
        {
            try
            {
                var personaSDNDetalleDocumento = _SQLBDEntities.LIS_SDN_DOCUMENTO.AsNoTracking()
                                                .Where(x => x.ID_LIS_SDN == idPersona)
                                                .ToList();

                return personaSDNDetalleDocumento;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDNDetalleDocumento  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDNDetalleDocumento");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN_DIRECCION> ReporteListaSDNDetalleDireccion(int idPersona)
        {
            try
            {
                var personaSDNDetalleDireccion = _SQLBDEntities.LIS_SDN_DIRECCION.AsNoTracking()
                                                .Where(x => x.ID_LIS_SDN == idPersona)
                                                .ToList();

                return personaSDNDetalleDireccion;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDNDetalleDireccion  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDNDetalleDireccion");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN_AKA> ReporteListaSDNDetalleAlias(int idPersona)
        {
            try
            {
                var personaSDNDetalleAlias = _SQLBDEntities.LIS_SDN_AKA.AsNoTracking()
                                            .Where(x => x.ID_LIS_SDN == idPersona)
                                            .ToList();

                return personaSDNDetalleAlias;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDNDetalleAlias  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDNDetalleAlias");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_SDN_NACIONALIDAD> ReporteListaSDNDetalleNacionalidad(int idPersona)
        {
            try
            {
                var personaSDNDetalleNacionalidad = _SQLBDEntities.LIS_SDN_NACIONALIDAD.AsNoTracking()
                                                    .Where(x => x.ID_LIS_SDN == idPersona)
                                                    .ToList();

                return personaSDNDetalleNacionalidad;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaSDNDetalleNacionalidad  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaSDNDetalleNacionalidad");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en una lista personalizada
        /// </summary>
        /// <param name="idLista">ID de la Lista Personalizada</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PERSONALIZADA> ReporteListaPersonalizada(int idLista)
        {
            try
            {
                var idPersonas = _SQLBDEntities.LIS_GENERAL_PERSONALIZADA.AsNoTracking()
                    .Where(x => x.ID_LISTA_GENERAL == idLista)
                    .Select(x => x.ID_LISTA_PERSONALIZADA)
                    .ToList();

                // Validar si hay relaciones
                if (!idPersonas.Any())
                {
                    log.Warn($"No se encontraron personas relacionadas para la lista con ID {idLista}");
                    return new List<LIS_PERSONALIZADA>();
                }

                // 2. Obtener personas que están en esa lista
                var personas = _SQLBDEntities.LIS_PERSONALIZADA.AsNoTracking()
                    .Where(x => idPersonas.Contains(x.ID))
                    .ToList(); // Forzar la consulta a la BD aquí

                // Validar si se encontraron personas
                if (!personas.Any())
                {
                    log.Warn($"No se encontraron detalles de personas con IDs en la lista {idLista}");
                    return new List<LIS_PERSONALIZADA>();
                }

                // 3. Obtener todos los países una sola vez
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();

                // 4. Proyectar los resultados
                var resultado = personas.Select(x =>
                {
                    var paisNombre = "";
                    if (x.PAIS_NACIMIENTO.HasValue)
                    {
                        var pais = paises.FirstOrDefault(p => (int)p.CODIGO_PAIS == x.PAIS_NACIMIENTO.Value);
                        paisNombre = pais?.NOMBRE ?? "";
                    }

                    return new LIS_PERSONALIZADA
                    {
                        ID = x.ID,
                        ID_UNICO = x.ID_UNICO,
                        PRIMER_NOMBRE = x.PRIMER_NOMBRE,
                        SEGUNDO_NOMBRE = x.SEGUNDO_NOMBRE,
                        TERCER_NOMBRE = x.TERCER_NOMBRE,
                        PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                        SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                        TERCER_APELLIDO = x.TERCER_APELLIDO,
                        RAZON = x.RAZON,
                        PAIS_NACIMIENTO = x.PAIS_NACIMIENTO,
                        PAIS_NACIMIENTO_NOMBRE = paisNombre
                    };
                }).ToList();

                log.Info($"Se generó el reporte con {resultado.Count} personas para la lista {idLista}");

                return resultado;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizada  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizada");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o mas listas personalizadas
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PERSONALIZADA> ReporteListaPersonalizadaDetalle(int idPersona)
        {
            try
            {
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();

                var personas = _SQLBDEntities.LIS_PERSONALIZADA.AsNoTracking()
                    .Where(x => x.ID == idPersona)
                    .ToList();

                var personaPersonalizadaDetalle = personas.Select(x => new LIS_PERSONALIZADA
                    {
                        ID = x.ID,
                        ID_UNICO = x.ID_UNICO,
                        PRIMER_NOMBRE = x.PRIMER_NOMBRE,
                        SEGUNDO_NOMBRE = x.SEGUNDO_NOMBRE,
                        TERCER_NOMBRE = x.TERCER_NOMBRE,
                        PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                        SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                        TERCER_APELLIDO = x.TERCER_APELLIDO,
                        RAZON = x.RAZON,
                        PAIS_NACIMIENTO = x.PAIS_NACIMIENTO,
                        PAIS_NACIMIENTO_NOMBRE = x.PAIS_NACIMIENTO != null ?
                            paises.FirstOrDefault(p => p.CODIGO_PAIS == x.PAIS_NACIMIENTO)?.NOMBRE ?? "" : ""
                    })
                    .ToList();

                return personaPersonalizadaDetalle;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizadaDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizadaDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o mas listas personalizadas
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PERSONALIZADA_DOCUMENTO> ReporteListaPersonalizadaDetalleDocumento(int idPersona)
        {
            try
            {
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();

                var documentos = _SQLBDEntities.LIS_PERSONALIZADA_DOCUMENTO.AsNoTracking()
                    .Where(x => x.ID_LISTA_PERSONALIZADA == idPersona)
                    .ToList();

                var personaPersonalizadaDetalleDocumento = documentos.Select(x => new LIS_PERSONALIZADA_DOCUMENTO
                    {
                        ID = x.ID,
                        COMENTARIO = x.COMENTARIO,
                        ID_LISTA_PERSONALIZADA = x.ID_LISTA_PERSONALIZADA,
                        NUMERO = x.NUMERO,
                        TIPO = x.TIPO,
                        PAIS_EXPEDICION = x.PAIS_EXPEDICION,
                        // Nueva propiedad para mostrar el nombre del país
                        PAIS_EXPEDICION_NOMBRE = x.PAIS_EXPEDICION != null ?
                            paises.FirstOrDefault(z => z.CODIGO_PAIS == x.PAIS_EXPEDICION)?.NOMBRE ?? "" : ""
                }).ToList();

                return personaPersonalizadaDetalleDocumento;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizadaDetalleDocumento  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizadaDetalleDocumento");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o mas listas personalizadas
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PERSONALIZADA_DIRECCION> ReporteListaPersonalizadaDetalleDireccion(int idPersona)
        {
            try
            {
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO.AsNoTracking().ToList();
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO.AsNoTracking().ToList();
                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION.AsNoTracking().ToList();

                var personaPersonalizadaDetalleDireccion = _SQLBDEntities.LIS_PERSONALIZADA_DIRECCION
                            .Where(a => a.ID_LISTA_PERSONALIZADA == idPersona)
                            .AsNoTracking()
                            .ToList()
                            .Select(
                                a => new LIS_PERSONALIZADA_DIRECCION
                                {
                                    ID = a.ID,
                                    ID_LISTA_PERSONALIZADA = a.ID_LISTA_PERSONALIZADA,
                                    DIRECCION_ESPECIFICA = a.DIRECCION_ESPECIFICA,
                                    AVENIDA_CALLE_PASAJE = a.AVENIDA_CALLE_PASAJE,
                                    ID_URBANIZACION = a.ID_URBANIZACION,
                                    URBANIZACION = urbanizaciones
                                        .FirstOrDefault(b => b.CODIGO_PAIS == a.ID_PAIS &&
                                                             b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO &&
                                                             b.CODIGO_MUNICIPIO == a.ID_MUNICIPIO &&
                                                             b.CODIGO_SECTOR == a.ID_URBANIZACION)?.DESCRIPCION ?? "",
                                    ID_MUNICIPIO = a.ID_MUNICIPIO,
                                    MUNICIPIO = municipios
                                        .FirstOrDefault(b => b.CODIGO_PAIS == a.ID_PAIS &&
                                                             b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO &&
                                                             b.CODIGO_MUNICIPIO == a.ID_MUNICIPIO)?.NOMBRE ?? "",
                                    ID_DEPARTAMENTO = a.ID_DEPARTAMENTO,
                                    DEPARTAMENTO = departamentos
                                        .FirstOrDefault(b => b.CODIGO_PAIS == a.ID_PAIS &&
                                                             b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO)?.NOMBRE ?? "",
                                    ID_PAIS = a.ID_PAIS,
                                    PAIS = paises
                                        .FirstOrDefault(b => b.CODIGO_PAIS == a.ID_PAIS)?.NOMBRE ?? "",
                                    COMENTARIO = a.COMENTARIO
                                }).ToList();

                return personaPersonalizadaDetalleDireccion;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizadaDetalleDireccion  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizadaDetalleDireccion");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o mas listas personalizadas
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PERSONALIZADA_ALIAS> ReporteListaPersonalizadaDetalleAlias(int idPersona)
        {
            try
            {
                var personaPersonalizadaDetalleAlias = _SQLBDEntities.LIS_PERSONALIZADA_ALIAS.AsNoTracking()
                                                       .Where(x => x.ID_LISTA_PERSONALIZADA == idPersona)
                                                       .ToList();

                return personaPersonalizadaDetalleAlias;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizadaDetalleAlias  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizadaDetalleAlias");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o mas listas personalizadas
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_GENERAL> ReporteListaPersonalizadaDetalleLista(int idPersona)
        {
            try
            {
                var idListas = _SQLBDEntities.LIS_GENERAL_PERSONALIZADA.AsNoTracking()
                               .Where(x => x.ID_LISTA_PERSONALIZADA == idPersona)
                               .Select(x => x.ID_LISTA_GENERAL).ToList();

                var personaPersonalizadaDetalleLista = _SQLBDEntities.LIS_GENERAL.AsNoTracking()
                                                       .Where(x => idListas.Contains(x.ID))
                                                       .ToList();

                return personaPersonalizadaDetalleLista;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPersonalizadaDetalleLista  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPersonalizadaDetalleLista");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista PEP 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PEP> ReporteListaPEP()
        {
            try
            {
                var personasPEP = _SQLBDEntities.LIS_PEP.Include("LIS_CAT_TITULOS")
                    .AsNoTracking().ToList();

                var listaFinal = personasPEP.Select(x => new LIS_PEP
                {
                    ID = x.ID,
                    APELLIDO_CASADA = x.APELLIDO_CASADA,
                    CARNET_RESIDENTE = x.CARNET_RESIDENTE,
                    CONOCIDO_POR = x.CONOCIDO_POR,
                    DECLARACION_JURADA = x.DECLARACION_JURADA,
                    DUI = x.DUI,
                    FUNCIONARIO_O_RELACION = x.FUNCIONARIO_O_RELACION,
                    NIT = x.NIT,
                    PASAPORTE = x.PASAPORTE,
                    PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                    PRIMER_NOMBRE = x.PRIMER_NOMBRE,
                    SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                    SEGUNDO_NOMBRE = x.SEGUNDO_NOMBRE,
                    TITULO_CARGO_PEP = x.TITULO_CARGO_PEP,
                    ABREVIATURA_TITULO = x.LIS_CAT_TITULOS != null ? x.LIS_CAT_TITULOS.ABREVIATURA : ""
                }).ToList();

                return listaFinal;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPEP  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPEP");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PEP> ReporteListaPEPDetalle(int idPEP)
        {
            try
            {
                var personaPEP = _SQLBDEntities.LIS_PEP.AsNoTracking()
                                 .Where(x => x.ID == idPEP)
                                 .ToList()
                                 .Select(x => new LIS_PEP
                                 {
                                     ID = x.ID,
                                     APELLIDO_CASADA = x.APELLIDO_CASADA,
                                     CARNET_RESIDENTE = x.CARNET_RESIDENTE,
                                     CONOCIDO_POR = x.CONOCIDO_POR,
                                     DECLARACION_JURADA = x.DECLARACION_JURADA,
                                     DUI = x.DUI,
                                     FUNCIONARIO_O_RELACION = x.FUNCIONARIO_O_RELACION,
                                     NIT = x.NIT,
                                     PASAPORTE = x.PASAPORTE,
                                     PRIMER_APELLIDO =  x.PRIMER_APELLIDO,
                                     PRIMER_NOMBRE = x.PRIMER_NOMBRE,
                                     SEGUNDO_APELLIDO =  x.SEGUNDO_APELLIDO,
                                     SEGUNDO_NOMBRE = x.SEGUNDO_NOMBRE,
                                     TITULO = x.LIS_CAT_TITULOS != null ? x.LIS_CAT_TITULOS.ABREVIATURA : ""
                                 }).ToList();

                return personaPEP;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPEPDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPEPDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PEP_CARGO> ReporteListaPEPDetalleCargo(int idPEP)
        {
            try
            {
                var personaPEPCargo = _SQLBDEntities.LIS_PEP_CARGO.AsNoTracking()
                                      .Where(x => x.ID_LIS_PEP == idPEP)
                                      .ToList();

                return personaPEPCargo;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPEPDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPEPDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<LIS_PEP_RELACION> ReporteListaPEPDetalleRelacion(int idPEP)
        {
            try
            {
                var personaPEPRelacion = _SQLBDEntities.LIS_PEP_RELACION.AsNoTracking()
                                        .Where(x => x.ID_LIS_PEP == idPEP)
                                        .ToList();

                return personaPEPRelacion;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaPEPDetalle  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaPEPDetalle");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de personas incluidas en oficios
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<MON_OFICIO_PERSONA> ReporteListaOficio(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var datos = _SQLBDEntities.MON_OFICIO_PERSONA
                    .Include(p => p.MON_OFICIO) // para que cargue la relación
                    .AsNoTracking()
                    .Where(p => p.MON_OFICIO.FECHA_OFICIO >= fechaIni && p.MON_OFICIO.FECHA_OFICIO <= fechaFin)
                    .ToList();

                // llenar manualmente la propiedad
                foreach (var p in datos)
                {
                    p.NUMERO_OFICIO = p.MON_OFICIO?.NUMERO_OFICIO; // asegúrate que exista
                }

                return datos;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteListaOficio  - " + e);
                throw new Exception("Error en generar reporte de : ReporteListaOficio");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE RESUMEN CLIENTE"


        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<VIEW_CLIENTE> ReporteResumenCliente(decimal codigoCliente)
        {
            try
            {
                var cliente = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                              .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                              .ToList();

                return cliente;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteResumenCliente  - " + e);
                throw new Exception("Error en generar reporte de : ReporteResumenCliente");
            }
        }



        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<VIEW_CLIENTE_DIRECCIONES> ReporteResumenClienteDirecciones(decimal codigoCliente)
        {
            try
            {
                var clienteDirecciones = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                                        .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                                        .ToList();

                return clienteDirecciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteResumenClienteDirecciones  - " + e);
                throw new Exception("Error en generar reporte de : ReporteResumenClienteDirecciones");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        public List<VIEW_CLIENTE_TELEFONOS> ReporteResumenClienteTelefonos(decimal codigoCliente)
        {
            try
            {
                var clienteTelefonos = _SQLBDEntities.VIEW_CLIENTE_TELEFONOS.AsNoTracking()
                                       .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                                       .ToList();

                return clienteTelefonos;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteResumenClienteTelefonos  - " + e);
                throw new Exception("Error en generar reporte de : ReporteResumenClienteTelefonos");
            }
        }


        /// <summary>
        /// Metodo que genera el reporte de transacciones por cliente partiendo de un rango de fechas
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns>DEVUELVE LA LISTA DE TRANSACCIONES DEL CLIENTE SEGUN EL RANGO DE FECHAS</returns>
        public List<VIEW_TRANSACCIONES> ReporteTransaccionesCliente(string FECHA_INI_T, string FECHA_FIN_T, decimal codigoCliente, string NOMBRE_T)
        {
            try
            {
                DateTime fi = DateTime.Parse(FECHA_INI_T);
                DateTime ff = DateTime.Parse(FECHA_FIN_T).AddHours(23);

                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                    .Where(x =>
                                        x.FECHA_CALENDARIO.HasValue &&
                                        x.FECHA_CALENDARIO.Value >= fi &&
                                        x.FECHA_CALENDARIO.Value <= ff &&
                                        x.CODIGO_CLIENTE == codigoCliente)
                                    .ToList();


                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteTransaccionesCliente  - " + e);
                throw new Exception("Error en generar reporte de : ReporteTransaccionesCliente");
            }
        }


        /// <summary>
        /// Metodo que genera el reporte de Alertas por cliente
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns>DEVUELVE LA LISTA DE ALERTAS DEL CLIENTE</returns>
        public List<ALE_ALERTA> ReporteAlertasCliente(decimal codigoCliente)
        {
            try
            {
                var alertas = _SQLBDEntities.ALE_ALERTA.AsNoTracking()
                              .Where(x => x.ID_CLIENTE == codigoCliente)
                              .ToList();

                return alertas;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteAlertasCliente  - " + e);
                throw new Exception("Error en generar reporte de : ReporteAlertasCliente");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE PRIMAS Y COMPLEMENTOS"


        /// <summary>
        /// Metodo en el que se genera reporte de primas y complementos superiores a $7,700.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_REPORTE_PRIMAS> ReportePrimasSuperioresA7500(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var primas = _SQLBDEntities.VIEW_REPORTE_PRIMAS.AsNoTracking()
                             .Where(x =>
                                    x.FECHA_CALENDARIO >= fechaIni &&
                                    x.FECHA_CALENDARIO <= fechaFin &&
                                    x.VALOR_TRANSACCION > 7500 && x.VALOR_TRANSACCION < 10000)
                             .ToList();

                return primas;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePrimasSuperioresA7500  - " + e);
                throw new Exception("Error en generar reporte de : ReportePrimasSuperioresA7500");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de primas y complementos iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_REPORTE_PRIMAS> ReportePrimasYComplementosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_REPORTE_PRIMAS.AsNoTracking()
                                   .Where(x =>
                                        x.FECHA_CALENDARIO >= fechaIni &&
                                        x.FECHA_CALENDARIO <= fechaFin &&
                                        x.VALOR_TRANSACCION >= 10000)
                                   .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePrimasYComplementosIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePrimasYComplementosIgualesSuperioresA10000");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE VENTAS"


        /// <summary>
        /// Metodo en el que se genera reporte de ventas al contado de activos extraordinarios 
        /// iguales o superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_REPORTE_VENTAS> ReporteVentasIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var ventas = _SQLBDEntities.VIEW_REPORTE_VENTAS.AsNoTracking()
                             .Where(x =>
                                    x.FECHA_HORA >= fechaIni &&
                                    x.FECHA_HORA <= fechaFin &&
                                    x.PRECIO_VENTA >= 10000)
                             .ToList();

                return ventas;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteVentasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteVentasIgualesSuperioresA10000");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE TRANSACCIONES"


        /// <summary>
        /// Metodo en el que se genera reporte de todas las transacciones en un 
        /// rango de fechas determinado
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReporteTransacciones(string fechaInicial, string fechaFinal)
        {
            try
            {
                // Intentar parsear las fechas de forma segura
                if (!DateTime.TryParse(fechaInicial, out DateTime fi) || !DateTime.TryParse(fechaFinal, out DateTime ff))
                {
                    log.Error("Error en el formato de fecha");
                    throw new Exception("Formato de fecha inválido");
                }

                // Asegurar que la fecha final incluya todo el día
                ff = ff.Date.AddDays(1).AddTicks(-1);
                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                    .Where(x =>
                         x.FECHA_CALENDARIO.HasValue &&
                         x.FECHA_CALENDARIO.Value >= fi &&
                         x.FECHA_CALENDARIO.Value <= ff)
                    .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteTransacciones  - " + e);
                throw new Exception("Error en generar reporte de : ReporteTransacciones");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE OPERACIONES EN EFECTIVO"


        /// <summary>
        /// Metodo en el que se genera reporte de operaciones individuales en efectivo iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReporteOperacionesIndividualesEfectivoIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                   .Where(x =>
                                        x.FECHA_CALENDARIO.Value >= fechaIni &&
                                        x.FECHA_CALENDARIO.Value <= fechaFin &&
                                        x.VALOR_TRANSACCION >= 10000)
                                   .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteOperacionesIndividualesEfectivoIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteOperacionesIndividualesEfectivoIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de operaciones acumuladas en efectivo iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReporteOperacionesAcumuladasEfectivoIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<String> listSecuencia = new List<string>();

                var listIds = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                          .Where(x =>
                              x.FECHA_CALENDARIO.Value >= fechaIni &&
                              x.FECHA_CALENDARIO.Value <= fechaFin &&
                              x.VALOR_TRANSACCION < 10000)
                          .GroupBy(x => x.NUMERO_PRODUCTO)
                          .Select(x => new
                          {
                              MONTO = x.Sum(y => y.VALOR_TRANSACCION),
                              TOTAL_PÁGOS = x.Count(),
                              ListIds = x.Select(ids => ids.SECUENCIA).ToList()
                          })
                          .Where(x => x.TOTAL_PÁGOS >= 2 && x.MONTO >= 10000)
                          .Select(x => x.ListIds)
                          .ToList();

                foreach (List<String> item in listIds)
                    listSecuencia.AddRange(item);

                var transacciones = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                    .Where(x => listSecuencia.Contains(x.SECUENCIA))
                                    .ToList();

                return transacciones;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteOperacionesAcumuladasEfectivoIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteOperacionesAcumuladasEfectivoIgualesSuperioresA10000");
            }
        }


        #endregion


        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTES DE OPERACIONES"


        /// <summary>
        /// Metodo que genera reporte de operaciones acumuladas iguales o 
        /// superiores a $10,000
        /// </summary>
        /// <param name="codigoCliente">Código de cliente</param>
        /// <param name="totalPagos">Total de pagos</param>
        /// <param name="monto">Total de monto</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_CLIENTE> ReporteOperacionesAcumuladasIgualesSuperioresA10000(decimal codigoCliente, int totalPagos, decimal monto)
        {
            try
            {
                var direccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                                .Where(u => u.CODIGO_CLIENTE == codigoCliente && u.DIRECCION_PRINCIPAL == "S")
                                .Select(u => new
                                {
                                    DOMICILIO = u.PAIS + ", " + u.DEPARTAMENTO + ", " + u.MUNICIPIO + ", " + u.URBANIZACION
                                }).Take(1).ToList();


                if (!direccion.Any())
                    direccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                                 .Where(u => u.CODIGO_CLIENTE == codigoCliente)
                                 .Select(u => new
                                 {
                                     DOMICILIO = u.PAIS + ", " + u.DEPARTAMENTO + ", " + u.MUNICIPIO + ", " + u.URBANIZACION
                                 }).Take(1).ToList();


                string stringDireccion = string.Join("", direccion.Select(u => u.DOMICILIO));

                var resultado = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                                .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                                .AsEnumerable()
                                .Select(x => new VIEW_CLIENTE
                                {
                                    CODIGO_CLIENTE = x.CODIGO_CLIENTE,
                                    TIPO_IDENTIFICACION_1 = x.TIPO_IDENTIFICACION_1,
                                    NUMERO_IDENTIFICACION_1 = x.NUMERO_IDENTIFICACION_1,
                                    TIPO_IDENTIFICACION_2 = x.TIPO_IDENTIFICACION_2,
                                    NUMERO_IDENTIFICACION_2 = x.NUMERO_IDENTIFICACION_2,
                                    CODIGO_TIPO_CLIENTE = x.CODIGO_TIPO_CLIENTE,
                                    TIPO_CLIENTE = x.TIPO_CLIENTE,
                                    CODIGO_PAIS = x.CODIGO_PAIS,
                                    PAIS = x.PAIS,
                                    NACIONALIDAD = x.NACIONALIDAD,
                                    CODIGO_SECTOR_ECONOMICO = x.CODIGO_SECTOR_ECONOMICO,
                                    SECTOR_ECONOMICO = x.SECTOR_ECONOMICO,
                                    CODIGO_ACTIVIDAD_ECONOMICA = x.CODIGO_ACTIVIDAD_ECONOMICA,
                                    ACTIVIDAD_ECONOMICA = x.ACTIVIDAD_ECONOMICA,
                                    CODIGO_CLASE_ACTIVIDAD_ECONOMICA = x.CODIGO_CLASE_ACTIVIDAD_ECONOMICA,
                                    CLASE_ACTIVIDAD_ECONOMICA = x.CLASE_ACTIVIDAD_ECONOMICA,
                                    CODIGO_SUB_ACTIVIDAD_ECONOMICA = x.CODIGO_SUB_ACTIVIDAD_ECONOMICA,
                                    SUB_ACTIVIDAD_ECONOMICA = x.SUB_ACTIVIDAD_ECONOMICA,
                                    TIPO_DE_PERSONA = x.TIPO_DE_PERSONA,
                                    NOMBRES = x.NOMBRES,
                                    PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                                    SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                                    APELLIDO_DE_CASADA = x.APELLIDO_DE_CASADA,
                                    SEXO = x.SEXO,
                                    FECHA_DE_NACIMIENTO = x.FECHA_DE_NACIMIENTO,
                                    CODIGO_ESTADO_CIVIL = x.CODIGO_ESTADO_CIVIL,
                                    ESTADO_CIVIL = x.ESTADO_CIVIL,
                                    TOTAL_INGRESOS = x.TOTAL_INGRESOS,
                                    CODIGO_PROFESION = x.CODIGO_PROFESION,
                                    PROFESION = x.PROFESION,
                                    CODIGO_OCUPACION = x.CODIGO_OCUPACION,
                                    OCUPACION = x.OCUPACION,
                                    CODIGO_EMPRESA = x.CODIGO_EMPRESA,
                                    CONOCIDO_COMO = x.CONOCIDO_COMO,
                                    ALIAS = x.ALIAS,
                                    TOTAL_TRANSACCIONES = totalPagos,
                                    TOTAL_MONTO = monto,
                                    DOMICILIO = stringDireccion
                                }).ToList();

                return resultado;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteOperacionesAcumuladasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteOperacionesAcumuladasIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo que obtiene la información del cliente  de la
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="codigoCliente">Código de cliente</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_CLIENTE> ReporteClienteOperacionesDiariasIgualesSuperioresA10000(decimal codigoCliente)
        {
            try
            {
                var direccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                                .Where(u => u.CODIGO_CLIENTE == codigoCliente && u.DIRECCION_PRINCIPAL == "S")
                                .Select(u => new
                                {
                                    DOMICILIO = u.PAIS + ", " + u.DEPARTAMENTO + ", " + u.MUNICIPIO + ", " + u.URBANIZACION
                                }).Take(1).ToList();


                if (!direccion.Any())
                    direccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                                 .Where(u => u.CODIGO_CLIENTE == codigoCliente)
                                 .Select(u => new
                                 {
                                     DOMICILIO = u.PAIS + ", " + u.DEPARTAMENTO + ", " + u.MUNICIPIO + ", " + u.URBANIZACION
                                 }).Take(1).ToList();

                string stringDireccion = direccion.FirstOrDefault()?.DOMICILIO ?? "";

                var datos = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                                .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                                .ToList();

                var resultado = datos.Select(x => new VIEW_CLIENTE
                {
                    CODIGO_CLIENTE = x.CODIGO_CLIENTE,
                    TIPO_IDENTIFICACION_1 = x.TIPO_IDENTIFICACION_1,
                    NUMERO_IDENTIFICACION_1 = x.NUMERO_IDENTIFICACION_1,
                    TIPO_IDENTIFICACION_2 = x.TIPO_IDENTIFICACION_2,
                    NUMERO_IDENTIFICACION_2 = x.NUMERO_IDENTIFICACION_2,
                    CODIGO_TIPO_CLIENTE = x.CODIGO_TIPO_CLIENTE,
                    TIPO_CLIENTE = x.TIPO_CLIENTE,
                    CODIGO_PAIS = x.CODIGO_PAIS,
                    PAIS = x.PAIS,
                    NACIONALIDAD = x.NACIONALIDAD,
                    CODIGO_SECTOR_ECONOMICO = x.CODIGO_SECTOR_ECONOMICO,
                    SECTOR_ECONOMICO = x.SECTOR_ECONOMICO,
                    CODIGO_ACTIVIDAD_ECONOMICA = x.CODIGO_ACTIVIDAD_ECONOMICA,
                    ACTIVIDAD_ECONOMICA = x.ACTIVIDAD_ECONOMICA,
                    CODIGO_CLASE_ACTIVIDAD_ECONOMICA = x.CODIGO_CLASE_ACTIVIDAD_ECONOMICA,
                    CLASE_ACTIVIDAD_ECONOMICA = x.CLASE_ACTIVIDAD_ECONOMICA,
                    CODIGO_SUB_ACTIVIDAD_ECONOMICA = x.CODIGO_SUB_ACTIVIDAD_ECONOMICA,
                    SUB_ACTIVIDAD_ECONOMICA = x.SUB_ACTIVIDAD_ECONOMICA,
                    TIPO_DE_PERSONA = x.TIPO_DE_PERSONA,
                    NOMBRES = x.NOMBRES,
                    PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                    SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                    APELLIDO_DE_CASADA = x.APELLIDO_DE_CASADA,
                    SEXO = x.SEXO,
                    FECHA_DE_NACIMIENTO = x.FECHA_DE_NACIMIENTO,
                    CODIGO_ESTADO_CIVIL = x.CODIGO_ESTADO_CIVIL,
                    ESTADO_CIVIL = x.ESTADO_CIVIL,
                    TOTAL_INGRESOS = x.TOTAL_INGRESOS,
                    CODIGO_PROFESION = x.CODIGO_PROFESION,
                    PROFESION = x.PROFESION,
                    CODIGO_OCUPACION = x.CODIGO_OCUPACION,
                    OCUPACION = x.OCUPACION,
                    CODIGO_EMPRESA = x.CODIGO_EMPRESA,
                    CONOCIDO_COMO = x.CONOCIDO_COMO,
                    ALIAS = x.ALIAS,
                    DOMICILIO = stringDireccion
                }).ToList();


                return resultado;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteClienteOperacionesDiariasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteClienteOperacionesDiariasIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo que obtiene la información de la  
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="secuencia">Código que identifica a cada transacción</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000(string secuencia)
        {
            try
            {
                var transaccion = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                                  .Where(x => x.SECUENCIA == secuencia)
                                  .Take(1)
                                  .ToList();

                var info = _SQLBDEntities.ALE_DATOS_ADICIONALES_TRANSACCION
                                    .AsNoTracking()
                                    .FirstOrDefault(x => x.SECUENCIA == secuencia);

                //left join
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO.AsNoTracking().ToList();
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO.AsNoTracking().ToList();

                var lista = transaccion
                    .Select(t => new VIEW_TRANSACCIONES
                        {
                            FECHA_CALENDARIO = t.FECHA_CALENDARIO,
                            NUMERO_PRODUCTO = t.NUMERO_PRODUCTO,
                            TIPO_TRANSACCION = t.TIPO_TRANSACCION,
                            VALOR_TRANSACCION = t.VALOR_TRANSACCION,
                            CLASE_PRODUCTO = t.CLASE_PRODUCTO,
                            NUMERO_RECIBO = t.NUMERO_RECIBO,
                            AGENCIA_BANCO = t.CODIGO_FINANCIERA,
                            ORIGEN_FONDOS = info?.TRAN_ORIGEN_FONDOS ?? "",
                            CODIGO_CAJERO = info?.TRAN_CODIGO_CAJERO ?? "",
                            DEPARTAMENTO = info?.TRAN_DEPARTAMENTO != null ?
                               departamentos.FirstOrDefault(x => x.CODIGO_DEPARTAMENTO == info.TRAN_DEPARTAMENTO)?.NOMBRE : "",
                            MUNICIPIO = info?.TRAN_MUNICIPIO != null ?
                                municipios.FirstOrDefault(x => x.CODIGO_MUNICIPIO == info.TRAN_MUNICIPIO)?.NOMBRE : ""
                        
                    }).ToList();

                return lista;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000");
            }
        }


        /// <summary>
        /// Metodo que obtiene la información adicional de la  
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="factura">Numero de factura</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<ALE_DATOS_ADICIONALES_TRANSACCION> ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000(string secuencia)
        {
            try
            {
                var datos = _SQLBDEntities.ALE_DATOS_ADICIONALES_TRANSACCION.AsNoTracking()
                             .Where(x => x.SECUENCIA == secuencia)
                             .Take(1)
                             .ToList();

                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO
                                      .Where(d => d.CODIGO_PAIS == 1)
                                      .ToList();

                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                                    .Where(m => m.CODIGO_PAIS == 1)
                                    .ToList();


                var infoAdicional = datos.Select( x=> new ALE_DATOS_ADICIONALES_TRANSACCION
                {
                                       ID = x.ID,
                                       SECUENCIA = x.SECUENCIA,
                                       PERT_NOMBRE = x.PERT_NOMBRE,
                                       PERT_PRIMER_APELLIDO = x.PERT_PRIMER_APELLIDO,
                                       PERT_SEGUNDO_APELLIDO = x.PERT_SEGUNDO_APELLIDO,
                                       PERT_APELLIDO_CASADA = x.PERT_APELLIDO_CASADA,
                                       PERT_DUI = x.PERT_DUI,
                                       PERT_NIT = x.PERT_NIT,
                                       PERT_NACIONALIDAD = x.PERT_NACIONALIDAD,
                                       PERT_DEPARTAMENTO = x.PERT_DEPARTAMENTO,
                                       PERT_DEPARTAMENTO_NOMBRE = x.PERT_DEPARTAMENTO != null
                                            ? departamentos.FirstOrDefault(d => d.CODIGO_DEPARTAMENTO == x.PERT_DEPARTAMENTO)?.NOMBRE ?? ""
                                            : "",
                                       PERT_MUNICIPIO = x.PERT_MUNICIPIO,
                                       PERT_MUNICIPIO_NOMBRE = x.PERT_MUNICIPIO != null
                                            ? municipios.FirstOrDefault(m => m.CODIGO_DEPARTAMENTO == x.PERT_DEPARTAMENTO && m.CODIGO_MUNICIPIO == x.PERT_MUNICIPIO)?.NOMBRE ?? ""
                                            : "",
                                       PERT_DOMICILIO = x.PERT_DOMICILIO,
                                       PERT_FECHA_NACIMIENTO = x.PERT_FECHA_NACIMIENTO,
                                       PERT_LUGAR_NACIMIENTO = x.PERT_LUGAR_NACIMIENTO,
                                       PERT_ESTADO_CIVIL = x.PERT_ESTADO_CIVIL,
                                       PERT_PROFESION = x.PERT_PROFESION,
                                       TRAN_CODIGO_CAJERO = x.TRAN_CODIGO_CAJERO,
                                       TRAN_DEPARTAMENTO = x.TRAN_DEPARTAMENTO,
                                       TRAN_DEPARTAMENTO_NOMBRE = x.TRAN_DEPARTAMENTO != null
                                            ? departamentos.FirstOrDefault(d => d.CODIGO_DEPARTAMENTO == x.TRAN_DEPARTAMENTO)?.NOMBRE ?? ""
                                            : "",
                                       TRAN_MUNICIPIO = x.TRAN_MUNICIPIO,
                                       TRAN_MUNICIPIO_NOMBRE = x.TRAN_MUNICIPIO != null
                                            ? municipios.FirstOrDefault(m => m.CODIGO_DEPARTAMENTO == x.TRAN_DEPARTAMENTO && m.CODIGO_MUNICIPIO == x.TRAN_MUNICIPIO)?.NOMBRE ?? ""
                                            : "",
                                       TRAN_ORIGEN_FONDOS = x.TRAN_ORIGEN_FONDOS,
                                       INFO_DATOS_RECIBO = x.INFO_DATOS_RECIBO,
                                       INFO_CALIDAD_IMPRESION = x.INFO_CALIDAD_IMPRESION,
                                       INFO_LLENO_DECLARACION = x.INFO_LLENO_DECLARACION,
                                       INFO_MODALIDAD_PAGO = x.INFO_MODALIDAD_PAGO,
                                       INFO_VERSION_DECLARACION_JURADA = x.INFO_VERSION_DECLARACION_JURADA,
                                       INFO_QUIEN_LLENO_DECLARACION_JURADA = x.INFO_QUIEN_LLENO_DECLARACION_JURADA,
                                       INFO_DOCUMENTACION_REQUERIDA = x.INFO_DOCUMENTACION_REQUERIDA,
                                       INFO_TELEFONO_CONTACTO = x.INFO_TELEFONO_CONTACTO,
                                       INFO_SEGUIMIENTO = x.INFO_SEGUIMIENTO,
                                       CLIE_LUGAR_NACIMIENTO = x.CLIE_LUGAR_NACIMIENTO,
                                       CLIE_DEPARTAMENTO = x.CLIE_DEPARTAMENTO,
                                       CLIE_DEPARTAMENTO_NOMBRE = x.CLIE_DEPARTAMENTO != null
                                            ? departamentos.FirstOrDefault(d => d.CODIGO_DEPARTAMENTO == x.CLIE_DEPARTAMENTO)?.NOMBRE ?? ""
                                            : "",
                                       CLIE_MUNICIPIO = x.CLIE_MUNICIPIO,
                                       CLIE_MUNICIPIO_NOMBRE = x.CLIE_MUNICIPIO != null
                                            ? municipios.FirstOrDefault(m => m.CODIGO_DEPARTAMENTO == x.CLIE_DEPARTAMENTO && m.CODIGO_MUNICIPIO == x.CLIE_MUNICIPIO)?.NOMBRE ?? ""
                                            : ""
                                    })
                                   .ToList();

                return infoAdicional;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000");
            }
        }


        #endregion


        #region "++++++++++++++++++++++ REPORTE ROS ++++++++++++++++++++++++++++"


        public List<MON_ROS> Ros(long ID_ROS)
        {
            try
            {

                var ros = _SQLBDEntities.MON_ROS
                    .AsNoTracking()
                    .Where(u => u.ID == ID_ROS)
                    .ToList(); // 1️⃣ Traer a memoria

                // 2️⃣ Proyectar en memoria (opcional, si querés limitar campos)
                var resultado = ros.Select(u => new MON_ROS
                {
                    CODIGO_UIF = u.CODIGO_UIF,
                    FECHA_ELABORACION = u.FECHA_ELABORACION,
                    DESCRIPCION_OPERACION_REPORTADA = u.DESCRIPCION_OPERACION_REPORTADA,
                    ANALISIS_CASO = u.ANALISIS_CASO
                }).ToList();

                return resultado;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : Ros  - " + e);
                throw new Exception("Error en generar reporte de : Ros");
            }
        }


        public List<MON_ROS_ACTOR> actorRos(long ID_ROS)
        {
            try
            {
                var actores = _SQLBDEntities.MON_ROS_ACTOR
                    .Where(x => x.ID_ROS == ID_ROS)
                    .ToList(); // Primero traemos a memoria solo los actores necesarios

                var estadoCivil = _SQLBDEntities.VIEW_ESTADO_CIVIL.ToList();
                var actividades = _SQLBDEntities.VIEW_ACTIVIDAD_ECONOMICA.ToList();
                var nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO.ToList();
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO.ToList();
                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION.ToList();
                var documentos = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.ToList();
                var profesiones = _SQLBDEntities.VIEW_PROFESIONES.ToList();

                var sujeto = actores.Select(x => new MON_ROS_ACTOR
                {
                    ID_ROS = x.ID_ROS,
                    RELACION_CON_SO = x.RELACION_CON_SO,
                    FECHA_NACIMIENTO = x.FECHA_NACIMIENTO,
                    PRIMER_NOMBRE = x.PRIMER_NOMBRE,
                    SEGUNDO_NOMBRE = x.SEGUNDO_NOMBRE,
                    PRIMER_APELLIDO = x.PRIMER_APELLIDO,
                    SEGUNDO_APELLIDO = x.SEGUNDO_APELLIDO,
                    APELLIDO_CASADA = x.APELLIDO_CASADA,
                    TIPO_PERSONA = x.TIPO_PERSONA,
                    GENERO = x.GENERO,
                    ID_ESTADO_FAMILIAR = x.ID_ESTADO_FAMILIAR,
                    CIVIL = estadoCivil.FirstOrDefault(e => e.CODIGO == x.ID_ESTADO_FAMILIAR)?.DESCRIPCION,
                    ID_ACTIVIDAD_ECONOMICA = x.ID_ACTIVIDAD_ECONOMICA,
                    ECONOMICA = actividades.FirstOrDefault(a => a.CODIGO_ACTIVIDAD_ECONOMICA == x.ID_ACTIVIDAD_ECONOMICA)?.DESCRIPCION,
                    ID_NACIONALIDAD = x.ID_NACIONALIDAD,
                    NACIONALIDAD = nacionalidades.FirstOrDefault(n => n.CODIGO_PAIS == x.ID_NACIONALIDAD)?.NACIONALIDAD,
                    ID_PAIS = x.ID_PAIS,
                    PAIS = nacionalidades.FirstOrDefault(n => n.CODIGO_PAIS == x.ID_PAIS)?.NOMBRE,
                    ID_DEPARTAMENTO = x.ID_DEPARTAMENTO,
                    DEPARTAMENTO = departamentos.FirstOrDefault(d => d.CODIGO_DEPARTAMENTO == x.ID_DEPARTAMENTO)?.NOMBRE,
                    ID_MUNICIPIO = x.ID_MUNICIPIO,
                    MUNICIPIO = municipios.FirstOrDefault(m => m.CODIGO_MUNICIPIO == x.ID_MUNICIPIO)?.NOMBRE,
                    ID_URBANIZACION = x.ID_URBANIZACION,
                    URBANIZACION = urbanizaciones.FirstOrDefault(u => u.CODIGO_SECTOR == x.ID_URBANIZACION)?.DESCRIPCION,
                    ID_TIPO_DOCUMENTO = x.ID_TIPO_DOCUMENTO,
                    TDOCUMENTO = documentos.FirstOrDefault(t => t.CODIGO_TIPO_IDENTIFICACION == x.ID_TIPO_DOCUMENTO)?.DESCRIPCION,
                    ID_PROFESION = x.ID_PROFESION,
                    PROFESION = profesiones.FirstOrDefault(p => p.CODIGO_PROFESION == x.ID_PROFESION)?.DESCRIPCION,
                    NUMERO_DOCUMENTO = x.NUMERO_DOCUMENTO,
                    DIRECCION_ESPECIFICA = x.DIRECCION_ESPECIFICA
                }).ToList();

                return sujeto;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : Ros  - " + e);
                throw new Exception("Error en generar reporte de : Ros");
            }
        }


        public List<MON_ROS_OPERACION> operacionRos(long ID_ROS)
        {
            try
            {
                var operacionesRaw = _SQLBDEntities.MON_ROS_OPERACION
                    .AsNoTracking()
                    .Include("MON_AGENCIA_BANCARIA.MON_CAT_FINANCIERA")
                    .Where(s => s.ID_ROS == ID_ROS)
                    .ToList(); // Traer todo a memoria

                var sospecha = operacionesRaw.Select(s => new MON_ROS_OPERACION
                {
                    ID_ROS = s.ID_ROS,
                    ID_TRANSACCION = s.ID_TRANSACCION,
                    ID_AGENCIA_BANCO = s.ID_AGENCIA_BANCO,
                    AGENCIABANCO = s.MON_AGENCIA_BANCARIA?.DESCRIPCION,
                    BANCO = s.MON_AGENCIA_BANCARIA?.MON_CAT_FINANCIERA?.DESCRIPCION,
                    FECHA_OPERACION = s.FECHA_OPERACION,
                    NOMBRE_CLIENTE = s.NOMBRE_CLIENTE,
                    NUMERO_PRESTAMO = s.NUMERO_PRESTAMO,
                    VALOR_PAGADO = s.VALOR_PAGADO,
                    ID_PAIS = s.ID_PAIS,
                    ID_DEPARTAMENTO = s.ID_DEPARTAMENTO,
                    ID_MUNICIPIO = s.ID_MUNICIPIO,
                    DIRECCION_FINANCIERA = s.DIRECCION_FINANCIERA,
                    PROCEDENCIA_DESTINO = s.PROCEDENCIA_DESTINO,
                    RAZON_ROS = s.RAZON_ROS,
                    ID_TIPO_DOCUMENTO = s.ID_TIPO_DOCUMENTO,
                    NUMERO_DOCUMENTO = s.NUMERO_DOCUMENTO,
                    ID_TIPO_PRODUCTO = s.ID_TIPO_PRODUCTO
                }).ToList();

                return sospecha;
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : Ros - " + e.Message + " | Inner: " + e.InnerException?.Message);
                throw new Exception("Error en generar reporte de : Ros", e);
            }
        }


        #endregion


    }
}
