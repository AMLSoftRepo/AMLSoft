using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Matriz
{
    public interface IEscalaCalificacionBlo : IGenericBlo<MAT_ESCALA_CALIFICACION>
    {
        /// <summary>
        /// Metodo que permite validar que un rango de alerta no este dentro de otro
        /// es decir que estos valores no existan en los registros actuales
        /// </summary>
        /// <param name="id">Identificador de tipo alerta</param>
        /// <param name="valorMin">valor minimo</param>
        /// <param name="valorMax">valor maximo</param>
        /// <returns>String con el nombre de la alerta en conflicto</returns>
        string validaRangoMinMax(int id, decimal valorMin, decimal valorMax);


        /// <summary>
        /// Metodo que permite obtener el color del tipo de alerta
        /// si el valor esta dentro de algun rango retorna el color 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>String con el color en Hexadecimal</returns>
        string ObtenerColorXValor(decimal valor);
    }
}
