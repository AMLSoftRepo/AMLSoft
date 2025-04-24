using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Alertas
{
    public interface IDatosAdicionalesTransaccionBlo : IGenericBlo<ALE_DATOS_ADICIONALES_TRANSACCION>
    {
        /// <summary>
        /// Metodo que permite obtener los datos adicionales de cada transacción por 
        /// su código de secuencia
        /// </summary>
        /// <param name="secuencia">Identificador de la transacción</param>
        /// <returns>Lista de datos adicionales</returns>
        List<ALE_DATOS_ADICIONALES_TRANSACCION> GetDatosAdicionalesPorSecuencia(string secuencia);

    }
}
