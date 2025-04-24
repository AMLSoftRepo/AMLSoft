using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Seguridad;
using Ninject;

namespace Blo.Seguridad
{
    public class OpcionBlo : GenericBlo<SEG_OPCION>, IOpcionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IOpcionDao _opcionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public OpcionBlo(IOpcionDao opcionDao)
            : base(opcionDao)
        {
            _opcionDao = opcionDao;
        }
    }
}
