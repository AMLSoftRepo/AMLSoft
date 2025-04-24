using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Listas
{
    public class PEPDao : GenericDao<LIS_PEP>, IPEPDao
    {

        /// <summary>
        /// Metodo que permite obtener una lista de de PEP's.
        /// Ademas realiza la paginación de los datos, permite buscar por varios campos de un PEP
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de PEP's</returns>
        public IQueryable<dynamic> GetPEP(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                List<LIS_PEP> lista = new List<LIS_PEP>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    lista = _SQLBDEntities.LIS_PEP.AsNoTracking()
                        .Where(x => (
                                  (x.PRIMER_NOMBRE + " " + x.SEGUNDO_NOMBRE + " " + x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.APELLIDO_CASADA) + " " +
                                  x.CONOCIDO_POR + " " +
                                  x.DUI + " " +
                                  x.NIT + " " +
                                  x.PASAPORTE + " " +
                                  x.CARNET_RESIDENTE + " " +
                                  x.FUNCIONARIO_O_RELACION + " " +
                                  x.DECLARACION_JURADA).ToUpper().Contains(searchString.Trim().ToUpper()))
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();

                    total = _SQLBDEntities.LIS_PEP.AsNoTracking()
                        .Where(x => (
                                  (x.PRIMER_NOMBRE + " " + x.SEGUNDO_NOMBRE + " " + x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.APELLIDO_CASADA) + " " +
                                  x.CONOCIDO_POR + " " +
                                  x.DUI + " " +
                                  x.NIT + " " +
                                  x.PASAPORTE + " " +
                                  x.CARNET_RESIDENTE + " " +
                                  x.FUNCIONARIO_O_RELACION + " " +
                                  x.DECLARACION_JURADA).ToUpper().Contains(searchString.Trim().ToUpper()))
                        .Count();
                }
                else
                {
                    lista = _SQLBDEntities.LIS_PEP.AsNoTracking()
                        .OrdenarGrid(sortBy, direction)
                        .Skip(start)
                        .Take(limit.Value)
                        .ToList();


                    total = _SQLBDEntities.LIS_PEP.AsNoTracking()
                        .Count();
                }

                var records = lista.Select(x => new
                   {
                       x.ID,
                       TITULO = x.TITULO_CARGO_PEP!=null? x.LIS_CAT_TITULOS.ABREVIATURA.Trim():"",
                       NOMBRE = x.PRIMER_NOMBRE + " " + x.SEGUNDO_NOMBRE + " " + x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.APELLIDO_CASADA,
                       x.TITULO_CARGO_PEP,
                       x.PRIMER_NOMBRE,
                       x.SEGUNDO_NOMBRE,
                       x.PRIMER_APELLIDO,
                       x.SEGUNDO_APELLIDO,
                       x.APELLIDO_CASADA,
                       x.CONOCIDO_POR,
                       DOCUMENTO = (x.DUI != null ? "DUI: " + x.DUI :
                                    (x.NIT != null ? "NIT: " + x.NIT :
                                    (x.PASAPORTE != null ? "PASAPORTE: " + x.PASAPORTE :
                                    (x.CARNET_RESIDENTE != null ? "CARNET RESIDENTE: " + x.CARNET_RESIDENTE : "SIN DOCUMENTO")))),
                       x.DUI,
                       x.NIT,
                       x.PASAPORTE,
                       x.CARNET_RESIDENTE,
                       x.FUNCIONARIO_O_RELACION,
                       x.DECLARACION_JURADA
                   }).AsQueryable();

                return records;

            }
            catch (Exception e)
            {
                log.Error("Error al obtener los oficios historicos: " + e);
                throw new Exception("Error al obtener los oficios historicos");
            }
        }

    }
}
