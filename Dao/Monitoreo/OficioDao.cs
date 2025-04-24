using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;

namespace Dao.Monitoreo
{
    public class OficioDao : GenericDao<MON_OFICIO>, IOficioDao
    {

        /// <summary>
        /// Metodo que permite obtener una lista de oficios pendientes de recibir una respuesta de la UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios</returns>
        public IQueryable<dynamic> GetOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<MON_OFICIO> lista = new List<MON_OFICIO>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    searchString = searchString.Trim().ToUpper();
                    if (searchString.StartsWith("PERSONA:"))  //Buscar datos de personas 
                    {
                        string persona = searchString.Remove(0, 8).Trim();
                        if (!string.IsNullOrWhiteSpace(persona))
                        {
                            List<long> idsOficios = new List<long>();

                            idsOficios = _SQLBDEntities.MON_OFICIO_PERSONA.AsNoTracking()
                                .Where(x => (
                                    x.NOMBRE + " " +
                                    x.DUI + " " +
                                    x.NIT + " " +
                                    x.NUMERO_DOCUMENTO
                                    ).ToUpper().Contains(persona))
                                .Select(x => x.ID_OFICIO)
                                .ToList();

                            lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                                .Where(x => idsOficios.Contains(x.ID) && x.PROCESADO && x.FECHA_RECIBIDO_UIF == null)
                                .OrdenarGrid(sortBy, direction)
                                .Skip(start)
                                .Take(limit.Value)
                                .ToList();

                            total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                                .Where(x => idsOficios.Contains(x.ID) && x.PROCESADO && x.FECHA_RECIBIDO_UIF == null)
                                .Count();
                        }
                    }
                    else //Buscar datos de oficios 
                    {
                        lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                            .Where(x =>
                                    x.PROCESADO && x.FECHA_RECIBIDO_UIF == null &&
                                    (
                                        x.NUMERO_OFICIO + " " +
                                        x.REFERENCIA + " " +
                                        (x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO")
                                    ).ToUpper().Contains(searchString))
                            .OrdenarGrid(sortBy, direction)
                            .Skip(start)
                            .Take(limit.Value)
                            .ToList();

                        total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                            .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF == null &&
                                    (
                                        x.NUMERO_OFICIO + " " +
                                        x.REFERENCIA + " " +
                                        (x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO")
                                    ).ToUpper().Contains(searchString))
                            .Count();
                    }
                }
                else
                {
                    lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                        .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF == null)
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                        .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF == null)
                        .Count();
                }

                var records = lista.Select(x => new
                              {
                                  x.ID,
                                  x.ID_CONTACTO_SOLICITANTE,
                                  x.ID_CONTACTO_INSTITUCION,
                                  x.ID_CONTACTO_INSTITUCION_OFICIAL,
                                  x.CUMPLIMIENTO,
                                  x.NUMERO_OFICIO,
                                  x.FECHA_MAXIMA_UIF,
                                  x.FECHA_OFICIO,
                                  x.FECHA_RECIBIDO,
                                  x.FECHA_RECIBIDO_UIF,
                                  x.FECHA_INVESTIGACION_INI,
                                  x.FECHA_INVESTIGACION_FIN,
                                  x.COMENTARIO,
                                  FECHA_RECIBIDO_UIF_VALOR = x.FECHA_RECIBIDO_UIF != null ? x.FECHA_RECIBIDO_UIF.Value.ToString("dd/MM/yyyy") : "null",
                                  x.FECHA_RESPUESTA_UIF,
                                  x.FORMATO_RESPUESTA,
                                  x.DIAS_MAX,
                                  x.DIAS_HABILES,
                                  x.REFERENCIA,
                                  x.ORIGEN,
                                  CUMPLIMIENTOLETRAS = x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO"
                              }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de oficios: " + e);
                throw new Exception("Error al obtener lista de oficios");
            }

        }


        /// <summary>
        /// Metodo que permite obtener una lista de oficios Procesados y con respuesta UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios Historicos</returns>
        public IQueryable<dynamic> GetHistorialOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<MON_OFICIO> lista = new List<MON_OFICIO>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    searchString = searchString.Trim().ToUpper();
                    if (searchString.StartsWith("PERSONA:"))  //Buscar datos de personas 
                    {
                        string persona = searchString.Remove(0, 8).Trim();
                        if (!string.IsNullOrWhiteSpace(persona))
                        {
                            List<long> idsOficios = new List<long>();

                            idsOficios = _SQLBDEntities.MON_OFICIO_PERSONA.AsNoTracking()
                                .Where(x => (
                                    x.NOMBRE + " " +
                                    x.DUI + " " +
                                    x.NIT + " " +
                                    x.NUMERO_DOCUMENTO
                                    ).ToUpper().Contains(persona))
                                .Select(x => x.ID_OFICIO)
                                .ToList();

                            lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                                .Where(x => idsOficios.Contains(x.ID) && x.PROCESADO && x.FECHA_RECIBIDO_UIF != null)
                                .OrdenarGrid(sortBy, direction)
                                .Skip(start)
                                .Take(limit.Value)
                                .ToList();

                            total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                                .Where(x => idsOficios.Contains(x.ID) && x.PROCESADO && x.FECHA_RECIBIDO_UIF != null)
                                .Count();
                        }
                    }
                    else //Buscar datos de oficios 
                    {
                        lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                            .Where(x =>
                                    x.PROCESADO && x.FECHA_RECIBIDO_UIF != null &&
                                    (
                                        x.NUMERO_OFICIO + " " +
                                        x.REFERENCIA + " " +
                                        (x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO")
                                    ).ToUpper().Contains(searchString))
                            .OrdenarGrid(sortBy, direction)
                            .Skip(start)
                            .Take(limit.Value)
                            .ToList();

                        total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                            .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF != null &&
                                    (
                                        x.NUMERO_OFICIO + " " +
                                        x.REFERENCIA + " " +
                                        (x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO")
                                    ).ToUpper().Contains(searchString))
                            .Count();
                    }
                }
                else
                {
                    lista = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                        .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF != null)
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                        .Where(x => x.PROCESADO && x.FECHA_RECIBIDO_UIF != null)
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    x.ID_CONTACTO_SOLICITANTE,
                    x.ID_CONTACTO_INSTITUCION,
                    x.ID_CONTACTO_INSTITUCION_OFICIAL,
                    x.CUMPLIMIENTO,
                    x.NUMERO_OFICIO,
                    x.FECHA_MAXIMA_UIF,
                    x.FECHA_OFICIO,
                    x.FECHA_RECIBIDO,
                    x.FECHA_RECIBIDO_UIF,
                    x.FECHA_INVESTIGACION_INI,
                    x.FECHA_INVESTIGACION_FIN,
                    x.COMENTARIO,
                    FECHA_RECIBIDO_UIF_VALOR = x.FECHA_RECIBIDO_UIF != null ? x.FECHA_RECIBIDO_UIF.Value.ToString("dd/MM/yyyy") : "null",
                    x.FECHA_RESPUESTA_UIF,
                    x.FORMATO_RESPUESTA,
                    x.DIAS_MAX,
                    x.DIAS_HABILES,
                    x.REFERENCIA,
                    x.ORIGEN,
                    CUMPLIMIENTOLETRAS = x.CUMPLIMIENTO <= 0 ? "VERDE" : "ROJO"
                }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener los oficios historicos: " + e);
                throw new Exception("Error al obtener los oficios historicos");
            }
        }


        /// <summary>
        /// Metodo que permite obtener el total de oficios pendientes de procesar y
        /// donde la fecha actual mas los dias aplicados en configuración sea mayor
        /// o igual a la fecha maxima UIF. Entonces sera notificada en el sistema.
        /// </summary>
        /// <returns>Int total de oficios</returns>
        public int GetTotalNotificarOficio()
        {
            int cantidad = 0;

            try
            {
                int diasNotificarOficio = 0;
                var configuracion = _SQLBDEntities.MON_OFICIO_CONFIGURACION.ToList();

                if (configuracion.Any())
                    diasNotificarOficio = configuracion.FirstOrDefault().DIAS_NOTIFICACION;

                DateTime obtenerFecha = DateTime.Now.AddDays(diasNotificarOficio);
                cantidad = _SQLBDEntities.MON_OFICIO.AsNoTracking()
                    .Where(x => !x.PROCESADO && obtenerFecha >= x.FECHA_MAXIMA_UIF)
                    .Count();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error al obtener el total de oficios a notificar");
            }

            return cantidad;
        }

        /// <summary>
        /// Metodo que obtiene una lista de oficios PENDIENTES de procesar, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        public List<MON_OFICIO> GetCalendarioOficios(DateTime fechaIni, DateTime fechaFin)
        {
            List<MON_OFICIO> lista = new List<MON_OFICIO>();
            try
            {
                lista = _SQLBDEntities.MON_OFICIO
                    .AsNoTracking()
                    .Where(x =>
                        !x.PROCESADO &&
                        x.FECHA_MAXIMA_UIF >= fechaIni &&
                        x.FECHA_MAXIMA_UIF <= fechaFin)
                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de oficios", e);
            }

            return lista;
        }


        /// <summary>
        /// Metodo que obtiene una lista de oficios en HISTORIAL, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        public List<MON_OFICIO> GetHistorialCalendarioOficios(DateTime fechaIni, DateTime fechaFin)
        {
            List<MON_OFICIO> lista = new List<MON_OFICIO>();
            try
            {
                lista = _SQLBDEntities.MON_OFICIO
                    .AsNoTracking()
                    .Where(x =>
                        x.PROCESADO &&
                        x.FECHA_MAXIMA_UIF >= fechaIni &&
                        x.FECHA_MAXIMA_UIF <= fechaFin &&
                        x.FECHA_RECIBIDO_UIF != null)
                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de oficios", e);
            }

            return lista;
        }

    }
}
