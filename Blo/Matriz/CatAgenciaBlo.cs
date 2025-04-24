using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Matriz;

namespace Blo.Matriz
{
    public class CatAgenciaBlo : GenericBlo<MAT_CAT_AGENCIA>, ICatAgenciaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatAgenciaDao _catAgenciaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatAgenciaBlo(ICatAgenciaDao catAgenciaDao)
            : base(catAgenciaDao)
        {
            _catAgenciaDao = catAgenciaDao;
        }

        public List<MAT_CAT_AGENCIA> GetCodAgencia(int codigo)
        {
            List<MAT_CAT_AGENCIA> lista = new List<MAT_CAT_AGENCIA>();

            try
            {
                lista = _catAgenciaDao.GetCodAgencia(codigo);
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
                lista = _catAgenciaDao.ExistCodAgencia(id, codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de Agencias", e);
            }

            return lista;
        }

    }
   
}
