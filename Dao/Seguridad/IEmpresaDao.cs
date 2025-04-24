using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public interface IEmpresaDao : IGenericDao<SEG_EMPRESA>
    {
        List<SEG_EMPRESA> GetEmpresaByCodigo(string codigo);
    }
}
