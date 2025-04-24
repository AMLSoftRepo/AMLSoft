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
    public class CatInstitucionBlo : GenericBlo<MON_CAT_INSTITUCION>, ICatInstitucionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatInstitucionDao _catInstitucionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatInstitucionBlo(ICatInstitucionDao catInstitucionDao)
            : base(catInstitucionDao)
        {
            _catInstitucionDao = catInstitucionDao;
        }
    }
}
