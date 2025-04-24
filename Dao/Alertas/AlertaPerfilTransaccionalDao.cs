using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public class AlertaPerfilTransaccionalDao : GenericDao<ALE_ALERTA_PERFIL_TRAN>, IAlertaPerfilTransaccionalDao
    {
         public IQueryable<dynamic> GetAlertaPerfilTransaccional(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
         {
          try
            {
                List<ALE_ALERTA_PERFIL_TRAN> lista = new List<ALE_ALERTA_PERFIL_TRAN>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.ALE_ALERTA_PERFIL_TRAN
                         .AsNoTracking()
                         .Where(x => (
                                   x.ID_CLIENTE + " " +
                                   x.NOMBRE_CLIENTE + " " +
                                   x.EVENTO_ALERTA + " " + 
                                   x.DESCRIPCION_ALERTA).ToUpper().Contains(searchString.Trim().ToUpper()))
                         .OrdenarGrid(sortBy, direction)
                         .Skip(start)
                         .Take(limit.Value)
                         .ToList();


                    total = _SQLBDEntities.ALE_ALERTA_PERFIL_TRAN
                         .AsNoTracking()
                         .Where(x => (
                                   x.ID_CLIENTE + " " +
                                   x.NOMBRE_CLIENTE + " " +
                                   x.EVENTO_ALERTA  + " " +
                                   x.DESCRIPCION_ALERTA).ToUpper().Contains(searchString.Trim().ToUpper()))
                         .Count();
                }
                else
                {
                    lista = _SQLBDEntities.ALE_ALERTA_PERFIL_TRAN
                        .AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.ALE_ALERTA_PERFIL_TRAN
                        .AsNoTracking()
                        .Count();
                }

                return lista.AsQueryable();

            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de Alertas de Perfil transaccional: " + e);
                throw new Exception("Error al obtener lista de Alertas de Perfil transaccional");
            }
        }

         public int NotificarPerfilTransaccional()
         {
             int total = 0;
             try
             {
                 total = _SQLBDEntities.ALE_ALERTA_PERFIL_TRAN.AsNoTracking().Count();
             }
             catch (Exception e)
             {
                 log.Error("Error al obtener el total Alertas de Perfil Transaccional " + e);
             }

             return total;
         }
    }

}
