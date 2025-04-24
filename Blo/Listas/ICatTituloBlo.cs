using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Listas
{
   public interface ICatTituloBlo : IGenericBlo<LIS_CAT_TITULOS>
    {
       List<LIS_CAT_TITULOS> GetCodigoTitulo(int codigo);
    }
}
