using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Perfiles;

namespace Blo.Perfiles
{
    public class CoincidenciaListaBlo : GenericBlo<PER_COINCIDENCIA_LISTA>, ICoincidenciaListaBlo
    {
        private ICoincidenciaListaDao _coincidenciaListaDao;

        [Inject]
        public CoincidenciaListaBlo(ICoincidenciaListaDao coincidenciaListaDao)
            : base(coincidenciaListaDao)
        {
            _coincidenciaListaDao = coincidenciaListaDao;
        }

        public List<PER_COINCIDENCIA_LISTA> GetCodigoCoincidencia(int codigo)
        {
            List<PER_COINCIDENCIA_LISTA> lista = new List<PER_COINCIDENCIA_LISTA>();

            try
            {
                lista = _coincidenciaListaDao.GetCodigoCoincidencia(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de coincidencia", e);
            }

            return lista;
        }
    }
}
