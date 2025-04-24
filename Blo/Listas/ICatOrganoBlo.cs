using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Listas
{
    public interface ICatOrganoBlo : IGenericBlo<LIS_CAT_ORGANOS>
    {
        List<LIS_CAT_ORGANOS> GetCodigoOrgano(int codigo);
        
        
    }
}
