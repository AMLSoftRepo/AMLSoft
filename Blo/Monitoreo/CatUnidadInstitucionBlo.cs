using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;

namespace Blo.Monitoreo
{
    public class CatUnidadInstitucionBlo : GenericBlo<MON_CAT_UNIDAD>, ICatUnidadInstitucionBlo
    {
        /// Instancia de la clase
        /// </summary>
        private ICatUnidadInstitucionDao _catUnidadInstitucionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatUnidadInstitucionBlo(ICatUnidadInstitucionDao catUnidadInstitucionDao)
            : base(catUnidadInstitucionDao)
        {
            _catUnidadInstitucionDao = catUnidadInstitucionDao;
        }

        /// <summary>
        /// Metodo que permite obtener una lista de unidades por institución
        /// </summary>
        /// <param name="idInstitucion">Identificador de institución</param>
        /// <returns>Lista de unidades</returns>
        public List<MON_CAT_UNIDAD> GetUnidadesPorInstitucion(int idInstitucion)
        {
            try
            {
                return _catUnidadInstitucionDao.GetUnidadesPorInstitucion(idInstitucion);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener las unidades por institución: " + e);
                throw new Exception("Error al obtener las unidades por institución");
            }
        }

    }
}
