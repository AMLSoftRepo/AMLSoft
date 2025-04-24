using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public interface ICatTituloDao:IGenericDao<LIS_CAT_TITULOS>
    {
        List<LIS_CAT_TITULOS> GetCodigoTitulo(int codigo);
    }
}
