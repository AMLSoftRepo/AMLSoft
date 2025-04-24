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
    public class CalificacionFactorBlo : GenericBlo<PER_CALIFICACION_FACTOR>, ICalificacionFactorBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICalificacionFactorDao _calificacionFactorDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CalificacionFactorBlo(ICalificacionFactorDao calificacionFactorDao)
            : base(calificacionFactorDao)
        {
            _calificacionFactorDao = calificacionFactorDao;
        }

        /// <summary>
        /// Metodo que permite obtener la calificación de cada uno de los factores
        /// 
        /// ID	DESCRIPCION
        /// 1	TIPO DE CLIENTE
        /// 2	ACTIVIDAD ECONOMICA
        /// 3	SECTOR ECONOMICO
        /// 4	PROFESION
        /// 5	GEOGRAFICO
        /// 
        /// Esto con el objetivo de medir el grado de riesgo para cada cliente
        /// </summary>
        /// <param name="codigoCliente">Identificador unico del cliente</param>
        /// <returns>Lista de PER_CALIFICACION_FACTOR</returns>
        public List<PER_CALIFICACION_FACTOR> calificacionPorCodigoCLiente(long codigoCliente)
        {
            List<PER_CALIFICACION_FACTOR> listCalificacion = new List<PER_CALIFICACION_FACTOR>();

            try
            {
                listCalificacion = _calificacionFactorDao.calificacionPorCodigoCLiente(codigoCliente);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de calificación de factores por cliente: " + e);
            }

            return listCalificacion;
        }

    }
}
