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
    public class CatGrupoBlo:GenericBlo<LIS_CAT_GRUPO_FATF>,ICatGrupoBlo 
    {
          private ICatGrupoDao _catGrupoDao;
         [Inject]
          public CatGrupoBlo(ICatGrupoDao catGrupoDao)
              : base(catGrupoDao)
        {
            _catGrupoDao = catGrupoDao;
        }

         public List<LIS_CAT_GRUPO_FATF> GetCodigoGrupo(int codigo)
        {
            List<LIS_CAT_GRUPO_FATF> lista = new List<LIS_CAT_GRUPO_FATF>();

            try
            {
                lista = _catGrupoDao.GetCodigoGrupo(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Grupo del FATF", e);
            }

            return lista;
        }
    }
}
