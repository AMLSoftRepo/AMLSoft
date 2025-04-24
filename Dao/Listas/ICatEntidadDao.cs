using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public interface ICatEntidadDao : IGenericDao<LIS_CAT_ENTIDADES>
    {
        List<LIS_CAT_ENTIDADES> GetCodigoEntidad(int codigo);
    }
}
