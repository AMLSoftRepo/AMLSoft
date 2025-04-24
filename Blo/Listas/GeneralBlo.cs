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
    public class GeneralBlo : GenericBlo<LIS_GENERAL>, IGeneralBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IGeneralDao _generalDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public GeneralBlo(IGeneralDao generalDao)
            : base(generalDao)
        {
            _generalDao = generalDao;
        }
    }
}
