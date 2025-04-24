using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Perfiles
{
    public interface ITipoCalificacionBlo : IGenericBlo<PER_TIPO_CALIFICACION>
    {
        /// <summary>
        /// Metodo que permite validar que un rango de calificación no este dentro de otro
        /// es decir que estos valores no existan en los registros actuales
        /// </summary>
        /// <param name="id">Identificador de tipo alerta</param>
        /// <param name="valorMin">valor minimo</param>
        /// <param name="valorMax">valor maximo</param>
        /// <returns>String con el nombre de la calificación en conflicto</returns>
        string validaRangoMinMax(int id, decimal valorMin, decimal valorMax);

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
