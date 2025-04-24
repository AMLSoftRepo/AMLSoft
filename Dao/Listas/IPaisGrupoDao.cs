using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
  public  interface IPaisGrupoDao:IGenericDao<LIS_PAIS_GRUPO>
    {
      List<LIS_PAIS_GRUPO> GetCodigoPaisGrupo(int codigo);
    }
}
