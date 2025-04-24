using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public class CatGrupoDao:GenericDao<LIS_CAT_GRUPO_FATF>,ICatGrupoDao
    {
        public List<LIS_CAT_GRUPO_FATF> GetCodigoGrupo(int codigo)
        {
            List<LIS_CAT_GRUPO_FATF> lista = new List<LIS_CAT_GRUPO_FATF>();
            try
            {
                lista = _SQLBDEntities.LIS_CAT_GRUPO_FATF
                        .Where(x => x.ID == codigo)
                        .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de grupo miembros del FATF" + e);
            }
            return lista;
        }
    }
}
