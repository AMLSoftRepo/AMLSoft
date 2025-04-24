using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public class CatFinancieraDao: GenericDao<MON_CAT_FINANCIERA>,ICatFinancieraDao
    {
        public List<MON_CAT_FINANCIERA> GetCodFinanciera(int  codigo)
        {
            List<MON_CAT_FINANCIERA> lista = new List<MON_CAT_FINANCIERA>();

            try
            {
                lista = _SQLBDEntities.MON_CAT_FINANCIERA
                                    .Where(x => x.CODIGO_FINANCIERA == codigo)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de financieras", e);
            }

            return lista;
        }

        public List<MON_CAT_FINANCIERA> ExistCodFinanciera(int id,int codigo)
        {
            List<MON_CAT_FINANCIERA> lista = new List<MON_CAT_FINANCIERA>();

            try
            {
                lista = _SQLBDEntities.MON_CAT_FINANCIERA
                                    .Where(x => x.ID != id && x.CODIGO_FINANCIERA == codigo)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de financieras", e);
            }

            return lista;
        }

    }
}
