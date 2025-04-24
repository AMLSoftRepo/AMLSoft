using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;
using Ninject;
using System.Transactions;

namespace Blo.Matriz
{
    public class ControlEventoBlo : GenericBlo<MAT_CONTROL_EVENTO>, IControlEventoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IControlEventoDao _controlEventoDao;
        private IControlDao _controlDao;
        private IEventoRiesgoBlo _eventoRiesgoBlo;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ControlEventoBlo(IControlEventoDao controlEventoDao,IControlDao controlDao,IEventoRiesgoBlo eventoRiesgoBlo)
            : base(controlEventoDao)
        {
            _controlEventoDao = controlEventoDao;
            _controlDao = controlDao;
            _eventoRiesgoBlo = eventoRiesgoBlo;
        }


        /// <summary>
        /// Metodo que permite guargar o asignar un control a un evento
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTO</param>
        /// <param name="idControl">Identificador único de CONTROL</param>
        public void SaveControl(long idEvento, long idControl)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    MAT_CONTROL_EVENTO controlEvento = new MAT_CONTROL_EVENTO();

                    controlEvento.ID_CONTROL = idControl;
                    controlEvento.ID_EVENTO = idEvento;
                    _controlEventoDao.Save(controlEvento);

                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error("Error guardando nuevo control de evento", e);
                    throw new Exception("Error guardando nuevo control de evento", e);
                }
            }
        }


        /// <summary>
        /// Metodo que permite eliminar un control asociado a un evento
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTOS</param>
        /// <param name="idControl">Identificador único de CONTROLES</param>
        public void EliminarControles(long idEvento, long idControl)
        {
            try
            {
                var listControlEventos = _controlEventoDao.GetAll()
                        .Where(x => x.ID_EVENTO == idEvento && x.ID_CONTROL == idControl)
                        .ToList();

                foreach (var item in listControlEventos)
                    _controlEventoDao.Remove(item.ID);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error elimindo control de eventos", ex);
            }
        }


        /// <summary>
        /// Metodo que permite guardar un nuevo control
        /// y támbien agregarlo a un evento determinado
        /// </summary>
        /// <param name="idEvento">Identificador único de EVENTO</param>
        /// <param name="data">conjunto de datos del CONTROL</param>
        public void GuargarNuevoControl(long idEvento, MAT_CONTROL data)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    MAT_CONTROL_EVENTO controlEvento = new MAT_CONTROL_EVENTO();

                    _controlDao.Save(data);

                    controlEvento.ID_CONTROL = data.ID;
                    controlEvento.ID_EVENTO = idEvento;
                    _controlEventoDao.Save(controlEvento);


                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error("Error guardando nuevo control de evento", e);
                    throw new Exception("Error guardando nuevo control de evento", e);
                }
            }
        }


    }
}
