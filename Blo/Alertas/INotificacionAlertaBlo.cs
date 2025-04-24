using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Alertas
{
    public interface INotificacionAlertaBlo : IGenericBlo<ALE_NOTIFICACION_ALERTA>
    {
        /// <summary>
        /// Método que permite guardar los tipos de alerta para un contacto 
        /// en especifico, en este proceso se eliminan las axistentes y
        /// se agregan los nuevos tipos de alerta
        /// </summary>
        /// <param name="idContacto">identificador de contacto_alerta</param>
        /// <param name="idsAlertas">Array de id tipo alerta</param>
        void SaveNotificacionContacto(int idContacto, int[] idsAlertas);

        /// <summary>
        /// Metodo que permite eliminar todos las notificaciones
        /// asignadas aun contacto de alerta
        /// </summary>
        /// <param name="idContacto">Identificador único de contacto_alerta</param>
        void DeleteNotificacionContacto(int idContacto);
    }
}
