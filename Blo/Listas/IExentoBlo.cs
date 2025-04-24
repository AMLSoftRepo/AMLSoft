using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Listas
{
    public interface IExentoBlo : IGenericBlo<LIS_EXENTO>
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
        IQueryable<dynamic> GetExentos(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);
    }
}
