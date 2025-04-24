using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace Dao.Alertas
{
    public class BloqueoDao : GenericDao<ALE_BLOQUEO_PRESTAMO>, IBloqueoDao
    {
        /// <summary>
        /// Metodo que permite eliminar todos los registros donde el
        /// numero de prestamo contenga el estado de bloqueado
        /// </summary>
        /// <param name="prestamo">Numero de prestamo</param>
        public void EliminarPrestamoBloqueado(string prestamo)
        {
            try
            {
                var bloqueos = _SQLBDEntities.ALE_BLOQUEO_PRESTAMO
                .Where(x => x.PRESTAMO == prestamo)
                .ToList();

                _SQLBDEntities.ALE_BLOQUEO_PRESTAMO.RemoveRange(bloqueos);
                _SQLBDEntities.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error("Error al eliminar registros  del prestamo bloqueado: " + e);
                throw new Exception("Error al eliminar registros del prestamo bloqueado");
            }
        }

    }
}
