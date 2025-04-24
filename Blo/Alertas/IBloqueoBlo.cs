using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Alertas
{
    public interface IBloqueoBlo: IGenericBlo<ALE_BLOQUEO_PRESTAMO>
    {
        /// <summary>
        /// Metodo que permite eliminar todos los registros donde el
        /// numero de prestamo contenga el estado de bloqueado
        /// </summary>
        /// <param name="prestamo">Numero de prestamo</param>
        void EliminarPrestamoBloqueado(string prestamo);

    }
}
