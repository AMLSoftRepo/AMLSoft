using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public class EventoRiesgoDao : GenericDao<MAT_EVENTO_RIESGO>, IEventoRiesgoDao
    {
        /// <summary>
        /// Metodo donde realiza el calculo del riesgo inherente
        /// Formula: Probabilidad * Impacto o Severidad
        /// </summary>
        ///<param name="probabilidad">Nivel de probabilidad</param>
        ///<param name="impacto">Nivel de impacto</param>
        /// <returns>Valor de RiesgoInherente</returns>
        public decimal RiesgoInherente(decimal probabilidad, decimal impacto)
        {
            decimal riesgoInherente = 0;

            riesgoInherente = probabilidad * impacto;

            return riesgoInherente;

        }


        /// <summary>
        /// Metodo donde realiza el calculo de la eficacia del control
        /// Formula: PromedioTotal de los controles aplicados al evento
        /// </summary>
        /// <param name="idEvento">Identificador único de MAT_EVENTO_RIESGO</param>
        /// <returns>Valor de EficaciaControl</returns>
        public decimal EficaciaControl(long idEvento)
        {
            decimal promedioTotal = 0;
            var control = _SQLBDEntities.MAT_CONTROL_EVENTO.AsNoTracking()
                                .Where(x => x.ID_EVENTO == idEvento)
                                .Select(x => x.MAT_CONTROL.TOTAL_POR)
                                .ToList();

            if (control.Any())
                promedioTotal = control.Sum() / control.Count();

            return promedioTotal;
        }


        /// <summary>
        /// Metodo donde realiza el calculo del riesgo residual
        /// Formula: Riesgo Inherente * (1-Eficacia del Control)
        /// </summary>
        /// <param name="riesgoInherente">Valor de riesgoInherente</param>
        /// <param name="eficaciaControl">Valor de eficaciaControl</param>
        /// <returns>Valor de riesgoResidual</returns>
        public decimal RiesgoResidual(decimal riesgoInherente, decimal eficaciaControl)
        {
            decimal riesgoResidual = 0;
            riesgoResidual = riesgoInherente * (1 - eficaciaControl);

            return riesgoResidual;
        }


        /// <summary>
        /// Metodo que permite obtener una lista de eventos.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de eventos</returns>
        public IQueryable<dynamic> GetEventos(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<MAT_EVENTO_RIESGO> lista = new List<MAT_EVENTO_RIESGO>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.MAT_EVENTO_RIESGO.AsNoTracking()
                        .Where(x => (
                            x.MAT_CAT_AGENCIA.NOMBRE + " " +
                            x.MAT_CAT_UNIDAD.DESCRIPCION + " " +
                            x.DESCRIPCION
                            ).ToUpper().Contains(searchString.Trim().ToUpper())
                        )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_EVENTO_RIESGO.AsNoTracking()
                        .Where(x => (
                            x.MAT_CAT_AGENCIA.NOMBRE + " " +
                            x.MAT_CAT_UNIDAD.DESCRIPCION + " " +
                            x.DESCRIPCION
                            ).ToUpper().Contains(searchString.Trim().ToUpper())
                        )
                        .Count();

                }
                else
                {
                    lista = _SQLBDEntities.MAT_EVENTO_RIESGO.AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.MAT_EVENTO_RIESGO.AsNoTracking()
                        .Count();
                }

                var records = lista
                    .Select(x => new
                    {
                        x.ID,
                        x.ID_AGENCIA,
                        AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                        x.ID_UNIDAD,
                        x.ID_FACTOR_RIESGO,
                        x.ID_RIESGO,
                        x.ID_CAUSA_RIESGO,
                        x.COMO,
                        x.DESCRIPCION,
                        x.ID_PROBABILIDAD_OCURRENCIA,
                        x.ID_IMPACTO,
                        RIESGO_INHERENTE = Math.Round(RiesgoInherente(x.MAT_CAT_PROBABILIDAD_OCURRENCIA.NIVEL, x.MAT_CAT_SEVERIDAD.NIVEL),2),
                        EFICACIA_CONTROL = Math.Round(EficaciaControl(x.ID),2),
                        RIESGO_RESIDUAL = Math.Round(RiesgoResidual(
                                            RiesgoInherente(x.MAT_CAT_PROBABILIDAD_OCURRENCIA.NIVEL, x.MAT_CAT_SEVERIDAD.NIVEL),
                                            EficaciaControl(x.ID)),2)
                    }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener los eventos: " + e);
                throw new Exception("Error al obtener los eventos");
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="idEvento">Identificador unico del evento</param>
        /// <returns>Lista de controles</returns>
        public IQueryable<dynamic> GetControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0)
        {
            try
            {
                List<MAT_CONTROL> lista = new List<MAT_CONTROL>();
                int start = (page.Value - 1) * limit.Value;
                total = 0;

                var controlEvento = _SQLBDEntities.MAT_CONTROL_EVENTO.AsNoTracking()
                    .Where(x => x.ID_EVENTO == idEvento)
                    .Select(x => x.ID_CONTROL)
                    .ToList();

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => controlEvento.Contains(x.ID) && 
                                  (
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.DESCRIPCION + " " +
                                  x.OBSERVACIONES + " " +
                                  x.TOTAL_POR
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .OrderBy(x=>x.ID_AGENCIA)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => controlEvento.Contains(x.ID) && 
                                  (
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.DESCRIPCION + " " +
                                  x.OBSERVACIONES + " " +
                                  x.TOTAL_POR
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .Count();

                }
                else
                {
                    lista = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x=> controlEvento.Contains(x.ID))
                        .OrderBy(x => x.ID_AGENCIA)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => controlEvento.Contains(x.ID))
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                    AUTOMATIZACION = x.MAT_CAT_AUTOMATIZACION.DESCRIPCION,
                    DISENO = x.MAT_CAT_DISENO.DESCRIPCION,
                    DOCUMENTACION = x.MAT_CAT_DOCUMENTACION.DESCRIPCION,
                    FRECUENCIA = x.MAT_CAT_FRECUENCIA.DESCRIPCION,
                    MEZCLA = x.MAT_CAT_MEZCLA.DESCRIPCION,
                    TIPO_CONTROL = x.MAT_CAT_TIPO_CONTROL.DESCRIPCION,
                    x.DESCRIPCION,
                    x.OBSERVACIONES,
                    x.TOTAL_POR
                }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener los controles: " + e);
                throw new Exception("Error al obtener los controles");
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="idEvento">Identificador unico del evento</param>
        /// <returns>Lista de controles</returns>
        public IQueryable<dynamic> GetSeleccionControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0)
        {
            try
            {
                List<MAT_CONTROL> lista = new List<MAT_CONTROL>();
                int start = (page.Value - 1) * limit.Value;
                total = 0;

                int idAgencia = _SQLBDEntities.MAT_EVENTO_RIESGO.Where(x => x.ID == idEvento).FirstOrDefault().ID_AGENCIA;

                var controlEvento = _SQLBDEntities.MAT_CONTROL_EVENTO.AsNoTracking()
                    .Where(x => x.ID_EVENTO == idEvento)
                    .Select(x => x.ID_CONTROL)
                    .ToList();

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => !controlEvento.Contains(x.ID) && 
                                    x.ID_AGENCIA == idAgencia &&
                                  (
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.DESCRIPCION + " " +
                                  x.OBSERVACIONES + " " +
                                  x.TOTAL_POR
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .OrderBy(x => x.ID_AGENCIA)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => !controlEvento.Contains(x.ID) &&
                                    x.ID_AGENCIA == idAgencia &&
                                  (
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.DESCRIPCION + " " +
                                  x.OBSERVACIONES + " " +
                                  x.TOTAL_POR
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .Count();

                }
                else
                {
                    lista = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => !controlEvento.Contains(x.ID) && 
                                    x.ID_AGENCIA == idAgencia )
                        .OrderBy(x => x.ID_AGENCIA)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => !controlEvento.Contains(x.ID) && 
                                    x.ID_AGENCIA == idAgencia )
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                    AUTOMATIZACION = x.MAT_CAT_AUTOMATIZACION.DESCRIPCION,
                    DISENO = x.MAT_CAT_DISENO.DESCRIPCION,
                    DOCUMENTACION = x.MAT_CAT_DOCUMENTACION.DESCRIPCION,
                    FRECUENCIA = x.MAT_CAT_FRECUENCIA.DESCRIPCION,
                    MEZCLA = x.MAT_CAT_MEZCLA.DESCRIPCION,
                    TIPO_CONTROL = x.MAT_CAT_TIPO_CONTROL.DESCRIPCION,
                    x.DESCRIPCION,
                    x.OBSERVACIONES,
                    x.TOTAL_POR
                }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener los controles: " + e);
                throw new Exception("Error al obtener los controles");
            }
        }


    }
}
