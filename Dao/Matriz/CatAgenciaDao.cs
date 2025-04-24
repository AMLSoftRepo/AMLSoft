using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public class CatAgenciaDao: GenericDao<MAT_CAT_AGENCIA>,ICatAgenciaDao
    {
        public List<MAT_CAT_AGENCIA> GetCodAgencia(int codigo)
        {
            List<MAT_CAT_AGENCIA> lista = new List<MAT_CAT_AGENCIA>();

            try
            {
                lista = _SQLBDEntities.MAT_CAT_AGENCIA
                                    .Where(x => x.CODIGO_AGENCIA == codigo)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Agencias", e);
            }

            return lista;
        }

        public List<MAT_CAT_AGENCIA> ExistCodAgencia(int id, int codigo)
        {
            List<MAT_CAT_AGENCIA> lista = new List<MAT_CAT_AGENCIA>();

            try
            {
                lista = _SQLBDEntities.MAT_CAT_AGENCIA
                                    .Where(x => x.ID != id && x.CODIGO_AGENCIA == codigo)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Agencias", e);
            }

            return lista;
        }

    }
}
