using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dao.Reportes;
using log4net;
using Model;
using Ninject;

namespace Blo.Reportes
{
    public class ReportesBlo : IReportesBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IReportesDao _reportesDao;


        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ReportesBlo(IReportesDao reportesDao)
        {
            _reportesDao = reportesDao;
        }


        /// <summary>
        /// Propiedad para administrar el mecanismo de logeo 
        /// de informacion y fallas
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);



        #region "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ REPORTE DE MATRIZ DE RIESGO"



        /// <summary>
        /// Metodo que permite obtener una lista de ids de matriz filtrado por
        /// un rango de fechas.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de ids de Matriz</returns>
        public List<long> idsMatrizPorRangoFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                return _reportesDao.idsMatrizPorRangoFecha(fechaInicial, fechaFinal);
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
                return _reportesDao.ReporteMatriz_Controles(idsMatriz, idAgencia);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Error al obtener controles para reporte de matriz de riesgo", e);
            }
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
                return _reportesDao.ReporteMatriz_Eventos(idsMatriz, idAgencia);
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
                return _reportesDao.ReporteInformacionOficio(fechaIni, fechaFin);
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
                return _reportesDao.ReporteRespuestaOficios(fechaIni, fechaFin, idUnidad);
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
                return _reportesDao.ReporteAlertas(fechaIni, fechaFin, tipoAlerta);
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
                return _reportesDao.ReportePagosDiariosIgualesSuperiores571_5000(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperiores571_5000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperiores571_5000");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return _reportesDao.ReportePagosDiariosIgualesSuperioresA10000(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA10000");
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
                return _reportesDao.ReportePagosDiariosIgualesSuperioresA5000(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA5000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA5000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return _reportesDao.ReportePagosAcumuladosIgualesSuperioresA10000(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosAcumuladosIgualesSuperioresA10000");
            }
        }

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $25,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return _reportesDao.ReportePagosDiariosIgualesSuperioresA25000(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA25000  - " + e);
                throw new Exception("Error en generar reporte de : ReportePagosDiariosIgualesSuperioresA25000");
            }
        }


        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $25,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        public List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return _reportesDao.ReportePagosAcumuladosIgualesSuperioresA25000(fechaIni, fechaFin);
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
                return _reportesDao.ReporteListaONU();
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
                return _reportesDao.ReporteListaONUDetalle(idPersona);
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
                return _reportesDao.ReporteListaONUDetalleDocumento(idPersona);
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
                return _reportesDao.ReporteListaONUDetalleDireccion(idPersona);
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
                return _reportesDao.ReporteListaONUDetalleAlias(idPersona);
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
                return _reportesDao.ReporteListaSDN();
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
                return _reportesDao.ReporteListaSDNDetalle(idPersona);
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
                return _reportesDao.ReporteListaSDNDetalleDocumento(idPersona);
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
                return _reportesDao.ReporteListaSDNDetalleDireccion(idPersona);
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
                return _reportesDao.ReporteListaSDNDetalleAlias(idPersona);
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
                return _reportesDao.ReporteListaSDNDetalleNacionalidad(idPersona);
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
                return _reportesDao.ReporteListaPersonalizada(idLista);
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
                return _reportesDao.ReporteListaPersonalizadaDetalle(idPersona);
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
                return _reportesDao.ReporteListaPersonalizadaDetalleDocumento(idPersona);
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
                return _reportesDao.ReporteListaPersonalizadaDetalleDireccion(idPersona);
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
                return _reportesDao.ReporteListaPersonalizadaDetalleAlias(idPersona);
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
                return _reportesDao.ReporteListaPersonalizadaDetalleLista(idPersona);
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
                return _reportesDao.ReporteListaPEP();
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
                return _reportesDao.ReporteListaPEPDetalle(idPEP);
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
                return _reportesDao.ReporteListaPEPDetalleCargo(idPEP);
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
                return _reportesDao.ReporteListaPEPDetalleRelacion(idPEP);
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
                return _reportesDao.ReporteListaOficio(fechaIni, fechaFin);
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
                return _reportesDao.ReporteResumenCliente(codigoCliente);
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
                return _reportesDao.ReporteResumenClienteDirecciones(codigoCliente);
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
                return _reportesDao.ReporteResumenClienteTelefonos(codigoCliente);
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
                return _reportesDao.ReporteTransaccionesCliente(FECHA_INI_T, FECHA_FIN_T, codigoCliente, NOMBRE_T);
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
                return _reportesDao.ReporteAlertasCliente(codigoCliente);
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
                return _reportesDao.ReportePrimasSuperioresA7500(fechaIni, fechaFin);
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
                return _reportesDao.ReportePrimasYComplementosIgualesSuperioresA10000(fechaIni, fechaFin);
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
                return _reportesDao.ReporteVentasIgualesSuperioresA10000(fechaIni, fechaFin);
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
                return _reportesDao.ReporteTransacciones(fechaInicial, fechaFinal);
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
                return _reportesDao.ReporteOperacionesIndividualesEfectivoIgualesSuperioresA10000(fechaIni, fechaFin);
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
                return _reportesDao.ReporteOperacionesAcumuladasEfectivoIgualesSuperioresA10000(fechaIni, fechaFin);
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
                return _reportesDao.ReporteOperacionesAcumuladasIgualesSuperioresA10000(codigoCliente, totalPagos, monto);
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
                return _reportesDao.ReporteClienteOperacionesDiariasIgualesSuperioresA10000(codigoCliente);
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
                return _reportesDao.ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000(secuencia);
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
                return _reportesDao.ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000(secuencia);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000  - " + e);
                throw new Exception("Error en generar reporte de : ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000");
            }
        }


        #endregion


        #region "++++++++++++++++++++++++++++++++++++ REPORTE ROS ++++++++++++++++++++++++++++++++++++++"

        public List<MON_ROS> Ros(long ID_ROS)
        {
            try
            {
                return _reportesDao.Ros(ID_ROS);
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
                return _reportesDao.actorRos(ID_ROS);
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
                return _reportesDao.operacionRos(ID_ROS);
            }
            catch (Exception e)
            {
                log.Error("Error en generar reporte de : Ros  - " + e);
                throw new Exception("Error en generar reporte de : Ros");
            }
        }

        #endregion

    }
}
