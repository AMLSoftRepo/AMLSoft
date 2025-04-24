using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public class ControlDao : GenericDao<MAT_CONTROL>, IControlDao
    {
        /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de controles</returns>
        public IQueryable<dynamic> GetControles(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<MAT_CONTROL> lista = new List<MAT_CONTROL>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => (
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.DESCRIPCION + " " +
                                  x.OBSERVACIONES + " " +
                                  x.TOTAL_POR
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Where(x => (
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
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.MAT_CONTROL.AsNoTracking()
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    x.ID_AGENCIA,
                    AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                    x.ID_AUTOMATIZACION,
                    x.ID_DISENO,
                    x.ID_DOCUMENTACION,
                    x.ID_FRECUENCIA,
                    x.ID_MEZCLA,
                    x.ID_TIPO_CONTROL,
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
