using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Monitoreo
{
    public interface IRosBlo : IGenericBlo<MON_ROS>
    {
        /// <summary>
        /// Metodos para ROS
        /// </summary>

        List<MON_ROS> GetCodUIF(string  codigo);
      
    }
}
