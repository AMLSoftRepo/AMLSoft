using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Perfiles;

namespace Blo.Perfiles
{
    public class TipoCalificacionBlo : GenericBlo<PER_TIPO_CALIFICACION>, ITipoCalificacionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ITipoCalificacionDao _tipoCalificacionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public TipoCalificacionBlo(ITipoCalificacionDao tipoCalificacionDao)
            : base(tipoCalificacionDao)
        {
            _tipoCalificacionDao = tipoCalificacionDao;
        }

        /// <summary>
        /// Metodo que permite validar que un rango de calificación no este dentro de otro
        /// es decir que estos valores no existan en los registros actuales
        /// </summary>
        /// <param name="id">Identificador de tipo alerta</param>
        /// <param name="valorMin">valor minimo</param>
        /// <param name="valorMax">valor maximo</param>
        /// <returns>String con el nombre de la calificación en conflicto</returns>
        public string validaRangoMinMax(int id, decimal valorMin, decimal valorMax)
        {
            foreach (var item in _tipoCalificacionDao.GetAll().Where(x => x.ID != id))
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
                color = _tipoCalificacionDao.ObtenerColorXValor(valor);
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
                id = _tipoCalificacionDao.ObtenerIdCalificacionXValor(valor);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return id;
        }
    }
}
