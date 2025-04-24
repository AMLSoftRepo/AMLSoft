using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Listas;

namespace Blo.Listas
{
   public class CatTituloBlo:GenericBlo<LIS_CAT_TITULOS>,ICatTituloBlo 
    {
       private ICatTituloDao _catTituloDao;

         [Inject]
        public CatTituloBlo(ICatTituloDao catTituloDao)
            : base(catTituloDao)
        {
            _catTituloDao = catTituloDao;
        }

         public List<LIS_CAT_TITULOS> GetCodigoTitulo(int codigo)
        {
            List<LIS_CAT_TITULOS> lista = new List<LIS_CAT_TITULOS>();

            try
            {
                lista = _catTituloDao.GetCodigoTitulo(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Titulos del Estado", e);
            }

            return lista;
        }
    }
}
