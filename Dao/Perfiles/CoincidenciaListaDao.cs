using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public class CoincidenciaListaDao:GenericDao<PER_COINCIDENCIA_LISTA>,ICoincidenciaListaDao
    {
        public List<PER_COINCIDENCIA_LISTA> GetCodigoCoincidencia(int codigo)
        {
            List<PER_COINCIDENCIA_LISTA> lista = new List<PER_COINCIDENCIA_LISTA>();
            try
            {
                lista = _SQLBDEntities.PER_COINCIDENCIA_LISTA
                        .Where(x => x.ID == codigo)
                        .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos coincidencias" + e);
            }
            return lista;
        }
    }
}
