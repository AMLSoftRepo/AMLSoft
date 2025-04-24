using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public interface ICalificacionFactorDao : IGenericDao<PER_CALIFICACION_FACTOR>
    {
        /// <summary>
        /// Metodo que permite obtener la calificación de cada uno de los factores
        /// 
        /// ID	DESCRIPCION
        /// 1	TIPO DE CLIENTE
        /// 2	ACTIVIDAD ECONOMICA
        /// 3	CLASE ACTIVIDAD ECONOMICA
        /// 4	SUBCLASE ACTIVIDAD ECONOMICA
        /// 5	SECTOR ECONOMICO
        /// 6	PROFESION
        /// 7	GEOGRAFICO
        /// 
        /// Esto con el objetivo de medir el grado de riesgo para cada cliente
        /// </summary>
        /// <param name="codigoCliente">Identificador unico del cliente</param>
        /// <returns>Lista de PER_CALIFICACION_FACTOR</returns>
        List<PER_CALIFICACION_FACTOR> calificacionPorCodigoCLiente(long codigoCliente);
    }
}
