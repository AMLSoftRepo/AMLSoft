using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public interface ICatGrupoDao : IGenericDao<LIS_CAT_GRUPO_FATF>
    {
        List<LIS_CAT_GRUPO_FATF> GetCodigoGrupo(int codigo);
    }
}
