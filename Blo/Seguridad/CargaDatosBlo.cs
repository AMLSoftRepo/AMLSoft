using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Seguridad;

namespace Blo.Seguridad
{
    public class CargaDatosBlo : GenericBlo<SEG_CARGAR_DATOS>, ICargaDatosBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICargaDatosDao _cargaDatosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CargaDatosBlo(ICargaDatosDao cargaDatosDao)
            : base(cargaDatosDao)
        {
            _cargaDatosDao = cargaDatosDao;
        }
    }
}
