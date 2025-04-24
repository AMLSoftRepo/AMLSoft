using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public class CatOrganoDao : GenericDao<LIS_CAT_ORGANOS>, ICatOrganoDao
    {
        public List<LIS_CAT_ORGANOS> GetCodigoOrgano(int codigo)
        {
            List<LIS_CAT_ORGANOS> lista = new List<LIS_CAT_ORGANOS>();
            try
            {
                lista = _SQLBDEntities.LIS_CAT_ORGANOS
                            .Where(x => x.ID == codigo)
                            .ToList();

            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de organos del estado", e);
            }
            return lista;
        }

    }
}
