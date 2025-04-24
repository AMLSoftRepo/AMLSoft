using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public class AlertaDao : GenericDao<ALE_ALERTA>, IAlertaDao
    {
        /// <summary>
        /// Metodo que permite obtener una lista de alertas con estado de notificadas.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="estado">Estado actual de la alerta</param>
        /// <param name="tipoAlerta">Identificador unico de tipo alerta</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de alertas con estado de notificadas</returns>
        public IQueryable<dynamic> GetAlertasNotificadas(out int total, int? page, int? limit, string sortBy, string direction, string estado, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null)
        {
            try
            {
                SQLBDEntities _Entities = new SQLBDEntities();
                _Entities.Configuration.ProxyCreationEnabled = false;
                List<ALE_ALERTA> lista = new List<ALE_ALERTA>();
                bool filtroFechas = false;
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Comprobando que se filtrara por un rango de fechas
                filtroFechas = (fechaInicio != null && fechaFin != null);
                //Agregando 23 horas para evitar problemas al seleccionar la misma fecha
                fechaFin = (fechaFin == null ? fechaFin : fechaFin.Value.AddHours(23));

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            x.ESTADO_ACTUAL == estado &&
                            (x.DESCRIPCION + " " +
                              x.ID_CLIENTE + " " +
                              x.PRESTAMO + " " +
                              x.NUMERO_FACTURA
                           ).ToUpper().Contains(searchString.Trim().ToUpper())
                         )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            x.ESTADO_ACTUAL == estado &&
                            (x.DESCRIPCION + " " +
                              x.ID_CLIENTE + " " +
                              x.PRESTAMO + " " +
                              x.NUMERO_FACTURA
                           ).ToUpper().Contains(searchString.Trim().ToUpper())
                         )
                        .Count();

                }
                else
                {
                    lista = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            x.ESTADO_ACTUAL == estado
                         )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            x.ESTADO_ACTUAL == estado
                         )
                        .Count();
                }

                return lista.AsQueryable();

            }
            catch (Exception e)
            {
                log.Error("Error al obtener alertas con estado de notificada: " + e);
                throw new Exception("Error al obtener alertas con estado de notificada");
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de alertas en historial
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="tipoAlerta">Identificador unico de tipo alerta</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de alertas en historial</returns>
        public IQueryable<dynamic> GetAlertasHistorial(out int total, int? page, int? limit, string sortBy, string direction, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null)
        {
            try
            {
                SQLBDEntities _Entities = new SQLBDEntities();
                _Entities.Configuration.ProxyCreationEnabled = false;
                List<ALE_ALERTA> lista = new List<ALE_ALERTA>();
                bool filtroFechas = false;
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Comprobando que se filtrara por un rango de fechas
                filtroFechas = (fechaInicio != null && fechaFin != null);
                //Agregando 23 horas para evitar problemas al seleccionar la misma fecha
                fechaFin = (fechaFin == null ? fechaFin : fechaFin.Value.AddHours(23));

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            (x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_DESCARTADA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAR) &&
                            (x.DESCRIPCION + " " +
                              x.ID_CLIENTE + " " +
                              x.PRESTAMO + " " +
                              x.NUMERO_FACTURA
                           ).ToUpper().Contains(searchString.Trim().ToUpper())
                         )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            (x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_DESCARTADA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAR) &&
                            (x.DESCRIPCION + " " +
                              x.ID_CLIENTE + " " +
                              x.PRESTAMO + " " +
                              x.NUMERO_FACTURA
                           ).ToUpper().Contains(searchString.Trim().ToUpper())
                         )
                        .Count();

                }
                else
                {
                    lista = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            (x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_DESCARTADA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAR) 
                         )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _Entities.ALE_ALERTA.AsNoTracking()
                        .Where(x =>
                            x.ID_TIPO_ALERTA == (tipoAlerta != 0 ? tipoAlerta : x.ID_TIPO_ALERTA) &&
                            x.FECHA_ALERTA >= (filtroFechas ? fechaInicio.Value : x.FECHA_ALERTA) &&
                            x.FECHA_ALERTA <= (filtroFechas ? fechaFin.Value : x.FECHA_ALERTA) &&
                            (x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_DESCARTADA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAA ||
                             x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_PROCESADAR) 
                         )
                        .Count();
                }

                return lista.AsQueryable();

            }
            catch (Exception e)
            {
                log.Error("Error al obtener alertas en historial: " + e);
                throw new Exception("Error al obtener alertas en historial");
            }
        }


        /// <summary>
        /// Metodo que permite obtener  el total de las alertas en una determinada fecha y un tipo de alerta
        /// </summary>
        /// <param name="fecha">Fecha en que se genera la alerta</param>
        /// <param name="tipoAlerta">Tipo de lerta</param>
        /// <returns>Numero de lertas</returns>
        public int getAlertasxFechaxTipoAlerta(DateTime fecha, int tipoAlerta)
        {
            int total = 0;

            try
            {
                DateTime fechaIni = fecha.Date;
                DateTime fechaFin = fecha.Date.AddHours(23);
                total = _SQLBDEntities.ALE_ALERTA.AsNoTracking()
                       .Where(x =>
                           x.FECHA_ALERTA >= fechaIni &&
                           x.FECHA_ALERTA <= fechaFin &&
                           x.ID_TIPO_ALERTA == tipoAlerta)
                       .Count();
            }
            catch (Exception e)
            {
                log.Error("Error al obtener el total de alertas por fecha y tipo de alerta", e);
            }

            return total;
        }


        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas notificadas
        /// </summary>
        /// <returns>int cantidad de alertas notificadas</returns>
        public int NotificarAlertas()
        {
            int total = 0;
            try
            {
                total = _SQLBDEntities.ALE_ALERTA.AsNoTracking()
                    .Where(x => x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_NATIFICADA)
                    .Count();
            }
            catch (Exception e)
            {
                log.Error("Error al obtener el total de clientes en listas " + e);
            }

            return total;
        }


        /// <summary>
        // Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas pendientes de analizar
        /// </summary>
        /// <returns>int cantidad de alertas pendientes de analizar</returns>
        public int NotificarAlertasAnalizar()
        {
            int total = 0;
            try
            {
                total = _SQLBDEntities.ALE_ALERTA.AsNoTracking()
                    .Where(x => x.ESTADO_ACTUAL == ALE_ALERTA.ESTADO_ANALIZADA)
                    .Count();
            }
            catch (Exception e)
            {
                log.Error("Error al obtener el total de clientes en listas " + e);
            }

            return total;
        }


    }
}
