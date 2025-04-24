using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Matriz;

namespace Blo.Matriz
{
    public class EscalaCalificacionBlo : GenericBlo<MAT_ESCALA_CALIFICACION>, IEscalaCalificacionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IEscalaCalificacionDao _escalaCalificacionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public EscalaCalificacionBlo(IEscalaCalificacionDao escalaCalificacionDao)
            : base(escalaCalificacionDao)
        {
            _escalaCalificacionDao = escalaCalificacionDao;
        }

        /// <summary>
        /// Metodo que permite validar que un rango de alerta no este dentro de otro
        /// es decir que estos valores no existan en los registros actuales
        /// </summary>
        /// <param name="id">Identificador de tipo alerta</param>
        /// <param name="valorMin">valor minimo</param>
        /// <param name="valorMax">valor maximo</param>
        /// <returns>String con el nombre de la alerta en conflicto</returns>
        public string validaRangoMinMax(int id, decimal valorMin, decimal valorMax)
        {
            foreach (var item in _escalaCalificacionDao.GetAll().Where(x => x.ID != id))
            {
                if (valorMin >= item.VALORMIN && valorMax <= item.VALORMAX)
                    return item.DESCRIPCION.ToUpper() + " Valor mínimo: " +
                           item.VALORMIN + " Valor máximo: " +
                           item.VALORMAX;
            }

            return null;
        }

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
                color = _escalaCalificacionDao.ObtenerColorXValor(valor);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return color;
        }


    }
}
