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
    public class CargoInstitucionBlo : GenericBlo<MON_CARGO_INSTITUCION>, ICargoInstitucionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICargoInstitucionDao _cargoInstitucionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CargoInstitucionBlo(ICargoInstitucionDao cargoInstitucionDao)
            : base(cargoInstitucionDao)
        {
            _cargoInstitucionDao = cargoInstitucionDao;
        }
    }
}
