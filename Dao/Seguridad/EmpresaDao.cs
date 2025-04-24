using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public class EmpresaDao: GenericDao<SEG_EMPRESA>,IEmpresaDao
    {
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
                lista = _SQLBDEntities.SEG_EMPRESA
                                    .Where(x => x.CODIGO == codigo)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de roles de usurio", e);
            }

            return lista;
        }
    }
}
