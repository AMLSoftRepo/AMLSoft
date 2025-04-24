using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;
using Blo.Alertas;
using Ninject;
using System.Transactions;
using System.Threading;

namespace Blo.Matriz
{
    public class MatrizBlo : GenericBlo<MAT_MATRIZ>, IMatrizBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IMatrizDao _matrizDao;
        private IMatrizControlEventoDao _matrizControlEventoDao;
        private IMatrizControlDao _matrizControlDao;
        private IMatrizEventoRiesgoDao _matrizEventoRiesgoDao;
        private IControlEventoDao _controlEventoDao;
        private IControlDao _controlDao;
        private IEventoRiesgoDao _eventoRiesgoDao;
        private IEscalaCalificacionDao _escalaCalificacionDao;


        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public MatrizBlo(IMatrizDao matrizDao, IMatrizControlEventoDao matrizControlEventoDao, IMatrizControlDao matrizControlDao,
            IMatrizEventoRiesgoDao matrizEventoRiesgoDao, IControlEventoDao controlEventoDao, IControlDao controlDao,
            IEventoRiesgoDao eventoRiesgoDao, IEscalaCalificacionDao escalaCalificacionDao)
            : base(matrizDao)
        {
            _matrizDao = matrizDao;
            _matrizControlDao = matrizControlDao;
            _matrizControlEventoDao = matrizControlEventoDao;
            _matrizEventoRiesgoDao = matrizEventoRiesgoDao;
            _controlEventoDao = controlEventoDao;
            _controlDao = controlDao;
            _eventoRiesgoDao = eventoRiesgoDao;
            _escalaCalificacionDao = escalaCalificacionDao;
        }


