using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;

namespace Blo.Alertas
{
    public class DatosAdicionalesTransaccionBlo : GenericBlo<ALE_DATOS_ADICIONALES_TRANSACCION>, IDatosAdicionalesTransaccionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private IDatosAdicionalesTransaccionDao _datosAdicionalesTransaccionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public DatosAdicionalesTransaccionBlo(IDatosAdicionalesTransaccionDao datosAdicionalesTransaccionDao)
            : base(datosAdicionalesTransaccionDao)
        {
            _datosAdicionalesTransaccionDao = datosAdicionalesTransaccionDao;
        }


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
                lista = _datosAdicionalesTransaccionDao.GetDatosAdicionalesPorSecuencia(secuencia);
            }
            catch (Exception e)
            {
                log.Error("Error cargando datos adicionales a la transacción", e);
            }

            return lista;
        }

    }
}
