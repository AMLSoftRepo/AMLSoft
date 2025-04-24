using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public interface ICatAgenciaDao: IGenericDao<MAT_CAT_AGENCIA>
    {
        List<MAT_CAT_AGENCIA> GetCodAgencia(int codigo);

        List<MAT_CAT_AGENCIA> ExistCodAgencia(int id, int codigo);
    }
}
