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
    public class SDNNacionalidadBlo : GenericBlo<LIS_SDN_NACIONALIDAD>, ISDNNacionalidadBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISDNNacionalidadDao _sdnNacionalidadDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SDNNacionalidadBlo(ISDNNacionalidadDao sdnNacionalidadDao)
            : base(sdnNacionalidadDao)
        {
            _sdnNacionalidadDao = sdnNacionalidadDao;
        }
    }
}
