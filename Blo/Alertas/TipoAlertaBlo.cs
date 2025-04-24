using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;

namespace Blo.Alertas
{
    public class TipoAlertaBlo : GenericBlo<ALE_TIPO_ALERTA>, ITipoAlertaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ITipoAlertaDao _tipoAlertaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public TipoAlertaBlo(ITipoAlertaDao tipoAlertaDao)
            : base(tipoAlertaDao)
        {
            _tipoAlertaDao = tipoAlertaDao;
        }

    }
}
