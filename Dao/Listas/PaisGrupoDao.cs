using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
   public class PaisGrupoDao:GenericDao<LIS_PAIS_GRUPO>,IPaisGrupoDao
    {
       public List<LIS_PAIS_GRUPO> GetCodigoPaisGrupo(int codigo)
       {
           List<LIS_PAIS_GRUPO> lista = new List<LIS_PAIS_GRUPO>();
           try
           {
               lista = _SQLBDEntities.LIS_PAIS_GRUPO
                       .Where(x => x.ID == codigo)
                       .ToList();
           }
           catch (Exception e)
           {
               log.Error("Error cargando listado de codigos de grupo paises miembros del grupo ",e);
           }
           return lista;
       }
    }
}
