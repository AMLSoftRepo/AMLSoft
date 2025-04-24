using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Listas;
using Ninject;

namespace Blo.Listas
{
    public class ExentoBlo : GenericBlo<LIS_EXENTO>, IExentoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IExentoDao _exentoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ExentoBlo(IExentoDao exentoDao)
            : base(exentoDao)
        {
            _exentoDao = exentoDao;
        }


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
                return _exentoDao.GetExentos(out total, page, limit, sortBy, direction, searchString);

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}
