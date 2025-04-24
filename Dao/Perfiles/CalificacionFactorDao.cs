using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Perfiles
{
    public class CalificacionFactorDao : GenericDao<PER_CALIFICACION_FACTOR>, ICalificacionFactorDao
    {
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
            VIEW_CLIENTE cliente = new VIEW_CLIENTE();
            List<VIEW_CLIENTE_DIRECCIONES> clienteDireccion = new List<VIEW_CLIENTE_DIRECCIONES>();
            List<PER_CONFIGURACION_FACTOR> confFactor = new List<PER_CONFIGURACION_FACTOR>();
            List<PER_CALIFICACION_FACTOR> calificacionFactor = new List<PER_CALIFICACION_FACTOR>();
            List<PER_CALIFICACION_FACTOR> listCalificacion = new List<PER_CALIFICACION_FACTOR>();
            int valor = 0;

            try
            {
                //Cliente
                cliente = _SQLBDEntities.VIEW_CLIENTE
                          .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                          .FirstOrDefault();

                //Direcciónprincipal del cliente
                clienteDireccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES
                                   .Where(x => x.CODIGO_CLIENTE == codigoCliente && x.DIRECCION_PRINCIPAL == "S")
                                   .ToList();

                //Si no hay una dirección principal seleccionar todas
                if (!clienteDireccion.Any())
                    clienteDireccion = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES
                                      .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                                      .ToList();


                foreach (var item in _SQLBDEntities.PER_FACTOR)
                {
                    PER_CALIFICACION_FACTOR calificacion = new PER_CALIFICACION_FACTOR();

                    //Valores por default por factor
                    confFactor = _SQLBDEntities.PER_CONFIGURACION_FACTOR.Where(x => x.ID_FACTOR == item.ID).ToList();


                    //Evaluación de factores
                    if (item.ID == 1)
                        calificacionFactor = _SQLBDEntities.PER_CALIFICACION_FACTOR
                                             .Where(x => x.ID_ITEM == item.ID + "-" + cliente.CODIGO_TIPO_CLIENTE)
                                             .ToList();
                    else if (item.ID == 2)
                        calificacionFactor = _SQLBDEntities.PER_CALIFICACION_FACTOR
                                             .Where(x => x.ID_ITEM == item.ID + "-" + cliente.CODIGO_ACTIVIDAD_ECONOMICA)
                                             .ToList();
                    else if (item.ID == 3)
                        calificacionFactor = _SQLBDEntities.PER_CALIFICACION_FACTOR
                                             .Where(x => x.ID_ITEM == item.ID + "-" + cliente.CODIGO_ACTIVIDAD_ECONOMICA + "-" +
                                                    cliente.CODIGO_CLASE_ACTIVIDAD_ECONOMICA + "-" + cliente.CODIGO_SUB_ACTIVIDAD_ECONOMICA)
                                             .ToList();
                    else if (item.ID == 4)
                        calificacionFactor = _SQLBDEntities.PER_CALIFICACION_FACTOR
                                             .Where(x => x.ID_ITEM == item.ID + "-" + cliente.CODIGO_PROFESION)
                                             .ToList();
                    else if (item.ID == 5)
                    {
                        if (clienteDireccion.Any())
                            calificacionFactor = _SQLBDEntities.PER_CALIFICACION_FACTOR.ToList()
                                                 .Where(x => x.ID_ITEM == item.ID + "-" + clienteDireccion.First().CODIGO_PAIS + "-" +
                                                         clienteDireccion.First().CODIGO_DEPARTAMENTO + "-" + clienteDireccion.First().CODIGO_MUNICIPIO + "-" +
                                                         clienteDireccion.First().CODIGO_SECTOR)
                                                 .ToList();
                    }


                    if (!calificacionFactor.Any())
                        valor = confFactor.First().VALOR; // valor por default
                    else
                        valor = calificacionFactor.First().PUNTAJE; //Valor asigando


                    calificacion.PUNTAJE = valor;
                    calificacion.ID_FACTOR = item.ID;
                    calificacion.PER_FACTOR = item;
                    listCalificacion.Add(calificacion);
                }
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de calificación de factores por cliente: " + e);
                listCalificacion = null ;
            }

            return listCalificacion;
        }

    }
}
