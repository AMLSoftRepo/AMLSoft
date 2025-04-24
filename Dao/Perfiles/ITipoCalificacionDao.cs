using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public interface ITipoCalificacionDao : IGenericDao<PER_TIPO_CALIFICACION>
    {
        /// <summary>
        /// Metodo que permite obtener el color del tipo de alerta
        /// si el valor esta dentro de algun rango retorna el color 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>String con el color en Hexadecimal</returns>
        string ObtenerColorXValor(decimal valor);


        /// <summary>
        /// Metodo que permite obtener el id del tipo de calificación 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>int con el id  del tipo calificación</returns>
        int ObtenerIdCalificacionXValor(decimal valor);


    }
}
