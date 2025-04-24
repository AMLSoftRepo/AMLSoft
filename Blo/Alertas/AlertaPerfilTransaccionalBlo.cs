using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;

namespace Blo.Alertas
{
    public class AlertaPerfilTransaccionalBlo : GenericBlo<ALE_ALERTA_PERFIL_TRAN>, IAlertaPerfilTransaccionalBlo 
    {
        private IAlertaPerfilTransaccionalDao _alertaPerfilTransaccionalDao;
         
        [Inject]
        public AlertaPerfilTransaccionalBlo(IAlertaPerfilTransaccionalDao alertaPerfilTransaccionalDao)
            : base(alertaPerfilTransaccionalDao)
        {
            _alertaPerfilTransaccionalDao = alertaPerfilTransaccionalDao;
        }
        public IQueryable<dynamic> GetAlertaPerfilTransaccional(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _alertaPerfilTransaccionalDao.GetAlertaPerfilTransaccional(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de Alertas de Perfil Transaccional: " + e);
                throw new Exception("Error al obtener lista de Alertas de Perfil Transaccional");
            }
        }
           public int NotificarPerfilTransaccional()
        {
            return _alertaPerfilTransaccionalDao.NotificarPerfilTransaccional();
        }
    }
}
