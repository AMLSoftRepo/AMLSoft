using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public class ExentoDao : GenericDao<LIS_EXENTO>, IExentoDao
    {

        /// <summary>
        /// Metodo que permite obtener una lista de clientes exentos.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de clientes exentos</returns>
        public IQueryable<dynamic> GetExentos(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<LIS_EXENTO> lista = new List<LIS_EXENTO>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.LIS_EXENTO.AsNoTracking()
                        .Where(x => (
                                  x.CODIGO_CLIENTE.ToString() + " " +
                                  x.NOMBRES + " " +
                                  x.PRIMER_APELLIDO + " " + 
                                  x.SEGUNDO_APELLIDO + " " + 
                                  x.APELLIDO_DE_CASADA
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                          )
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.LIS_EXENTO.AsNoTracking()
                       .Where(x => (
                                  x.CODIGO_CLIENTE.ToString() + " " +
                                  x.NOMBRES + " " +
                                  x.PRIMER_APELLIDO + " " +
                                  x.SEGUNDO_APELLIDO + " " +
                                  x.APELLIDO_DE_CASADA
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                          )
                        .Count();

                }
                else
                {
                    lista = _SQLBDEntities.LIS_EXENTO.AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.LIS_EXENTO.AsNoTracking()
                        .Count();
                }

                var records = lista.Select(x => new
                {
                    x.ID,
                    x.CODIGO_CLIENTE,
                    x.NUMERO_IDENTIFICACION,
                    x.NUMERO_IDENTIFICACION_2,
                    x.NOMBRES,
                    APELLIDOS = x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.APELLIDO_DE_CASADA,
                    x.PAIS_NACIMIENTO,
                    x.COMENTARIO,
                    x.MOTIVO_INGRESO,
                    x.USUARIO,
                    FECHA = string.Format("{0:G}", x.FECHA)

                }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de clientes exentos: " + e);
                throw new Exception("Error al obtener lista de clientes excentos");
            }
        }

    }
}
