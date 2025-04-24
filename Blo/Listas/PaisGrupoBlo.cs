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
    public class PaisGrupoBlo:GenericBlo<LIS_PAIS_GRUPO>,IPaisGrupoBlo
    {
           private IPaisGrupoDao _paisGrupoDao;
         [Inject]
          public PaisGrupoBlo(IPaisGrupoDao paisGrupoDao)
              : base(paisGrupoDao)
        {
            _paisGrupoDao = paisGrupoDao;
        }

         public List<LIS_PAIS_GRUPO> GetCodigoPaisGrupo(int codigo)
        {
            List<LIS_PAIS_GRUPO> lista = new List<LIS_PAIS_GRUPO>();

            try
            {
                lista = _paisGrupoDao.GetCodigoPaisGrupo(codigo);
            }
            catch  (Exception e)
            {
                log.Error("Error cargando listado de codigos paises miembros del Grupo", e);
            }

            return lista;
        }
    }
}
