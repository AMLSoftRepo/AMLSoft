using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public interface IEscalaCalificacionDao : IGenericDao<MAT_ESCALA_CALIFICACION>
    {
        /// <summary>
        /// Metodo que permite obtener el color del tipo de alerta
        /// si el valor esta dentro de algun rango retorna el color 
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>String con el color en Hexadecimal</returns>
        string ObtenerColorXValor(decimal valor);
    }
}
