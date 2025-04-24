using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Listas
{
    public interface ICatGrupoBlo:IGenericBlo<LIS_CAT_GRUPO_FATF>
    {
        List<LIS_CAT_GRUPO_FATF> GetCodigoGrupo(int codigo);
    }
}
