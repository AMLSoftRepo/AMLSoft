using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;

namespace Blo.Monitoreo
{
    public class CatFinancieraBlo : GenericBlo<MON_CAT_FINANCIERA>, ICatFinancieraBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatFinancieraDao _catFinancieraDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatFinancieraBlo(ICatFinancieraDao catFinancieraDao)
            : base(catFinancieraDao)
        {
            _catFinancieraDao = catFinancieraDao;
        }


        public List<MON_CAT_FINANCIERA> GetCodFinanciera(int codigo)
        {
            List<MON_CAT_FINANCIERA> lista = new List<MON_CAT_FINANCIERA>();

            try
            {
                lista = _catFinancieraDao.GetCodFinanciera(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de financieras", e);
            }

            return lista;
        }


        public List<MON_CAT_FINANCIERA> ExistCodFinanciera(int id, int codigo)
        {
            List<MON_CAT_FINANCIERA> lista = new List<MON_CAT_FINANCIERA>();

            try
            {
                lista = _catFinancieraDao.ExistCodFinanciera(id,codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de financieras", e);
            }

            return lista;
        }

    }
}
