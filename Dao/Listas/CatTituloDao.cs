using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public class CatTituloDao:GenericDao<LIS_CAT_TITULOS>,ICatTituloDao 
    {
        public  List<LIS_CAT_TITULOS> GetCodigoTitulo(int codigo)
        {
            List<LIS_CAT_TITULOS> lista = new List<LIS_CAT_TITULOS>();
            try
            {
                lista = _SQLBDEntities.LIS_CAT_TITULOS
                        .Where(x => x.ID == codigo)
                        .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de titulos de PEP", e);
            }
            return lista;
        }
    }
}
