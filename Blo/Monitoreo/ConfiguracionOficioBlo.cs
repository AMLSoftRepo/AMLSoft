using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Monitoreo;
using Ninject;

namespace Blo.Monitoreo
{
    public class ConfiguracionOficioBlo : GenericBlo<MON_OFICIO_CONFIGURACION>, IConfiguracionOficioBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IConfiguracionOficioDao _configuracionOficioDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ConfiguracionOficioBlo(IConfiguracionOficioDao configuracionOficioDao)
            : base(configuracionOficioDao)
        {
            _configuracionOficioDao = configuracionOficioDao;
        }
    }
}
