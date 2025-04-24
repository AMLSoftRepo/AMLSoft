using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public interface ICoincidenciaListaDao : IGenericDao<PER_COINCIDENCIA_LISTA>
    {
        List<PER_COINCIDENCIA_LISTA> GetCodigoCoincidencia(int codigo);
    }
}
