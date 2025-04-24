using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public interface IAlertaPerfilTransaccionalDao: IGenericDao<ALE_ALERTA_PERFIL_TRAN>
    {
        IQueryable<dynamic> GetAlertaPerfilTransaccional(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);

        int NotificarPerfilTransaccional();
    }
}
