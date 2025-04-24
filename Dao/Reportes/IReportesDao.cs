using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Reportes
{
    public interface IReportesDao
    {

        /// <summary>
        /// Metodo que permite obtener una lista de ids de matriz filtrado por
        /// un rango de fechas.
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de ids de Matriz</returns>
        List<long> idsMatrizPorRangoFecha(DateTime fechaInicial, DateTime fechaFinal);

        /// <summary>
        /// Metodo que permite generar reporte para matriz de riesgo por rango de fechas
        /// y agencias las cuales pueden ser una ó todas las agencias
        /// </summary>
        /// <param name="idsMatriz">Lista de ids de matriz</param>
        /// <param name="idAgencia">Identificador de CAT_AGENCIA</param>
        /// <returns>Lista de controles</returns>
        List<MAT_MATRIZ_CONTROL> ReporteMatriz_Controles(List<long> idsMatriz, int idAgencia);

        /// <summary>
        /// Metodo que permite generar reporte para matriz de riesgo por rango de fechas
        /// y agencias las cuales pueden ser una ó todas las agencias
        /// </summary>
        /// <param name="idsMatriz">Lista de ids de matriz</param>
        /// <param name="idAgencia">Identificador de CAT_AGENCIA</param>
        /// <returns>Lista de eventos</returns>
        List<MAT_MATRIZ_EVENTO_RIESGO> ReporteMatriz_Eventos(List<long> idsMatriz, int idAgencia);
        
        /// <summary>
        /// Metodo en el que genera reporte de resumen de requerimiento de información
        /// ó información de OFICIOS - se muestran información de los oficios que ya estan procesados
        /// y se agrega un filtro de fechas para luego obtener todas las personas de estos OFICIOS
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<MON_OFICIO_PERSONA> ReporteInformacionOficio(DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Metodo en el que se genera reporte de respuestas de oficios procesados, filtrados por 
        /// un rango de fechas, además la informaión de los OFICIOS esta filtrada por la unidad 
        /// a la cual se enviará la información
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="idUnidad">Identificador único de CAT_UNIDAD_INSTITUCION</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<MON_OFICIO> ReporteRespuestaOficios(DateTime fechaIni, DateTime fechaFin, int idUnidad);

        /// <summary>
        /// Metodo en el que se genera reporte de alertas procesadas,
        /// las cuales estan filtradas por un rango de fechas
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="tipoAlerta">Identificador de tipo de alerta</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<ALE_ALERTA> ReporteAlertas(DateTime fechaIni, DateTime fechaFin, int tipoAlerta);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista de la ONU 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_ONU> ReporteListaONU();

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista ONU</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_ONU> ReporteListaONUDetalle(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista ONU</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_ONU_DOCUMENTO> ReporteListaONUDetalleDocumento(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista ONU</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_ONU_DIRECCION> ReporteListaONUDetalleDireccion(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista de la ONU
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista ONU</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_ONU_ALIAS> ReporteListaONUDetalleAlias(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista SDN de la OFAC 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN> ReporteListaSDN();

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista SDN</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN> ReporteListaSDNDetalle(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista SDN</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN_DOCUMENTO> ReporteListaSDNDetalleDocumento(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista SDN</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN_DIRECCION> ReporteListaSDNDetalleDireccion(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista SDN</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN_AKA> ReporteListaSDNDetalleAlias(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en la lista SDN
        /// </summary>
        /// <param name="idPersona">ID de la Persona en la lista SDN</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_SDN_NACIONALIDAD> ReporteListaSDNDetalleNacionalidad(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en una lista personalizada
        /// </summary>    
        /// <param name="idLista">ID de la Lista Personalizada</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PERSONALIZADA> ReporteListaPersonalizada(int idLista);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o más listas personalizadas
        /// </summary>    
        /// <param name="idPersona">ID de la Persona de la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PERSONALIZADA> ReporteListaPersonalizadaDetalle(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o más listas personalizadas
        /// </summary>    
        /// <param name="idPersona">ID de la Persona de la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PERSONALIZADA_DOCUMENTO> ReporteListaPersonalizadaDetalleDocumento(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o más listas personalizadas
        /// </summary>    
        /// <param name="idPersona">ID de la Persona de la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PERSONALIZADA_DIRECCION> ReporteListaPersonalizadaDetalleDireccion(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o más listas personalizadas
        /// </summary>    
        /// <param name="idPersona">ID de la Persona de la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PERSONALIZADA_ALIAS> ReporteListaPersonalizadaDetalleAlias(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona listada en una o más listas personalizadas
        /// </summary>    
        /// <param name="idPersona">ID de la Persona de la lista</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_GENERAL> ReporteListaPersonalizadaDetalleLista(int idPersona);

        /// <summary>
        /// Metodo en el que se genera reporte de personas listadas en la lista PEP 
        /// </summary>           
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PEP> ReporteListaPEP();

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP 
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PEP> ReporteListaPEPDetalle(int idPEP);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP 
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PEP_CARGO> ReporteListaPEPDetalleCargo(int idPEP);

        /// <summary>
        /// Metodo en el que se genera reporte detalle de una persona en la lista PEP 
        /// </summary>
        /// <param name="idPEP">ID del PEP</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<LIS_PEP_RELACION> ReporteListaPEPDetalleRelacion(int idPEP);

        /// <summary>
        /// Metodo en el que se genera reporte de personas incluidas en oficios
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<MON_OFICIO_PERSONA> ReporteListaOficio(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios entre $573.43 y $5,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_REPORTE_CONTROLPAGOS> ReportePagosDiariosIgualesSuperiores571_5000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $5,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_REPORTE_CONTROLPAGOS> ReportePagosDiariosIgualesSuperioresA5000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos acumulados iguales ó superiores a $25,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_TRANSACCIONES> ReportePagosAcumuladosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de pagos diarios iguales ó superiores a $25,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_TRANSACCIONES> ReportePagosDiariosIgualesSuperioresA25000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente 
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_CLIENTE> ReporteResumenCliente(decimal codigoCliente);

        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente 
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_CLIENTE_DIRECCIONES> ReporteResumenClienteDirecciones(decimal codigoCliente);

        /// <summary>
        /// Metodo en el que se genera reporte resumen de un cliente
        /// </summary>
        /// <param name="codigoCliente">Código del cliente</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_CLIENTE_TELEFONOS> ReporteResumenClienteTelefonos(decimal codigoCliente);

        /// <summary>
        /// Metodo en el que se genera reporte de primas y complementos iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_REPORTE_PRIMAS> ReportePrimasYComplementosIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de operaciones individuales en efectivo iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_TRANSACCIONES> ReporteOperacionesIndividualesEfectivoIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo que genera el reporte de Alertas por cliente
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <returns>DEVUELVE LA LISTA DE ALERTAS DEL CLIENTE</returns>
        List<ALE_ALERTA> ReporteAlertasCliente(decimal codigoCliente);

        /// <summary>
        /// Metodo en el que se genera reporte de primas y complementos superiores a $7,700.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_REPORTE_PRIMAS> ReportePrimasSuperioresA7500(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo en el que se genera reporte de ventas al contado de activos extraordinarios 
        /// iguales o superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_REPORTE_VENTAS> ReporteVentasIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo que genera el reporte de transacciones por cliente
        /// </summary>
        /// <param name="FECHA_INI_T"></param>
        /// <param name="FECHA_FIN_T"></param>
        /// <param name="codigoCliente"></param>
        /// <param name="NOMBRE_T"></param>
        /// <returns>Lista de transacciones realizadas por el cliente indicado</returns>
        List<VIEW_TRANSACCIONES> ReporteTransaccionesCliente(string FECHA_INI_T, string FECHA_FIN_T, decimal codigoCliente, string NOMBRE_T);

        /// <summary>
        /// Metodo que genera el reporte de transacciones en un rango de fecha dado
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns>Lista de transacciones por rango de fecha</returns>
        List<VIEW_TRANSACCIONES> ReporteTransacciones(string fechaInicial, string fechaFinal);

        /// <summary>
        /// Metodo en el que se genera reporte de operaciones acumuladas en efectivo iguales ó superiores a $10,000.00 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista dinámica de información requerida para el reporte</returns>
        List<VIEW_TRANSACCIONES> ReporteOperacionesAcumuladasEfectivoIgualesSuperioresA10000(DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Metodo que genera reporte de operaciones acumuladas iguales o 
        /// superiores a $10,000
        /// </summary>
        /// <param name="codigoCliente">Código de cliente</param>
        /// <param name="totalPagos">Total de pagos</param>
        /// <param name="monto">Total de monto</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_CLIENTE> ReporteOperacionesAcumuladasIgualesSuperioresA10000(decimal codigoCliente, int totalPagos, decimal monto);

        /// <summary>
        /// Metodo que obtiene la información del cliente  de la
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="codigoCliente">Código de cliente</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_CLIENTE> ReporteClienteOperacionesDiariasIgualesSuperioresA10000(decimal codigoCliente);

        /// <summary>
        /// Metodo que obtiene la información de la  
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="secuencia">Código que identifica a cada transacción</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<VIEW_TRANSACCIONES> ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000(string secuencia);

        /// <summary>
        /// Metodo que obtiene la información adicional de la  
        /// operación diaria igual o superior a $10,000
        /// </summary>
        /// <param name="factura">Numero de factura</param>
        /// <returns>Lista dinámica de información requerida para el reprote</returns>
        List<ALE_DATOS_ADICIONALES_TRANSACCION> ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000(string factura);
        /// <summary>
        /// Metodo para generar reporte ROS
        /// </summary>
        /// <param name="ID_ROS"></param>
        /// <returns></returns>
        List<MON_ROS> Ros(long ID_ROS);
        List<MON_ROS_ACTOR> actorRos(long ID_ROS);
        List<MON_ROS_OPERACION> operacionRos(long ID_ROS);

    }
}