        /// <summary>
        /// Metodo que permite generar matriz por agencia, partiendo de los eventos y controles
        /// que se asignaron a dicho evento, finalmente este proceso deja limpia estas dos entidades
        /// para luego iniciar nuevamente el proceso de creación de la matriz de riesgo
        /// </summary>
        /// <param name="idAgencia">I</param>
        public long GenerarMatriz()
        {
            List<MAT_EVENTO_RIESGO> listEventoRiesgo = new List<MAT_EVENTO_RIESGO>();
            List<MAT_CONTROL> listControles = new List<MAT_CONTROL>();
            MAT_MATRIZ matriz = new MAT_MATRIZ();
            List<MAT_MATRIZ_CONTROL> listMatrizControl = new List<MAT_MATRIZ_CONTROL>();
            List<long> idsControlEvento = new List<long>();
            String usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    //Lista de eventos
                    listEventoRiesgo = _eventoRiesgoDao.GetAll(true).ToList();

                    if (usuarioIdentity.Contains("\\"))
                        usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

                    if (listEventoRiesgo.Any())
                    {
                        matriz.FECHA = DateTime.Now;
                        matriz.USUARIO = usuarioIdentity;
                        _matrizDao.Save(matriz);


                        //Obtener lista de ids de todos los controles asociados a algun evento
                        idsControlEvento = _controlEventoDao.GetAll().Select(x => x.ID_CONTROL).ToList();

                        //Obtener solo aquellos controles que son usados por uno o mas eventos
                        listControles = _controlDao.GetAll(true).Where(x => idsControlEvento.Contains(x.ID)).ToList();


                        //Controles
                        foreach (var itemControl in listControles)
                        {
                            MAT_MATRIZ_CONTROL matrizControl = new MAT_MATRIZ_CONTROL();

                            matrizControl.ID_AGENCIA = itemControl.ID_AGENCIA;
                            matrizControl.ID_MATRIZ = matriz.ID;
                            matrizControl.ID_CONTROL_ANTERIOR = itemControl.ID;
                            matrizControl.AUTOMATIZACION = itemControl.MAT_CAT_AUTOMATIZACION.DESCRIPCION;
                            matrizControl.DESCRIPCION = itemControl.DESCRIPCION;
                            matrizControl.DISENO = itemControl.MAT_CAT_DISENO.DESCRIPCION;
                            matrizControl.DOCUMENTACION = itemControl.MAT_CAT_DOCUMENTACION.DESCRIPCION;
                            matrizControl.FRECUENCIA = itemControl.MAT_CAT_FRECUENCIA.DESCRIPCION;
                            matrizControl.MEZCLA = itemControl.MAT_CAT_MEZCLA.DESCRIPCION;
                            matrizControl.OBSERVACIONES = itemControl.OBSERVACIONES;
                            matrizControl.TIPO_CONTROL = itemControl.MAT_CAT_TIPO_CONTROL.DESCRIPCION;
                            matrizControl.TOTAL_POR = itemControl.TOTAL_POR;
                            _matrizControlDao.SaveNoTrack(matrizControl);
                        }


                        //Eventos
                        foreach (var itemEvento in listEventoRiesgo)
                        {
                            MAT_MATRIZ_EVENTO_RIESGO matrizEvento = new MAT_MATRIZ_EVENTO_RIESGO();

                            matrizEvento.ID_AGENCIA = itemEvento.ID_AGENCIA;
                            matrizEvento.ID_MATRIZ = matriz.ID;
                            matrizEvento.ID_EVENTO_ANTERIOR = itemEvento.ID;
                            matrizEvento.CAUSA_RIESGO = itemEvento.MAT_CAUSA_RIESGO.DESCRIPCION;
                            matrizEvento.COMO = itemEvento.COMO;
                            matrizEvento.FACTOR_RIESGO = itemEvento.MAT_CAT_FACTOR_RIESGO.DESCRIPCION;
                            matrizEvento.IMPACTO = itemEvento.MAT_CAT_SEVERIDAD.DESCRIPCION;
                            matrizEvento.PROBABILIDAD_OCURRENCIA = itemEvento.MAT_CAT_PROBABILIDAD_OCURRENCIA.CLASIFICACION;
                            matrizEvento.RIESGO = itemEvento.MAT_RIESGO.DESCRIPCION;
                            matrizEvento.UNIDAD = itemEvento.MAT_CAT_UNIDAD.DESCRIPCION;
                            matrizEvento.DESCRIPCION = itemEvento.DESCRIPCION;
                            matrizEvento.EFICACIA_CONTROL = _eventoRiesgoDao.EficaciaControl(itemEvento.ID);
                            matrizEvento.RIESGO_INHERENTE = _eventoRiesgoDao.RiesgoInherente(
                                                                itemEvento.MAT_CAT_PROBABILIDAD_OCURRENCIA.NIVEL,
                                                                itemEvento.MAT_CAT_SEVERIDAD.NIVEL);
                            matrizEvento.COLOR_RIESGO_INHERENTE = _escalaCalificacionDao.ObtenerColorXValor(matrizEvento.RIESGO_INHERENTE);
                            matrizEvento.RIESGO_RESIDUAL = _eventoRiesgoDao.RiesgoResidual(matrizEvento.RIESGO_INHERENTE, matrizEvento.EFICACIA_CONTROL);
                            matrizEvento.COLOR_RIESGO_RESIDUAL = _escalaCalificacionDao.ObtenerColorXValor(matrizEvento.RIESGO_RESIDUAL);
                            _matrizEventoRiesgoDao.SaveNoTrack(matrizEvento);


                            foreach (var itemControlEvento in _controlEventoDao.GetAll().Where(x => x.ID_EVENTO == itemEvento.ID).ToList())
                            {
                                MAT_MATRIZ_CONTROL_EVENTO matrizControlEvento = new MAT_MATRIZ_CONTROL_EVENTO();

                                //Obtener los datos de matriz control usando id de matriz y id de control anterior
                                listMatrizControl = _matrizControlDao.GetAll()
                                                   .Where(x => x.ID_MATRIZ == matriz.ID && x.ID_CONTROL_ANTERIOR == itemControlEvento.ID_CONTROL)
                                                   .ToList();


                                matrizControlEvento.ID_MATRIZ = matriz.ID;
                                matrizControlEvento.ID_EVENTO = matrizEvento.ID;
                                matrizControlEvento.ID_CONTROL = listMatrizControl.First().ID;
                                _matrizControlEventoDao.SaveNoTrack(matrizControlEvento);


                                //Eliminar relación del control con el evento actual
                                //_controlEventoDao.RemoveNoTrack(itemControlEvento.ID);

                            }


                            //Eliminar evento de riesgo
                            //_eventoRiesgoBlo.RemoveNoTrack(itemEvento.ID);
                        }


                        scope.Complete();

                    }


                    return matriz.ID;
                }
                catch (Exception e)
                {
                    log.Error("Error al generar matriz de riesgo", e);
                    throw new Exception("Error al generar matriz de riesgo", e);
                }
            }
        }


    }
}
