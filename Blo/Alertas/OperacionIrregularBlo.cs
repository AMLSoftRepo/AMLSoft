using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;

namespace Blo.Alertas
{
    public class OperacionIrregularBlo : GenericBlo<ALE_OPERACION_IRREGULAR>, IOperacionIrregularBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IOperacionIrregularDao _OperacionIrregularDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public OperacionIrregularBlo(IOperacionIrregularDao OperacionIrregularDao)
            : base(OperacionIrregularDao)
        {
            _OperacionIrregularDao = OperacionIrregularDao;
        }

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
                return _OperacionIrregularDao.GetOperacionesIrregulares(out total, page,limit,sortBy,direction,searchString);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }
    }
}
