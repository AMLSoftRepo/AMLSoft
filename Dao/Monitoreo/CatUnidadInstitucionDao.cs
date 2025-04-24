using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public class CatUnidadInstitucionDao : GenericDao<MON_CAT_UNIDAD>, ICatUnidadInstitucionDao
    {
        /// <summary>
        /// Metodo que permite obtener una lista de unidades por institución
        /// </summary>
        /// <param name="idInstitucion">Identificador de institución</param>
        /// <returns>Lista de unidades</returns>
        public List<MON_CAT_UNIDAD> GetUnidadesPorInstitucion(int idInstitucion)
        {
            try
            {
                return _SQLBDEntities.MON_CAT_UNIDAD
                    .Where(x => x.ID_INSTITUCION == idInstitucion)
                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error al obtener las unidades por institución: " + e);
                throw new Exception("Error al obtener las unidades por institución");
            }
        }

    }
}
