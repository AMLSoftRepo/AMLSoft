using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Perfiles;

namespace Blo.Perfiles
{
    public class HistorialPerfilBlo : GenericBlo<PER_HISTORIAL_PERFIL>, IHistorialPerfilBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IHistorialPerfilDao _historialPerfilDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public HistorialPerfilBlo(IHistorialPerfilDao historialPerfilDao)
            : base(historialPerfilDao)
        {
            _historialPerfilDao = historialPerfilDao;
        }

    }
}
