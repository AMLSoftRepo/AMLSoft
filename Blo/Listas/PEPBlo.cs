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
    public class PEPBlo : GenericBlo<LIS_PEP>, IPEPBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPEPDao _pepDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PEPBlo(IPEPDao pepDao)
            : base(pepDao)
        {
            _pepDao = pepDao;
        }

        /// <summary>
        /// Metodo que permite obtener una lista de de PEP's.
        /// Ademas realiza la paginación de los datos, permite buscar por varios campos de un PEP
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de PEP's</returns>
        public IQueryable<dynamic> GetPEP(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _pepDao.GetPEP(out total,page,limit,sortBy,direction,searchString);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener los oficios historicos: " + e);
                throw new Exception("Error al obtener los oficios historicos");
            }
        }

    }
}
