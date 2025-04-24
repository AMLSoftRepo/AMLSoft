using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Perfiles
{
    public interface ICoincidenciaListaBlo:IGenericBlo<PER_COINCIDENCIA_LISTA>
    {
        List<PER_COINCIDENCIA_LISTA> GetCodigoCoincidencia(int codigo);
    }
}
