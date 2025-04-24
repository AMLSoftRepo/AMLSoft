using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public class OperacionIrregularDao: GenericDao<ALE_OPERACION_IRREGULAR>,IOperacionIrregularDao
    {
        /// <summary>
        /// Metodo que permite obtener una lista de operaciones irregulares.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de operaciones irregulares</returns>
        public IQueryable<dynamic> GetOperacionesIrregulares(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<ALE_OPERACION_IRREGULAR> lista = new List<ALE_OPERACION_IRREGULAR>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.ALE_OPERACION_IRREGULAR.AsNoTracking()
                        .Where(x => (
                                  x.ID_AGENCIA + " " +
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.ALE_TIPO_ALERTA.DESCRIPCION + " " +
                                  x.DESCRIPCION
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.ALE_OPERACION_IRREGULAR.AsNoTracking()
                        .Where(x => (
                                  x.ID_AGENCIA + " " +
                                  x.MAT_CAT_AGENCIA.NOMBRE + " " +
                                  x.ALE_TIPO_ALERTA.DESCRIPCION + " " +
                                  x.DESCRIPCION
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                        .Count();

                }
                else
                {
                    lista = _SQLBDEntities.ALE_OPERACION_IRREGULAR.AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.ALE_OPERACION_IRREGULAR.AsNoTracking()
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    x.ID_AGENCIA,
                    x.MAT_CAT_AGENCIA.CODIGO_AGENCIA,
                    NOMBRE_AGENCIA = x.MAT_CAT_AGENCIA.NOMBRE,
                    x.ID_TIPO_ALERTA,
                    NOMBRE_ALERTA = x.ALE_TIPO_ALERTA.DESCRIPCION,
                    COLOR = x.ALE_TIPO_ALERTA.COLOR,
                    x.DESCRIPCION,
                    x.EXPRESION
                }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener las operaciones irregulares: " + e);
                throw new Exception("Error al obtener las operaciones irregulares");
            }
        }
    }
}
