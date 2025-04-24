using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public class AlertaListaPersonaDao : GenericDao<ALE_ALERTA_LISTA_PERSONA>, IAlertaListaPersonaDao
    {

        /// <summary>
        /// Metodo que permite listar y buscar clientes que se encontraron en las listas internacionales
        /// como en las personalizadas.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de clientes en listas</returns>
        public IQueryable<dynamic> GetAlertaListaPersona(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<ALE_ALERTA_LISTA_PERSONA> lista = new List<ALE_ALERTA_LISTA_PERSONA>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.ALE_ALERTA_LISTA_PERSONA
                         .AsNoTracking()
                         .Where(x => (
                                   x.ID_CLIENTE + " " +
                                   x.NOMBRE_CLIENTE).ToUpper().Contains(searchString.Trim().ToUpper()))
                         .OrdenarGrid(sortBy, direction)
                         .Skip(start)
                         .Take(limit.Value)
                         .ToList();


                    total = _SQLBDEntities.ALE_ALERTA_LISTA_PERSONA
                         .AsNoTracking()
                         .Where(x => (
                                   x.ID_CLIENTE + " " +
                                   x.NOMBRE_CLIENTE).ToUpper().Contains(searchString.Trim().ToUpper()))
                         .Count();
                }
                else
                {
                    lista = _SQLBDEntities.ALE_ALERTA_LISTA_PERSONA
                        .AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.ALE_ALERTA_LISTA_PERSONA
                        .AsNoTracking()
                        .Count();
                }

                return lista.AsQueryable();

            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de clientes en listas: " + e);
                throw new Exception("Error al obtener lista de clientes en listas");
            }
        }


        /// <summary>
        /// Metodo que obtiene el total de clientes en listas
        /// </summary>
        /// <returns>Numero de clientes en listas</returns>
        public int NotificarListas()
        {
            int total = 0;
            try
            {
                total =_SQLBDEntities.ALE_ALERTA_LISTA_PERSONA.AsNoTracking().Count();
            }
            catch (Exception e)
            {
                log.Error("Error al obtener el total de clientes en listas " + e);
            }

            return total;
        }


    }
}
