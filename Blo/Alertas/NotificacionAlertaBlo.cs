using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;
using System.Transactions;

namespace Blo.Alertas
{
    public class NotificacionAlertaBlo : GenericBlo<ALE_NOTIFICACION_ALERTA>, INotificacionAlertaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private INotificacionAlertaDao _notificacionAlertaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public NotificacionAlertaBlo(INotificacionAlertaDao notificacionAlertaDao)
            : base(notificacionAlertaDao)
        {
            _notificacionAlertaDao = notificacionAlertaDao;
        }

        /// <summary>
        /// Método que permite guardar los tipos de alerta para un contacto 
        /// en especifico, en este proceso se eliminan las axistentes y
        /// se agregan los nuevos tipos de alerta
        /// </summary>
        /// <param name="idContacto">identificador de contacto_alerta</param>
        /// <param name="idsAlertas">Array de id tipo alerta</param>
        public void SaveNotificacionContacto(int idContacto, int[] idsAlertas)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    //Obtener los ids 
                    var ListaNotificaciones = _notificacionAlertaDao.GetAll().Where(x => x.ID_CONTACTO == idContacto).Select(x => x.ID).ToList();
                    foreach (int id in ListaNotificaciones)
                        _notificacionAlertaDao.Remove(id);
       

                    foreach (int id in idsAlertas)
                    {
                        ALE_NOTIFICACION_ALERTA notificacionAlerta = new ALE_NOTIFICACION_ALERTA();

                        notificacionAlerta.ID_CONTACTO = idContacto;
                        notificacionAlerta.ID_TIPO_ALERTA = id;

                        _notificacionAlertaDao.Save(notificacionAlerta);
                    }

                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error("Error guardando contacto-notificaciones: ", e);
                    throw new Exception("Error guardando contacto-notificaciones:", e);
                }
            }
        }

        /// <summary>
        /// Metodo que permite eliminar todos las notificaciones
        /// asignadas aun contacto de alerta
        /// </summary>
        /// <param name="idContacto">Identificador único de contacto_alerta</param>
        public virtual void DeleteNotificacionContacto(int idContacto)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    //Obtener los ids 
                    var ListaNotificaciones = _notificacionAlertaDao.GetAll().Where(x => x.ID_CONTACTO == idContacto).Select(x => x.ID).ToList();

                    foreach (int id in ListaNotificaciones)
                    {
                        //eliminar
                        _notificacionAlertaDao.Remove(id);
                    }

                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error("Error eliminando contacto-notificaciones: ", e);
                    throw new Exception("Error eliminando contacto-notificaciones:", e);
                }
            }
            return;
        }
    }
}
