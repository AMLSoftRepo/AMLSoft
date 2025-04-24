using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Seguridad;

namespace Blo.Seguridad
{
    public class EmpresaBlo : GenericBlo<SEG_EMPRESA>, IEmpresaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IEmpresaDao _empresaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public EmpresaBlo(IEmpresaDao empresaDao)
            : base(empresaDao)
        {
            _empresaDao = empresaDao;
        }

        /// <summary>
        /// Obtiene la empresa por su codigo
        /// </summary>
        /// <param name="user">codigo de empresa</param>
        /// <returns>Lista de SEG_EMPRESA</returns>
        public List<SEG_EMPRESA> GetEmpresaByCodigo(string codigo)
        {
            List<SEG_EMPRESA> lista = new List<SEG_EMPRESA>();

            try
            {
                lista = _empresaDao.GetEmpresaByCodigo(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de roles de usurio", e);
            }

            return lista;
        }
    }
}
