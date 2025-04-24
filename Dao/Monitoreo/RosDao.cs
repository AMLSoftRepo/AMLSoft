using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
   public class RosDao:GenericDao<MON_ROS>,IRosDao 
    {
       public List<MON_ROS> GetCodUIF(string  codigo)
       {
           List<MON_ROS> lista = new List<MON_ROS>();

           try
           {
               lista = _SQLBDEntities.MON_ROS
                                   .Where(x => x.CODIGO_UIF == codigo)
                                   .ToList();
           }
           catch (Exception e)
           {
               log.Error("Error cargando listado de codigos de ROS UIF", e);
           }

           return lista;
       }
    }
}
