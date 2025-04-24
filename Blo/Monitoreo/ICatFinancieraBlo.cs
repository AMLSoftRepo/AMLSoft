using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Monitoreo
{
    public interface ICatFinancieraBlo:IGenericBlo<MON_CAT_FINANCIERA>
    {
        List<MON_CAT_FINANCIERA> GetCodFinanciera(int codigo);

        List<MON_CAT_FINANCIERA> ExistCodFinanciera(int id, int codigo);
    }
}
