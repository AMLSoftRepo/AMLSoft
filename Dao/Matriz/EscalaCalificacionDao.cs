using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public class EscalaCalificacionDao : GenericDao<MAT_ESCALA_CALIFICACION>, IEscalaCalificacionDao
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
                var list = _SQLBDEntities.MAT_ESCALA_CALIFICACION
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
    }
}
