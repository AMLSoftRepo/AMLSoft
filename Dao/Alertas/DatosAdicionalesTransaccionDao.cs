using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public class DatosAdicionalesTransaccionDao : GenericDao<ALE_DATOS_ADICIONALES_TRANSACCION>, IDatosAdicionalesTransaccionDao
    {

        /// <summary>
        /// Metodo que permite obtener los datos adicionales de cada transacción por 
        /// su código de secuencia
        /// </summary>
        /// <param name="secuencia">Identificador de la transacción</param>
        /// <returns>Lista de datos adicionales</returns>
        public List<ALE_DATOS_ADICIONALES_TRANSACCION> GetDatosAdicionalesPorSecuencia(string secuencia)
        {
            List<ALE_DATOS_ADICIONALES_TRANSACCION> lista = new List<ALE_DATOS_ADICIONALES_TRANSACCION>();

            try
            {
                lista = _SQLBDEntities.ALE_DATOS_ADICIONALES_TRANSACCION.AsNoTracking()
                       .Where(x => x.SECUENCIA == secuencia)
                       .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando datos adicionales a la transacción", e);
            }

            return lista;
        }

    }
}
