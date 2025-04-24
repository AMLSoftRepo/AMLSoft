using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public interface ICatFinancieraDao: IGenericDao<MON_CAT_FINANCIERA>
    {
        List<MON_CAT_FINANCIERA> GetCodFinanciera(int codigo);

        List<MON_CAT_FINANCIERA> ExistCodFinanciera(int id, int codigo);
    }
}
