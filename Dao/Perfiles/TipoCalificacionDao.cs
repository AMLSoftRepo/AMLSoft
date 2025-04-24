using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public class TipoCalificacionDao : GenericDao<PER_TIPO_CALIFICACION>, ITipoCalificacionDao
    {
        /// <summary>
        /// Metodo que permite obtener el color del tipo de alerta
        /// si el valor esta dentro de algun rango retorna el color 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>String con el color en Hexadecimal</returns>
        public string ObtenerColorXValor(decimal valor)
        {
            string color = "#ffffff";
            try
            {
                var list = _SQLBDEntities.PER_TIPO_CALIFICACION
                          .Where(y => valor >= y.VALORMIN && valor <= y.VALORMAX)
                          .Select(y => y.COLOR);

                if (list.Any())
                    color = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return color;
        }


        /// <summary>
        /// Metodo que permite obtener el id del tipo de calificación 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>int con el id  del tipo calificación</returns>
        public int ObtenerIdCalificacionXValor(decimal valor)
        {
            int id = 0;
            try
            {
                var list = _SQLBDEntities.PER_TIPO_CALIFICACION
                          .Where(y => valor >= y.VALORMIN && valor <= y.VALORMAX)
                          .Select(y => y.ID);

                if (list.Any())
                    id = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return id;
        }


    }
}
