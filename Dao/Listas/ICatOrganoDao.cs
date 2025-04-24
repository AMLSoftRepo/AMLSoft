using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public interface ICatOrganoDao : IGenericDao<LIS_CAT_ORGANOS>
    {
        List<LIS_CAT_ORGANOS> GetCodigoOrgano(int codigo);        
    }
}
