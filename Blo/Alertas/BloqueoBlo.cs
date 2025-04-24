using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Alertas;
using Ninject;

namespace Blo.Alertas
{
    public class BloqueoBlo : GenericBlo<ALE_BLOQUEO_PRESTAMO>, IBloqueoBlo
    {
        /// Instancia de la clase
        /// </summary>        
        private IBloqueoDao _bloqueoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public BloqueoBlo(IBloqueoDao bloqueoDao)
            : base(bloqueoDao)
        {
            _bloqueoDao = bloqueoDao;
        }

        /// <summary>
        /// Metodo que permite eliminar todos los registros donde el
        /// numero de prestamo contenga el estado de bloqueado
        /// </summary>
        /// <param name="prestamo">Numero de prestamo</param>
        public void EliminarPrestamoBloqueado(string prestamo)
        {
            try
            {
                _bloqueoDao.EliminarPrestamoBloqueado(prestamo);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


    }
}
