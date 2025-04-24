using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public class ParametroDao : GenericDao<SEG_PARAMETRO>, IParametroDao
    {
        /// <summary>
        /// Obtiene un objeto de tipo SEG_PARAMETRO
        /// </summary>
        /// <param name="codigo">codigo para identificar el parametro</param>
        /// <returns>objeto</returns>
        public SEG_PARAMETRO GetParametroByCodigo(string codigo)
        {
            SEG_PARAMETRO lista = new SEG_PARAMETRO();
            try
            {
                lista = _SQLBDEntities.SEG_PARAMETRO
                        .Where(x => x.CODIGO == codigo)
                        .First();
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return lista;
        }
    }
}
