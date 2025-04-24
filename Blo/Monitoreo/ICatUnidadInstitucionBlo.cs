using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Monitoreo
{
    public interface ICatUnidadInstitucionBlo: IGenericBlo<MON_CAT_UNIDAD>
    {
        /// <summary>
        /// Metodo que permite obtener una lista de unidades por institución
        /// </summary>
        /// <param name="idInstitucion">Identificador de institución</param>
        /// <returns>Lista de unidades</returns>
        List<MON_CAT_UNIDAD> GetUnidadesPorInstitucion(int idInstitucion);
    }
}
