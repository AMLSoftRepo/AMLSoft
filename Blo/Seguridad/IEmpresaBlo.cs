using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Seguridad
{
    public interface IEmpresaBlo : IGenericBlo<SEG_EMPRESA>
    {
        List<SEG_EMPRESA> GetEmpresaByCodigo(string codigo);
    }
}
