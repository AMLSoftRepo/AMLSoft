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
    public class CatEntidadBlo:GenericBlo<LIS_CAT_ENTIDADES>,ICatEntidadBlo
    {
         private ICatEntidadDao _catEntidadDao;
         [Inject]
        public CatEntidadBlo(ICatEntidadDao catEntidadDao)
            : base(catEntidadDao)
        {
            _catEntidadDao = catEntidadDao;
        }

         public List<LIS_CAT_ENTIDADES> GetCodigoEntidad(int codigo)
        {
            List<LIS_CAT_ENTIDADES> lista = new List<LIS_CAT_ENTIDADES>();

            try
            {
                lista = _catEntidadDao.GetCodigoEntidad(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Entidades del Estado", e);
            }

            return lista;
        }
    }
}
