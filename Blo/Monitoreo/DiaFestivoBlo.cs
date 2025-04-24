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
    public class DiaFestivoBlo : GenericBlo<MON_DIA_FESTIVO>, IDiaFestivoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IDiaFestivoDao _diaFestivoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public DiaFestivoBlo(IDiaFestivoDao diaFestivoDao)
            : base(diaFestivoDao)
        {
            _diaFestivoDao = diaFestivoDao;
        }
    }
}
