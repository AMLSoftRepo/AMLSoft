using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public interface IRosDao : IGenericDao<MON_ROS>
    {
        List<MON_ROS> GetCodUIF(string  codigo);
    }
}
