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
    public class CatOrganoBlo:GenericBlo<LIS_CAT_ORGANOS>,ICatOrganoBlo
    {
        private ICatOrganoDao _catOrganoDao;
         [Inject]
        public CatOrganoBlo(ICatOrganoDao catOrganoDao)
            : base(catOrganoDao)
        {
            _catOrganoDao = catOrganoDao;
        }

        public List<LIS_CAT_ORGANOS> GetCodigoOrgano(int codigo)
        {
            List<LIS_CAT_ORGANOS> lista = new List<LIS_CAT_ORGANOS>();

            try
            {
                lista = _catOrganoDao.GetCodigoOrgano(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de descripcion de Organos del Estado", e);
            }

            return lista;
        }
        
    }
}
