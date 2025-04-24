using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;
using Ninject;

namespace Blo.Matriz
{
    public class ControlBlo : GenericBlo<MAT_CONTROL>, IControlBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IControlDao _controlDao;
        private ICatDisenoDao _catDisenoDao;
        private ICatAutomatizacionDao _catAutomatizacionDao;
        private ICatDocumentacionDao _catDocumentacionDao;
        private ICatFrecuenciaDao _catFrecuenciaDao;
        private ICatMezclaDao _catMezclaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ControlBlo(IControlDao controlDao, ICatDisenoDao catDisenoDao,
         ICatAutomatizacionDao catAutomatizacionDao, ICatDocumentacionDao catDocumentacionDao,
         ICatFrecuenciaDao catFrecuenciaDao, ICatMezclaDao catMezclaDao)
            : base(controlDao)
        {
            _controlDao = controlDao;
            _catDisenoDao = catDisenoDao;
            _catAutomatizacionDao = catAutomatizacionDao;
            _catDocumentacionDao = catDocumentacionDao;
            _catFrecuenciaDao = catFrecuenciaDao;
            _catMezclaDao = catMezclaDao;
        }


        /// <summary>
        /// Metodo que permite calcular el total del porcentaje aplicado a un control 
        /// </summary>
        /// <param name="control">objeto de MAT_CONTROL</param>
        public void CalcularTotalPorcentaje(ref MAT_CONTROL control)
        {
            control.TOTAL_POR = 0;

            try
            {
                control.TOTAL_POR = _catDisenoDao.GetById(control.ID_DISENO).VALOR +
                                    _catAutomatizacionDao.GetById(control.ID_AUTOMATIZACION).VALOR +
                                    _catDocumentacionDao.GetById(control.ID_DOCUMENTACION).VALOR +
                                    _catFrecuenciaDao.GetById(control.ID_FRECUENCIA).VALOR +
                                    _catMezclaDao.GetById(control.ID_MEZCLA).VALOR;
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de controles</returns>
        public IQueryable<dynamic> GetControles(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _controlDao.GetControles(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }

    }
}
