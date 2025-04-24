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
    public class PEPRelacionBlo : GenericBlo<LIS_PEP_RELACION>, IPEPRelacionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPEPRelacionDao _pepRelacionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PEPRelacionBlo(IPEPRelacionDao pepRelacionDao)
            : base(pepRelacionDao)
        {
            _pepRelacionDao = pepRelacionDao;
        }
    }
}
