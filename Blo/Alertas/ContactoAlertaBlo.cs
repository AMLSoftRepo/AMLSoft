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
    public class ContactoAlertaBlo : GenericBlo<ALE_CONTACTO_ALERTA>, IContactoAlertaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IContactoAlertaDao _contactoAlertaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ContactoAlertaBlo(IContactoAlertaDao contactoAlertaDao)
            : base(contactoAlertaDao)
        {
            _contactoAlertaDao = contactoAlertaDao;
        }
    }
}
